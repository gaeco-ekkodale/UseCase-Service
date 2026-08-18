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
using FastEndpoints;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UseCaseService.Api.DTOs;
using UseCaseService.Api.Endpoints.UseCases;
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Test.Endpoints.UseCases;

public class CreateUseCaseEndpointTests
{
    private readonly Faker _faker = new();

    private readonly IUseCasesRepository repo;

    private readonly CreateUseCaseEndpoint ep;

    public CreateUseCaseEndpointTests()
    {
        repo = Substitute.For<IUseCasesRepository>();

        var logger = Substitute.For<ILogger<CreateUseCaseEndpoint>>();

        ep = Factory.Create<CreateUseCaseEndpoint>(
            repo,
            logger);
    }

    [Fact]
    public async Task When_RequestIsValid_Then_ReturnsCreatedEntity()
    {
        // Arrange
        var req = new CreateUseCaseRequest
        {
            Name = _faker.Commerce.ProductName(),
            Description = _faker.Lorem.Sentence()
        };
        var created = new UseCase(Guid.NewGuid().ToString(), req.Name, req.Description);
        repo.CreateUseCaseAsync(Arg.Any<UseCase>()).Returns(created);

        // Act
        await ep.HandleAsync(req, CancellationToken.None);
        var rsp = ep.Response;

        // Assert
        rsp.Should().NotBeNull();
        rsp!.Id.Should().Be(created.Id);
        rsp.Name.Should().Be(req.Name);
        rsp.Description.Should().Be(req.Description);
        await repo.Received(1).CreateUseCaseAsync(Arg.Any<UseCase>());
    }

    [Fact]
    public async Task When_RequestMissingName_Then_ReturnsValidationErrors()
    {
        var req = new CreateUseCaseRequest { Name = "", Description = "desc" };

        await ep.HandleAsync(req, CancellationToken.None);

        ep.ValidationFailed.Should().BeTrue();
        ep.ValidationFailures.Should().Contain(f => f.PropertyName == nameof(CreateUseCaseRequest.Name));
        await repo.DidNotReceive().CreateUseCaseAsync(Arg.Any<UseCase>());
    }

    [Fact]
    public async Task When_RepositoryThrows_Then_ThrowsError()
    {
        repo.CreateUseCaseAsync(Arg.Any<UseCase>()).Returns<Task<UseCase>>(_ => throw new Exception("boom"));
        var req = new CreateUseCaseRequest { Name = _faker.Lorem.Word(), Description = _faker.Lorem.Sentence() };

        Func<Task> act = () => ep.HandleAsync(req, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>();
    }
}
