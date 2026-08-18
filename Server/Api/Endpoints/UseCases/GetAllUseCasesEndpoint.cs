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
using UseCaseService.Api.DTOs;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Endpoints.UseCases;

/// <summary>
/// Represents the API endpoint for retrieving all use cases.
/// </summary>
/// <remarks>
/// This endpoint does not require any request parameters and returns a collection of use cases.
/// </remarks>
public class GetAllUseCasesEndpoint : EndpointWithoutRequest<IEnumerable<UseCaseDto>>
{
    private readonly IUseCasesRepository _repo;
    private readonly ILogger<GetAllUseCasesEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllUseCasesEndpoint"/> class.
    /// </summary>
    /// <param name="repo">The repository for accessing use case data.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public GetAllUseCasesEndpoint(IUseCasesRepository repo, ILogger<GetAllUseCasesEndpoint> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Configures the endpoint's properties, including the route and summary information.
    /// </summary>
    public override void Configure()
    {
        Get("usecases");
        Summary(s =>
        {
            s.Summary = "Get all use cases";
            s.Description = "Returns all use cases";
        });
    }

    /// <summary>
    /// Handles the asynchronous request to get all use cases.
    /// </summary>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var data = await _repo.GetAllUseCasesAsync();
            var dtos = data.Select(UseCaseDto.FromDomain);
            await SendAsync(dtos, cancellation: ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all use cases");
            ThrowError("Retrieval failed");
        }
    }
}
