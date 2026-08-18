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
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Endpoints.UseCases;

/// <summary>
/// Represents the request for updating an existing use case.
/// </summary>
public class UpdateUseCaseRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the use case to be updated.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Gets or sets the new name of the use case.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the new description of the use case.
    /// </summary>
    public string Description { get; set; } = default!;
}

/// <summary>
/// Represents the API endpoint for updating an existing use case.
/// </summary>
public class UpdateUseCaseEndpoint : Endpoint<UpdateUseCaseRequest>
{
    private readonly IUseCasesRepository _repo;
    private readonly ILogger<UpdateUseCaseEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUseCaseEndpoint"/> class.
    /// </summary>
    /// <param name="repo">The repository for accessing use case data.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public UpdateUseCaseEndpoint(IUseCasesRepository repo, ILogger<UpdateUseCaseEndpoint> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Configures the endpoint's properties, including the route, summary, and response descriptions.
    /// </summary>
    public override void Configure()
    {
        Put("usecases/{id}");
        Summary(s =>
        {
            s.Summary = "Update use case";
            s.Description = "Updates an existing use case";
        });
        Description(d => d
            .Produces(404));
    }

    /// <summary>
    /// Handles the asynchronous request to update a use case.
    /// </summary>
    /// <param name="req">The request object containing the updated use case data.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task HandleAsync(UpdateUseCaseRequest req, CancellationToken ct)
    {
        try
        {
            var entity = new UseCase(req.Id, req.Name, req.Description);
            await _repo.UpdateUseCaseAsync(entity);
            await SendOkAsync(ct);
        }
        catch (OperationCanceledException)
        {
            await SendNotFoundAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update use case {Id}", req.Id);
            ThrowError("Update failed");
        }
    }
}
