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
using Microsoft.Extensions.Logging;
using NSubstitute;
using UseCaseService.Api.DTOs;
using UseCaseService.Api.Endpoints.UseCases;
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Test.Endpoints.UseCases;

public class GetUseCaseByIdEndpointTests
{
    private readonly Faker _faker = new();
    private readonly IUseCasesRepository repo;
    private readonly GetUseCaseByIdEndpoint ep;

    public GetUseCaseByIdEndpointTests()
    {
        repo = Substitute.For<IUseCasesRepository>();

        var logger = Substitute.For<ILogger<GetUseCaseByIdEndpoint>>();

        ep = Factory.Create<GetUseCaseByIdEndpoint>(
            repo,
            logger);
    }

    [Fact]
    public async Task When_IdExists_Then_ReturnsEntity()
    {
        var entity = new UseCase(Guid.NewGuid().ToString(), _faker.Commerce.ProductName(), _faker.Lorem.Sentence());
        repo.GetUseCaseAsync(entity.Id).Returns(entity);
        var req = new GetUseCaseByIdRequest { Id = entity.Id };

        await ep.HandleAsync(req, CancellationToken.None);

        var expectedDto = UseCaseDto.FromDomain(entity);
        ep.Response.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task When_IdNotFound_Then_NotFound()
    {
        repo.GetUseCaseAsync(Arg.Any<string>()).Returns<Task<UseCase>>(_ => throw new OperationCanceledException());

        await ep.HandleAsync(new GetUseCaseByIdRequest { Id = "missing" }, CancellationToken.None);

        ep.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task When_RepositoryThrows_Then_Throws()
    {
        repo.GetUseCaseAsync(Arg.Any<string>()).Returns<Task<UseCase>>(_ => throw new Exception("boom"));

        Func<Task> act = () => ep.HandleAsync(new GetUseCaseByIdRequest { Id = "x" }, CancellationToken.None);
        await act.Should().ThrowAsync<Exception>();
    }
}
