// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

namespace UseCaseService.Events.UseCases;

/// <summary>
/// Represents an event that is published when a new use case is created.
/// </summary>
/// <param name="Id">The unique identifier of the created use case.</param>
/// <param name="Name">The name of the created use case.</param>
/// <param name="Description">The description of the created use case.</param>
public record UseCaseCreated(string Id, string Name, string Description);

/// <summary>
/// Represents an event that is published when an existing use case is updated.
/// </summary>
/// <param name="Id">The unique identifier of the updated use case.</param>
/// <param name="Name">The new name of the use case.</param>
/// <param name="Description">The new description of the use case.</param>
public record UseCaseUpdated(string Id, string Name, string Description);

/// <summary>
/// Represents an event that is published when an existing use case is deleted.
/// </summary>
public record UseCaseDeleted();
