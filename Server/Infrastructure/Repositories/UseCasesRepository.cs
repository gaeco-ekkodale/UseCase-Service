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
using Microsoft.Extensions.Configuration;
using UseCaseService.Domain.Models;
using UseCaseService.Domain.Repositories;
using UseCaseService.Events.UseCases;

namespace UseCaseService.Infrastructure.Repositories;

/// <summary>
/// Implementation of the use-cases repository using Entity Framework Core.
/// </summary>
public class UseCasesRepository : IUseCasesRepository
{
    private readonly UseCaseDbContext _context;
    private readonly IOutboxRepository _outboxRepository;
    private readonly IConfiguration _configuration;
    private readonly string _useCaseTopic;

    public UseCasesRepository(UseCaseDbContext context, IOutboxRepository outboxRepository, IConfiguration configuration)
    {
        _context = context;
        _outboxRepository = outboxRepository;
        _configuration = configuration;
        _useCaseTopic = _configuration["Kafka:Topics:UseCase"] ?? throw new ArgumentNullException("Kafka:Topics:UseCase configuration is missing");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<UseCase>> GetAllUseCasesAsync()
    {
        var useCases = await _context.UseCases.ToListAsync();

        return useCases;
    }

    /// <inheritdoc />
    public async Task<UseCase> GetUseCaseAsync(string id)
    {
        var useCase = await _context.UseCases
            .SingleOrDefaultAsync(u => u.Id == id);

        if (useCase == null)
            throw new OperationCanceledException("Use case not found");

        return useCase;
    }

    /// <inheritdoc />
    public async Task<UseCase> CreateUseCaseAsync(UseCase newUseCase)
    {
        var addedUseCase = await _context.UseCases.AddAsync(newUseCase);
        _outboxRepository.Add(new UseCaseCreated(newUseCase.Id, newUseCase.Name, newUseCase.Description), _useCaseTopic, newUseCase.Id);
        await _context.SaveChangesAsync();
        return addedUseCase.Entity;
    }

    /// <inheritdoc />
    public async Task<UseCase> UpdateUseCaseAsync(UseCase newUseCase)
    {
        var useCase = await _context.UseCases
            .SingleOrDefaultAsync(u => u.Id == newUseCase.Id);

        if (useCase == null)
            throw new OperationCanceledException("Use case not found");

        useCase.Name = newUseCase.Name;
        useCase.Description = newUseCase.Description;
        _outboxRepository.Add(new UseCaseUpdated(useCase.Id, useCase.Name, useCase.Description), _useCaseTopic, useCase.Id);
        await _context.SaveChangesAsync();

        return useCase;
    }

    /// <inheritdoc />
    public async Task<UseCase> DeleteUseCaseAsync(string id)
    {
        var useCase = await _context.UseCases
            .FirstOrDefaultAsync(u => u.Id == id);

        if (useCase == null)
            throw new OperationCanceledException("Use case not found");

        var removedUseCase = _context.UseCases.Remove(useCase);
        _outboxRepository.Add(new UseCaseDeleted(), _useCaseTopic, useCase.Id);
        await _context.SaveChangesAsync();
        return removedUseCase.Entity;
    }
}
