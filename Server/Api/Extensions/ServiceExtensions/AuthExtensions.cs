// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UseCaseService.Api.Options;


namespace UseCaseService.Api.Extensions.ServiceExtensions;

/// <summary>
/// Provides extension methods for IServiceCollection with topic: Authentication and Authorization
/// </summary>
public static class AuthExtensions
{
    /// <summary>
    /// Registers JWT Bearer authentication and authorization configured from <see cref="KeycloakOptions"/> via the options pattern.
    /// Assumes <see cref="KeycloakOptions"/> has already been added with AddOptions / Configure in DI.
    /// </summary>
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(cfg =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                    cfg.RequireHttpsMetadata = !environment.IsDevelopment();
                    cfg.IncludeErrorDetails = true;
                    cfg.Authority = keycloakOptions.RealmUrl;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = keycloakOptions.RealmUrl,
                        ValidateLifetime = true,
                    };
                });

        services.AddAuthorization(options =>
        {
            if (!environment.IsDevelopment())
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            }
        });

        return services;
    }
}
