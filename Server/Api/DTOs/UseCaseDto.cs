// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using UseCaseService.Domain.Models;

namespace UseCaseService.Api.DTOs;

/// <summary>
/// Represents a Data Transfer Object (DTO) for a use case.
/// </summary>
public class UseCaseDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the use case.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the use case.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the use case.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Creates a <see cref="UseCaseDto"/> from a <see cref="UseCase"/> domain model.
    /// </summary>
    /// <param name="useCase">The use case domain model.</param>
    /// <returns>A new <see cref="UseCaseDto"/> instance populated with data from the domain model.</returns>
    public static UseCaseDto FromDomain(UseCase useCase)
    {
        return new UseCaseDto
        {
            Id = useCase.Id,
            Name = useCase.Name,
            Description = useCase.Description
        };
    }
}
