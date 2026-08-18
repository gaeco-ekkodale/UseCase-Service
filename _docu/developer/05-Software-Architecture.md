# Software Architecture

This document describes the software architecture of the UseCase Service.

## Overview

The UseCase Service consists of a backend service and a frontend client. The backend is implemented with .NET 8 and is organized into several distinct projects representing different layers. It provides a REST API for managing UseCases. The frontend is a React application that consumes the API and displays the plugins.

## Backend Architecture

The backend is a modular, multi-project solution and consists of the following layers:

- **API Layer (`UseCaseApp.Api`)**:  
  This layer is responsible for handling incoming HTTP requests and sending responses. It contains controllers to process API calls and utilizes Data Transfer Objects (DTOs) for communication. The API layer is the main entry point for client interactions.
- **Domain Layer (`UseCaseApp.Domain`)**:  
  The domain layer encapsulates models and Repository Interfaces of the application.
- **Infrastructure Layer (`UseCaseApp.Infrastructure`)**:  
  This layer contains the implemented repositories and database migrations for data access.
- **Events Layer (`UseCaseApp.Events`)**:  
  This layer is responsible for the definition of application events, e.g `UseCaseCreated`.
- **Test Projects (`UseCaseApp.Api.Test`, `UseCaseApp.Infrastructure.Tests`)**:  
  These separate projects contain unit and integration tests for the respective layers.

## Frontend Architecture

The frontend is a single-page application (SPA) that is built with React. It uses the following components:

- **App**: The root component of the application.
- **StandaloneApp**: The root component of the application if working locally without pluginhost.
- **Features**: Components that define client logic.
- **Components**: The reusable components of the application.
- **API Client**: The API client that communicates with the backend.

## Communication

The frontend communicates with the backend via a REST API.