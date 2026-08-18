# Concepts

This document describes the main concepts used in the UseCase Service.

## Micro-Frontends

The Client of the Service is designed as a micro-frontend. To be able to use it, it can be uploaded into the `PluginHost` using the `PluginManager`. It can also be started locally if your Keycloak Client is configured correctly.

## UseCase Management

The `UseCase Service` is responsible for managing UseCases by applying `CRUD` operations. This includes:

- **Creating UseCases**
- **Fetch UseCases**
- **Update UseCases**
- **Delete UseCases**

### UseCases

UseCases include a Name and a Description and are referenced via Id. They are used within the platform to select the relevant data of a specific use case.

## Authentication and Authorization

Authentication and authorization are handled by Keycloak. The `PluginHost` authenticates the user and then requests an access token specifically for the `UseCase` Plugin by making a token exchange with the user token. The plugins can then use this token to authorize the user with its own backend.

## Event Driven Design with Kafka

The UseCase Service uses an event-driven architecture to communicate changes in UseCases across the system. This is implemented using [Apache Kafka](https://kafka.apache.org/) as the message broker.

### Kafka Events

Whenever a UseCase is created, updated, or deleted, a corresponding event is published to Kafka. These events allow other services to subscribe to UseCase changes, promoting loose coupling and enabling real-time reactions elsewhere in the platform.