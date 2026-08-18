// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using FastEndpoints;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UseCaseService.Api.Endpoints.UseCases;
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Test.Endpoints.UseCases;

public class DeleteUseCaseEndpointTests
{
    private readonly IUseCasesRepository repo;
    private readonly DeleteUseCaseEndpoint ep;

    public DeleteUseCaseEndpointTests()
    {
        repo = Substitute.For<IUseCasesRepository>();

        var logger = Substitute.For<ILogger<DeleteUseCaseEndpoint>>();

        ep = Factory.Create<DeleteUseCaseEndpoint>(
            repo,
            logger);
    }

    [Fact]
    public async Task When_EntityExists_Then_ReturnsOk()
    {
        repo.DeleteUseCaseAsync(Arg.Any<string>()).Returns(new UseCase("id", "n", "d"));

        await ep.HandleAsync(new DeleteUseCaseRequest { Id = "id" }, CancellationToken.None);

        ep.HttpContext.Response.StatusCode.Should().Be(200);
        await repo.Received(1).DeleteUseCaseAsync("id");
    }

    [Fact]
    public async Task When_EntityMissing_Then_NotFound()
    {
        repo.DeleteUseCaseAsync(Arg.Any<string>()).Returns<Task<UseCase>>(_ => throw new OperationCanceledException());

        await ep.HandleAsync(new DeleteUseCaseRequest { Id = "missing" }, CancellationToken.None);

        ep.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task When_RepositoryThrows_Then_Throws()
    {
        repo.DeleteUseCaseAsync(Arg.Any<string>()).Returns<Task<UseCase>>(_ => throw new Exception("boom"));

        Func<Task> act = () => ep.HandleAsync(new DeleteUseCaseRequest { Id = "id" }, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>();
    }
}
