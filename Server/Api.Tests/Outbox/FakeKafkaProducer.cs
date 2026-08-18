// Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
//
// This file is part of the gaeco platform system.
//
// Use of this file is governed by the terms of the license
// in LICENSE.md at the root of this repository.
// Unauthorized copying, modification, distribution, or use of this file,
// via any medium, is strictly prohibited except as expressly permitted
// under that license.

using UseCaseService.Api.Producer;

namespace UseCaseService.Api.Test.Outbox;

/// <summary>
/// Represents a message captured by a fake Kafka producer for testing purposes.
/// </summary>
public class ProducedMessage
{
    /// <summary>
    /// Gets the topic the message was produced to.
    /// </summary>
    public string Topic { get; init; } = string.Empty;

    /// <summary>
    /// Gets the key of the produced message.
    /// </summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// Gets the value (payload) of the produced message.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Gets the headers of the produced message.
    /// </summary>
    public IDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();
}

/// <summary>
/// Represents a fake implementation of <see cref="IKafkaProducer"/> for testing purposes.
/// </summary>
/// <remarks>
/// This producer does not send messages to Kafka. Instead, it stores them in the <see cref="Messages"/> list for inspection during tests.
/// </remarks>
public class FakeKafkaProducer : IKafkaProducer
{
    /// <summary>
    /// Gets the list of messages that have been "produced" to this fake producer.
    /// </summary>
    public List<ProducedMessage> Messages { get; } = new();

    /// <summary>
    /// Adds a message to the in-memory <see cref="Messages"/> list instead of sending it to Kafka.
    /// </summary>
    /// <param name="topic">The topic of the message.</param>
    /// <param name="key">The key of the message.</param>
    /// <param name="value">The value of the message.</param>
    /// <param name="headers">The headers of the message.</param>
    /// <param name="cancellationToken">This parameter is ignored in this fake implementation.</param>
    /// <returns>A completed <see cref="Task"/>.</returns>
    public Task ProduceAsync(string topic, string key, string? value, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        Messages.Add(new ProducedMessage
        {
            Topic = topic,
            Key = key,
            Value = value,
            Headers = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>()
        });
        return Task.CompletedTask;
    }
}
