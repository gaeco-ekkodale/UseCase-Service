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
/// Represents the request for retrieving a use case by its ID.
/// </summary>
public class GetUseCaseByIdRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the use case.
    /// </summary>
    public string Id { get; set; } = default!;
}

/// <summary>
/// Represents the API endpoint for retrieving a single use case by its unique identifier.
/// </summary>
public class GetUseCaseByIdEndpoint : Endpoint<GetUseCaseByIdRequest, UseCaseDto>
{
    private readonly IUseCasesRepository _repo;
    private readonly ILogger<GetUseCaseByIdEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUseCaseByIdEndpoint"/> class.
    /// </summary>
    /// <param name="repo">The repository for accessing use case data.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public GetUseCaseByIdEndpoint(IUseCasesRepository repo, ILogger<GetUseCaseByIdEndpoint> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Configures the endpoint's properties, including the route, summary, and response descriptions.
    /// </summary>
    public override void Configure()
    {
        Get("usecases/{id}");
        Summary(s =>
        {
            s.Summary = "Get use case by id";
            s.Description = "Returns a single use case";
        });
        Description(d => d
            .Produces(404));
    }

    /// <summary>
    /// Handles the asynchronous request to get a use case by its ID.
    /// </summary>
    /// <param name="req">The request object containing the ID of the use case to retrieve.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task HandleAsync(GetUseCaseByIdRequest req, CancellationToken ct)
    {
        try
        {
            var entity = await _repo.GetUseCaseAsync(req.Id);
            var dto = UseCaseDto.FromDomain(entity);
            await SendAsync(dto, cancellation: ct);
        }
        catch (OperationCanceledException)
        {
            await SendNotFoundAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get use case {Id}", req.Id);
            ThrowError("Retrieval failed");
        }
    }
}
