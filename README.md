<div align="center">
  <img src="https://raw.githubusercontent.com/gaeco-ekkodale/.github/main/assets/gaeco_logo_horizontal_color.png" width="200" alt="gaeco logo">

  # UseCaseService

  <em>Manages use cases - the context that scopes which building data and which access rights apply.</em>

  [![License](https://img.shields.io/badge/license-fair--code-blue.svg)](LICENSE.md)
  [![Version](https://img.shields.io/github/v/release/gaeco-ekkodale/UseCase-Service)](../../releases)

  [gaeco-ekkodale Organization](https://github.com/gaeco-ekkodale) · [All Repos](https://github.com/orgs/gaeco-ekkodale/repositories)
</div>

---

gaeco (Graphs for Architecture, Engineering, Construction, Operations) is an event-driven microservice platform for BIM data management. It translates external building-industry standards (IFC, IBPDI, Brick Schema, ASHRAE 223 and others) into a shared, versioned classification and relationship model (Guideline + Ontology) and exposes consistent, graph-based building data (Instance) across use cases and departments — without forcing every consumer onto one rigid schema. Built for organizations managing building/portfolio data across disconnected departmental systems (construction, facilities management, leasing, accounting) that need automatic, reliable data propagation instead of manual, error-prone hand-offs.

> This project is licensed under the [Source Available](LICENSE.md). Source code is viewable and usable; commercial use is restricted.

---

## What this service does

A use case is gaeco's unit of context. It carries a name, a description and an id, and it is what allows the platform to serve different departments from one shared model without forcing them onto one rigid schema: the same classification can be visible with different properties and different permissions depending on the use case a user is working in.

The UseCaseService manages these use cases via CRUD operations and publishes every change as an event. Use case ids are referenced across the platform — most importantly by the [AccessService](https://github.com/gaeco-ekkodale/AccessService), where an access right is only unique in combination with a use case, and by the [InstanceService](https://github.com/gaeco-ekkodale/InstanceService), which manages its graph data per use case.

## Repository Structure

- `Server/Api/`: ASP.NET Core Web API
- `Server/Domain/`: domain models and contracts
- `Server/Infrastructure/`: EF Core data access and Kafka integration
- `Server/Events/`: Kafka event contracts
- `Server/Api.Tests/`, `Server/Infrastructure.Tests/`: unit tests
- `Client/`: React micro-frontend, exposed via Module Federation
- `_docker/`: Compose definition, env schemas and the App Registry package manifest
- `_docu/`: developer and user documentation
- `_pipeline/`: Azure DevOps CI/CD pipeline definitions
- `build/`: NUKE build scripts

## Tech Stack

- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core
- **Frontend**: React, TypeScript, Vite, Material UI, material-react-table, Tailwind CSS, React Query, Module Federation
- **Infrastructure**: PostgreSQL, Apache Kafka, Keycloak, Docker
- **Build**: NUKE

## Local Development

### Prerequisites

- Docker Desktop
- .NET 8 SDK
- Node.js 20+
- The shared platform infrastructure (Keycloak, Kafka) plus PluginHost and AppOrchestrator — see [`_docu/user/01-Installation.md`](_docu/user/01-Installation.md)

### Start with Docker Compose

```bash
cd _docker
docker compose -p usecase-service -f docker-compose.yml -f docker-compose-override.yml up -d
```

Ports are driven by the `USECASE_*_OUTERPORT` variables in the environment files; the API exposes Swagger at `/swagger`.

### Run the client locally

```bash
cd Client
npm ci
npm run dev
```

The client is a micro-frontend. In an integrated setup the `usecase-client` container publishes its micro-frontend metadata, which the AppOrchestrator discovers and binds into the PluginHost automatically.

## Build and Test

```bash
./build.sh     # Linux/macOS
.\build.ps1    # Windows
```

- Backend tests: `dotnet test` from the repository root
- Frontend build: `npm run build` in `Client/`

## Integration

- **Authentication**: Keycloak (OIDC/JWT). The PluginHost authenticates the user and performs a token exchange to obtain a token scoped to the use case plugin, which the plugin then uses against this service's API.
- **Events**: creating, updating or deleting a use case publishes a corresponding event to Apache Kafka, keeping consuming services loosely coupled and up to date.

## Documentation

- [Concepts](_docu/developer/01-Concepts.md)
- [Patterns](_docu/developer/02-Patterns.md)
- [Used Technologies](_docu/developer/03-Used-Technologies.md)
- [Data Model](_docu/developer/04-Data-Model.md)
- [Software Architecture](_docu/developer/05-Software-Architecture.md)
- [Installation](_docu/user/01-Installation.md) · [User Manual](_docu/user/02-User-Manual.md)
