// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using FluentAssertions;
using NSubstitute;
using Bogus;
using Microsoft.Extensions.Logging;
using UseCaseService.Domain.Repositories;
using UseCaseService.Api.Endpoints.UseCases;
using UseCaseService.Domain.Models;
using FastEndpoints;

namespace UseCaseService.Api.Test.Endpoints.UseCases;

public class UpdateUseCaseEndpointTests
{
    private readonly Faker _faker = new();
    private readonly IUseCasesRepository repo;
    private readonly UpdateUseCaseEndpoint ep;

    public UpdateUseCaseEndpointTests()
    {
        repo = Substitute.For<IUseCasesRepository>();

        var logger = Substitute.For<ILogger<UpdateUseCaseEndpoint>>();

        ep = Factory.Create<UpdateUseCaseEndpoint>(
            repo,
            logger);
    }

    [Fact]
    public async Task When_EntityExists_Then_UpdatesAndReturnsOk()
    {
        var updated = new UseCase("id123", _faker.Commerce.ProductName(), _faker.Lorem.Sentence());
        repo.UpdateUseCaseAsync(Arg.Any<UseCase>()).Returns(updated);
        var req = new UpdateUseCaseRequest { Id = updated.Id, Name = updated.Name, Description = updated.Description };

        await ep.HandleAsync(req, CancellationToken.None);

        ep.HttpContext.Response.StatusCode.Should().Be(200);
        await repo.Received(1).UpdateUseCaseAsync(Arg.Is<UseCase>(u => u.Id == updated.Id));
    }

    [Fact]
    public async Task When_EntityMissing_Then_NotFound()
    {
        repo.UpdateUseCaseAsync(Arg.Any<UseCase>()).Returns<Task<UseCase>>(_ => throw new OperationCanceledException());
        var req = new UpdateUseCaseRequest { Id = "missing", Name = "n", Description = "d" };

        await ep.HandleAsync(req, CancellationToken.None);

        ep.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task When_RepositoryThrows_Then_Throws()
    {
        repo.UpdateUseCaseAsync(Arg.Any<UseCase>()).Returns<Task<UseCase>>(_ => throw new Exception("err"));
        var req = new UpdateUseCaseRequest { Id = "id", Name = "n", Description = "d" };

        Func<Task> act = () => ep.HandleAsync(req, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>();
    }
}
