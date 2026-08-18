# Data Model

This document describes the data model of the UseCase Service.

## OutboxEvent

The `OutboxEvent` represents a domain-specific event that is persisted for later processing as part of the outbox pattern. It has two constructors, one for EF Core and a public one. The following properties are contained:

- **Id** (`Guid`): The unique identifier of the event.
- **AggregateId** (`string`, max. 40 characters): The identifier of the aggregate (domain entity) that the event relates to.
- **EventType** (`string`, max. 200 characters): The type of the event. Can be used to distinguish between different kinds of events.
- **OccurredOn** (`DateTimeOffset`): The timestamp of when the event occurred.
- **Payload** (`string?`): An optional serialized data payload of the event.
- **RetryCount** (`int`): The number of attempts made to process the event in case of previous failures. It always starts at `0` if the public constructor was used.

## UseCase

The `UseCase` represents a distinct use case within the system. It has the following properties:

- **Id** (`string`, max. 40 characters): The unique identifier of the use case.
- **Name** (`string`, max. 150 characters): The name of the use case.
- **Description** (`string`, max. 500 characters): A description of the use case.

If any of these are null on initialization via constructor, an `ArgumentNullException` is thrown.

# DTOs

## UseCaseDto

The `UseCaseDto` is a Data Transfer Object used for transmitting `UseCase` data between the API and its consumers.

### Properties

- **Id** (`string`): The unique identifier of the use case.
- **Name** (`string`): The name of the use case.
- **Description** (`string`): A description of the use case.