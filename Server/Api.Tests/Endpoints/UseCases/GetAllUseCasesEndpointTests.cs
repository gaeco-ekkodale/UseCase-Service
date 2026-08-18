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

public class GetAllUseCasesEndpointTests
{
    private readonly Faker _faker = new();
    private readonly IUseCasesRepository repo;
    private readonly GetAllUseCasesEndpoint ep;

    public GetAllUseCasesEndpointTests()
    {
        repo = Substitute.For<IUseCasesRepository>();
        repo = Substitute.For<IUseCasesRepository>();

        var logger = Substitute.For<ILogger<GetAllUseCasesEndpoint>>();

        ep = Factory.Create<GetAllUseCasesEndpoint>(
            repo,
            logger);
    }

    [Fact]
    public async Task When_RepositoryHasData_Then_ReturnsAll()
    {
        var list = Enumerable.Range(0, 5).Select(_ => new UseCase(Guid.NewGuid().ToString(), _faker.Commerce.ProductName(), _faker.Lorem.Sentence())).ToList();
        repo.GetAllUseCasesAsync().Returns(list);

        await ep.HandleAsync(CancellationToken.None);

        var expectedDtos = list.Select(UseCaseDto.FromDomain);
        ep.Response.Should().BeEquivalentTo(expectedDtos);
        await repo.Received(1).GetAllUseCasesAsync();
    }

    [Fact]
    public async Task When_RepositoryThrows_Then_Throws()
    {
        repo.GetAllUseCasesAsync().Returns<Task<IEnumerable<UseCase>>>(_ => throw new Exception("err"));

        Func<Task> act = () => ep.HandleAsync(CancellationToken.None);
        await act.Should().ThrowAsync<Exception>();
    }
}
