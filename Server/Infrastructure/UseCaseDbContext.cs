// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using Microsoft.EntityFrameworkCore;
using UseCaseService.Domain.Models;

namespace UseCaseService.Infrastructure;

/// <summary>
/// Represents the database context for the application, managing use cases and outbox events.
/// </summary>
/// <remarks>
/// This class is the primary entry point for interacting with the underlying database
/// using Entity Framework Core.
/// </remarks>
public class UseCaseDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for the <see cref="UseCase"/> entities.
    /// </summary>
    public DbSet<UseCase> UseCases { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> for the <see cref="OutboxEvent"/> entities.
    /// </summary>
    public DbSet<OutboxEvent> OutboxEvents { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UseCaseDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public UseCaseDbContext(DbContextOptions<UseCaseDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Configures the schema needed for the context's model.
    /// </summary>
    /// <remarks>
    /// This method is called by the framework to build the model and its mappings.
    /// It defines the primary keys ( the guid's ) and property configurations for the entities.
    /// </remarks>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UseCase>().HasKey(x => x.Id);
        modelBuilder.Entity<OutboxEvent>().HasKey(x => x.Id);
        modelBuilder.Entity<OutboxEvent>()
            .Property(p => p.Payload)
            .HasColumnType("text")
            .IsRequired(false);
        base.OnModelCreating(modelBuilder);
    }
}