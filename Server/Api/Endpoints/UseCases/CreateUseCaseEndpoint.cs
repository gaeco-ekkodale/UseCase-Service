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
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;

namespace UseCaseService.Api.Endpoints.UseCases;

/// <summary>
/// Represents the request data for creating a new use case.
/// </summary>
public class CreateUseCaseRequest
{
    /// <summary>
    /// Gets or sets the name for the new use case.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Gets or sets the description for the new use case.
    /// </summary>
    public string Description { get; set; } = default!;
}

/// <summary>
/// An endpoint for creating a new use case.
/// </summary>
public class CreateUseCaseEndpoint : Endpoint<CreateUseCaseRequest, UseCaseDto>
{
    private readonly IUseCasesRepository _repo;
    private readonly ILogger<CreateUseCaseEndpoint> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUseCaseEndpoint"/> class.
    /// </summary>
    /// <param name="repo">The use case repository.</param>
    /// <param name="logger">The logger instance.</param>
    public CreateUseCaseEndpoint(IUseCasesRepository repo, ILogger<CreateUseCaseEndpoint> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    /// <summary>
    /// Configures the endpoint's properties, like its route and summary.
    /// </summary>
    public override void Configure()
    {
        Post("usecases");
        Summary(s =>
        {
            s.Summary = "Create a new use case";
            s.Description = "Creates a new use case and returns its data";
        });
    }

    /// <summary>
    /// Handles the HTTP request to create a new use case.
    /// </summary>
    /// <param name="req">The request object containing the use case data.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public override async Task HandleAsync(CreateUseCaseRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Description))
        {
            AddError(r => r.Name, "Name and Description are required");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        try
        {
            var entity = new UseCase(Guid.NewGuid().ToString(), req.Name, req.Description);
            var created = await _repo.CreateUseCaseAsync(entity);
            var dto = UseCaseDto.FromDomain(created);
            await SendAsync(dto, cancellation: ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create use case");
            ThrowError("Creation failed");
        }
    }
}
