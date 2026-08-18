// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using Ekkodale.TelemetryExtensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UseCaseService.Api.Extensions.ServiceExtensions;
using UseCaseService.Api.Options;
using UseCaseService.Api.Producer;
using UseCaseService.Domain.Repositories;
using UseCaseService.Infrastructure;
using UseCaseService.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddOptions<PostgresOptions>()
    .Bind(builder.Configuration.GetSection(PostgresOptions.Postgres))
    .ValidateDataAnnotations();

builder.Services.AddOptions<KeycloakOptions>()
    .Bind(builder.Configuration.GetSection(KeycloakOptions.Keycloak))
    .ValidateDataAnnotations();

builder.Services.AddOptions<KafkaOptions>()
    .Bind(builder.Configuration.GetSection(KafkaOptions.SectionName))
    .ValidateDataAnnotations();

TelemetryOptions? telOpts = configuration.GetSection("OpenTelemetry").Get<TelemetryOptions>();
if (telOpts is null)
    throw new InvalidOperationException("OpenTelemetry configuration is missing");
builder.AddMonitoring(telOpts, Assembly.GetExecutingAssembly());

builder.Services.AddPostgres();
builder.Services.AddScoped<IUseCasesRepository, UseCasesRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddHostedService<UseCaseService.Api.Services.OutboxProcessorHostedService>();

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()  // Allowing any origin
            .AllowAnyMethod()  // Allowing any HTTP method
            .AllowAnyHeader()); // Allowing any header
});

builder.Services.AddKeycloakAuthentication(builder.Environment);

WebApplication app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors("AllowAllOrigins");

// Respect reverse proxy headers (Traefik) for scheme/host
var fwdOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
};
fwdOptions.KnownNetworks.Clear();
fwdOptions.KnownProxies.Clear();
app.UseForwardedHeaders(fwdOptions);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.ShortNames = true;
    c.Errors.UseProblemDetails();
    c.Errors.ProducesMetadataType = typeof(ProblemDetails);
    c.Endpoints.Configurator = ep =>
    {
        ep.Description(d =>
            d.Produces<ProblemDetails>(400, "application/problem+json")
             .Produces(401)
             .Produces(403));
    };
});
app.UseSwaggerGen();

app.MapHealthChecks("health").AllowAnonymous();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UseCaseDbContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();
