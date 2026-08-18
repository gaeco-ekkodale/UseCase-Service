// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;
using UseCaseService.Infrastructure.Repositories;

namespace UseCaseService.Infrastructure.Tests.Repositories;

public class UseCasesRepositoryTests : IDisposable
{
    private static Faker<UseCase> FakerUseCase => new Faker<UseCase>()
        .CustomInstantiator(f => new UseCase(
            f.Random.AlphaNumeric(32),
            f.Commerce.ProductName(),
            f.Lorem.Sentence(5)));

    private readonly UseCaseDbContext _context;
    private readonly IOutboxRepository _outboxRepo;
    private readonly UseCasesRepository _repo;
    private readonly IConfiguration _configuration;

    public UseCasesRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UseCaseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _configuration = Substitute.For<IConfiguration>();
        _configuration["Kafka:Topics:UseCase"].Returns("usecase-topic");

        _context = new UseCaseDbContext(options);
        _outboxRepo = new OutboxRepository(_context);
        _repo = new UseCasesRepository(_context, _outboxRepo, _configuration);
    }

    [Fact]
    public async Task When_CreatingUseCase_Then_UseCasePersistedAndOutboxEventAdded()
    {
        var useCase = FakerUseCase.Generate();

        var created = await _repo.CreateUseCaseAsync(useCase);

        created.Should().NotBeNull();
        (await _context.UseCases.CountAsync()).Should().Be(1);
        (await _context.OutboxEvents.CountAsync()).Should().Be(1);
        var evt = await _context.OutboxEvents.FirstAsync();
        evt.EventType.Should().Be("UseCaseCreated");
        evt.AggregateId.Should().Be(useCase.Id);
    }

    [Fact]
    public async Task When_UpdatingExistingUseCase_Then_UseCaseUpdatedAndOutboxEventAdded()
    {
        var original = FakerUseCase.Generate();
        await _repo.CreateUseCaseAsync(original);

        original.Name = "Updated Name";
        original.Description = "Updated Description";

        var updated = await _repo.UpdateUseCaseAsync(original);

        updated.Name.Should().Be("Updated Name");
        updated.Description.Should().Be("Updated Description");
        (await _context.OutboxEvents.CountAsync()).Should().Be(2); // Created + Updated
        (await _context.OutboxEvents.Select(e => e.EventType).ToListAsync())
            .Should().Contain(new[] { "UseCaseCreated", "UseCaseUpdated" });
    }

    [Fact]
    public async Task When_DeletingExistingUseCase_Then_UseCaseRemovedAndOutboxEventAdded()
    {
        var uc = FakerUseCase.Generate();
        await _repo.CreateUseCaseAsync(uc);

        var deleted = await _repo.DeleteUseCaseAsync(uc.Id);

        deleted.Id.Should().Be(uc.Id);
        (await _context.UseCases.AnyAsync()).Should().BeFalse();
        (await _context.OutboxEvents.CountAsync()).Should().Be(2); // Created + Deleted
        var types = await _context.OutboxEvents.Select(e => e.EventType).ToListAsync();
        types.Should().Contain(new[] { "UseCaseCreated", "UseCaseDeleted" });
    }

    [Fact]
    public async Task When_GettingAllUseCases_Then_ReturnsAllPersisted()
    {
        var list = FakerUseCase.Generate(3);
        foreach (var uc in list)
        {
            await _repo.CreateUseCaseAsync(uc);
        }

        var all = (await _repo.GetAllUseCasesAsync()).ToList();
        all.Should().HaveCount(3);
        all.Select(a => a.Id).Should().BeEquivalentTo(list.Select(l => l.Id));
    }

    [Fact]
    public async Task When_GettingNonExistingUseCase_Then_ThrowsOperationCanceledException()
    {
        var act = async () => await _repo.GetUseCaseAsync(Guid.NewGuid().ToString("N"));

        await act.Should().ThrowAsync<OperationCanceledException>()
            .WithMessage("Use case not found");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
