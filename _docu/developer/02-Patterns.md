# Patterns

This document describes the design patterns used in the UseCase Service.

## Repository Pattern

The repository pattern is used in the backend to abstract the data access layer. The `UseCaseApp.Domain/Repositories/IUseCasesRepository` interface defines the methods for accessing the data, and the `UseCaseApp.Infrastructure/Repositories/UseCasesRepository` class provides the implementation. This pattern allows to easily switch the database implementation without changing the business logic.

## Options Pattern

The options pattern is used to configure the application. The `KafkaOptions`, `KeycloakOption`, and `PostgresOption` classes define the configuration options, and the `appsettings.json` file provides the values. This pattern allows to change the configuration without recompiling the application.

## Outbox Pattern

The outbox pattern is implemented to ensure reliable event publishing in case of changes to the application state. The `UseCaseApp.Domain/Repositories/IOutboxRepository` interface and its implementation in `UseCaseApp.Infrastructure/Repositories/OutboxRepository` abstract the access to the outbox table. Domain events are stored via the `Add` method as atomic part of the transaction to persist changes to the main aggregate, using the `OutboxEvent` entity.

A background service, `OutboxProcessorHostedService`, periodically creates a service scope and queries the outbox table for unprocessed events in batches (e.g. 50 at a time) using `GetUnprocessedAsync`. For each event, it uses a Kafka producer (`IKafkaProducer`) to publish the event to the configured topic. If publishing is successful, the event is removed with `Remove`. In case of an error, the retry counter is incremented (`IncrementRetry`) for that event. At the end of each batch, changes are persisted with `SaveChangesAsync`.

This implementation decouples event publishing from business transactions and guarantees reliable delivery and error handling by supporting automatic retries. Configuration for the Kafka connection is managed via the `KafkaOptions` settings class.