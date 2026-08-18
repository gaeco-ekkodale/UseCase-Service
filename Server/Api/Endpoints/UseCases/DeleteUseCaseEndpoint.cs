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
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Endpoints.UseCases;

/// <summary>
/// Represents the request for deleting a use case.
/// </summary>
public class DeleteUseCaseRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the use case to be deleted.
    /// </summary>
    public string Id { get; set; } = default!;
}

/// <summary>
/// Represents the API endpoint for deleting a use case.
/// </summary>
public class DeleteUseCaseEndpoint : Endpoint<DeleteUseCaseRequest>
{
    private readonly IUseCasesRepository _repo;
    private readonly ILogger<DeleteUseCaseEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteUseCaseEndpoint"/> class.
    /// </summary>
    /// <param name="repo">The repository for accessing use case data.</param>
    /// <param name="logger">The logger for logging information and errors.</param>
    public DeleteUseCaseEndpoint(IUseCasesRepository repo, ILogger<DeleteUseCaseEndpoint> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Configures the endpoint's properties, including the route, summary, and response descriptions.
    /// </summary>
    public override void Configure()
    {
        Delete("usecases/{id}");
        Summary(s =>
        {
            s.Summary = "Delete use case";
            s.Description = "Deletes an existing use case";
        });
        Description(d => d
            .Produces(404));
    }

    /// <summary>
    /// Handles the asynchronous request to delete a use case.
    /// </summary>
    /// <param name="req">The request object containing the ID of the use case to delete.</param>
    /// <param name="ct">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task HandleAsync(DeleteUseCaseRequest req, CancellationToken ct)
    {
        try
        {
            await _repo.DeleteUseCaseAsync(req.Id);
            await SendOkAsync(ct);
        }
        catch (OperationCanceledException)
        {
            await SendNotFoundAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete use case {Id}", req.Id);
            ThrowError("Deletion failed");
        }
    }
}
