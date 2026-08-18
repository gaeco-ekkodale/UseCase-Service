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

namespace UseCaseService.Domain.Repositories;

/// <summary>
/// Repository interface for managing use-case entities.
/// </summary>
public interface IUseCasesRepository
{
    /// <summary>
    /// Retrieves all use-cases from the repository.
    /// </summary>
    /// <returns>A collection of all use-cases.</returns>
    Task<IEnumerable<UseCase>> GetAllUseCasesAsync();
    
    /// <summary>
    /// Retrieves a specific use-case by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the use-case to retrieve.</param>
    /// <returns>The use-case with the specified identifier.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the use-case is not found.</exception>
    Task<UseCase> GetUseCaseAsync(string id);
    
    /// <summary>
    /// Creates a new use-case in the repository.
    /// </summary>
    /// <param name="newUseCase">The use-case to create.</param>
    /// <returns>The created use-case.</returns>
    Task<UseCase> CreateUseCaseAsync(UseCase newUseCase);
    
    /// <summary>
    /// Updates an existing use-case in the repository.
    /// </summary>
    /// <param name="newUseCase">The use-case with updated information.</param>
    /// <returns>The updated use-case.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the use-case is not found.</exception>
    Task<UseCase> UpdateUseCaseAsync(UseCase newUseCase);
    
    /// <summary>
    /// Deletes a use-case from the repository by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the use-case to delete.</param>
    /// <returns>The deleted use-case.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the use-case is not found.</exception>
    Task<UseCase> DeleteUseCaseAsync(string id);
}
