// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UseCaseService.Domain.Models;

[Table("usecase")]
public class UseCase
{
    /// <summary>
    /// Primary identifier for the use-case.
    /// </summary>
    [Required]
    [MaxLength(40)]
    [Column("id")]
    public string Id { get; set; }
    /// <summary>
    /// The name of the use-case.
    /// </summary>
    [Required]
    [MaxLength(150)]
    [Column("name")]
    public string Name { get; set; }
    /// <summary>
    /// The description of the use-case.
    /// </summary>
    [Required]
    [MaxLength(500)]
    [Column("description")]
    public string Description { get; set; }

    /// <summary>
    /// The constructor for the use-case.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UseCase(string id, string name, string description)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
}
