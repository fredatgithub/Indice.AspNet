﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Indice.Hosting.Tasks.Data;
using Indice.Hosting.Tasks.Data.Extensions;
using Indice.Hosting.Tasks.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Indice.Hosting.Tasks.Implementations
{
    /// <summary>
    /// An implementation of <see cref="IMessageQueue{T}"/> for relational backend. Supports PostgreSQL and SQL Server through EntityFramework.
    /// </summary>
    /// <typeparam name="T">The type of message.</typeparam>
    public class RelationalMessageQueue<T> : IMessageQueue<T> where T : class
    {
        private readonly TaskDbContext _dbContext;
        private readonly IQueueNameResolver<T> _queueNameResolver;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly MessageQueueQueryDescriptor _queryDescriptor;

        /// <summary>
        /// Constructs a new <see cref="RelationalMessageQueue{T}"/>.
        /// </summary>
        /// <param name="dbContext">A <see cref="DbContext"/> for hosting multiple <see cref="IMessageQueue{T}"/>.</param>
        /// <param name="queueNameResolver">Resolves the queue name.</param>
        /// <param name="workerJsonOptions">These are the options regarding json Serialization. They are used internally for persisting payloads.</param>
        public RelationalMessageQueue(TaskDbContext dbContext, IQueueNameResolver<T> queueNameResolver, WorkerJsonOptions workerJsonOptions) {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _queueNameResolver = queueNameResolver ?? throw new ArgumentNullException(nameof(queueNameResolver));
            _jsonSerializerOptions = workerJsonOptions?.JsonSerializerOptions ?? throw new ArgumentNullException(nameof(workerJsonOptions));
            _queryDescriptor = new MessageQueueQueryDescriptor(dbContext);
        }

        /* We do not need to implement this method here, since when an item is dequeued it is also removed at the same time. */
        /// <inheritdoc/>
        public Task Cleanup(int? batchSize = null) => Task.CompletedTask;

        /// <inheritdoc/>
        public Task<int> Count() {
            var dbConnection = _dbContext.Database.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.AddParameterWithValue("@QueueName", _queueNameResolver.Resolve(), DbType.String);
            command.CommandText = _queryDescriptor.Count;
            command.CommandType = CommandType.Text;
            var count = command.ExecuteScalar();
            return Task.FromResult((int)count);
        }

        /// <inheritdoc/>
        public Task<QMessage<T>> Dequeue() {
            var dbConnection = _dbContext.Database.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.AddParameterWithValue("@QueueName", _queueNameResolver.Resolve(), DbType.String);
            command.CommandText = _queryDescriptor.Dequeue;
            command.CommandType = CommandType.Text;
            using var dataReader = command.ExecuteReader();
            DbQMessage message = null;
            while (dataReader.Read()) {
                message = new DbQMessage {
                    Id = dataReader.GetGuid(0),
                    QueueName = dataReader.IsDBNull(1) ? default : dataReader.GetString(1),
                    Payload = dataReader.IsDBNull(2) ? default : dataReader.GetString(2),
                    Date = dataReader.GetDateTime(3),
                    RowVersion = dataReader.IsDBNull(4) ? default : (byte[])dataReader[nameof(DbQMessage.RowVersion)],
                    DequeueCount = dataReader.GetInt32(5),
                    State = (QMessageState)dataReader.GetInt32(6)
                };
            }
            return Task.FromResult(message != null ? message.ToModel<T>(_jsonSerializerOptions) : default);
        }

        /// <inheritdoc/>
        public Task Enqueue(QMessage<T> item, bool isPoison) {
            var dbConnection = _dbContext.Database.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.AddParameterWithValue("@Id", item.Id, DbType.Guid);
            command.AddParameterWithValue("@QueueName", _queueNameResolver.Resolve(isPoison), DbType.String);
            command.AddParameterWithValue("@Payload", JsonSerializer.Serialize(item.Value, _jsonSerializerOptions), DbType.String);
            command.AddParameterWithValue("@Date", item.Date, DbType.DateTime2);
            command.AddParameterWithValue("@DequeueCount", item.DequeueCount, DbType.Int32);
            command.CommandText = _queryDescriptor.Enqueue;
            command.CommandType = CommandType.Text;
            var rowsAffected = command.ExecuteNonQuery();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task EnqueueRange(IEnumerable<T> items) {
            var query = new StringBuilder(_queryDescriptor.EnqueueRangeInsertStatement);
            var dbConnection = _dbContext.Database.GetDbConnection();
            dbConnection.Open();
            const double maxBatchSize = 1000d;
            var batches = Math.Ceiling(items.Count() / maxBatchSize);
            for (var i = 0; i < batches; i++) {
                var remainingItemsCount = items.Count() - i * maxBatchSize;
                var iterationLength = remainingItemsCount >= maxBatchSize ? maxBatchSize : remainingItemsCount;
                using var command = dbConnection.CreateCommand();
                for (var j = 0; j < iterationLength; j++) {
                    query.AppendFormat(_queryDescriptor.EnqueueRangeValuesStatement, j);
                    var isLastItem = j == iterationLength - 1;
                    query.Append(!isLastItem ? ", " : ";");
                    command.AddParameterWithValue($"@QueueName{j}", _queueNameResolver.Resolve(), DbType.String);
                    command.AddParameterWithValue($"@Payload{j}", JsonSerializer.Serialize(items.ElementAt(j), _jsonSerializerOptions), DbType.String);
                }
                command.CommandText = query.ToString();
                command.CommandType = CommandType.Text;
                var rowsAffected = command.ExecuteNonQuery();
                query.Clear();
                query.Append(_queryDescriptor.EnqueueRangeInsertStatement);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<T> Peek() {
            var dbConnection = _dbContext.Database.GetDbConnection();
            dbConnection.Open();
            using var command = dbConnection.CreateCommand();
            command.CommandText = _queryDescriptor.Peek;
            command.CommandType = CommandType.Text;
            using var dataReader = command.ExecuteReader();
            string payload = null;
            while (dataReader.Read()) {
                payload = dataReader.IsDBNull(0) ? default : dataReader.GetString(2);
            }
            return Task.FromResult(!string.IsNullOrEmpty(payload) ? JsonSerializer.Deserialize<T>(payload, _jsonSerializerOptions) : default);
        }
    }
    internal class MessageQueueQueryDescriptor
    {
        public MessageQueueQueryDescriptor(DbContext context) {
            switch (context.Database.ProviderName) {
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    Count = PostgreSqlMessageQueueQueries.Count;
                    Dequeue = PostgreSqlMessageQueueQueries.Dequeue;
                    Enqueue = PostgreSqlMessageQueueQueries.Enqueue;
                    EnqueueRangeInsertStatement = PostgreSqlMessageQueueQueries.EnqueueRangeInsertStatement;
                    EnqueueRangeValuesStatement = PostgreSqlMessageQueueQueries.EnqueueRangeValuesStatement;
                    Peek = PostgreSqlMessageQueueQueries.Peek;
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                default:
                    Count = SqlServerMessageQueueQueries.Count;
                    Dequeue = SqlServerMessageQueueQueries.Dequeue;
                    Enqueue = SqlServerMessageQueueQueries.Enqueue;
                    EnqueueRangeInsertStatement = SqlServerMessageQueueQueries.EnqueueRangeInsertStatement;
                    EnqueueRangeValuesStatement = SqlServerMessageQueueQueries.EnqueueRangeValuesStatement;
                    Peek = SqlServerMessageQueueQueries.Peek;
                    break;
            }
        }

        public string Count { get; }
        public string Dequeue { get; }
        public string Enqueue { get; }
        public string EnqueueRangeInsertStatement { get; }
        public string EnqueueRangeValuesStatement { get; }
        public string Peek { get; }
    }

    internal static class SqlServerMessageQueueQueries
    {
        public const string Count = @"
            SELECT COUNT(*)
            FROM [work].[QMessage]
            WHERE [QueueName] = @QueueName;";
        public const string Dequeue = @"
            SET NOCOUNT ON; 
            WITH cte AS (
                SELECT TOP(1) * 
                FROM [work].[QMessage] WITH (ROWLOCK, READPAST)
                WHERE [QueueName] = @QueueName
                ORDER BY [Date] ASC
            )
            DELETE FROM cte 
            OUTPUT [deleted].*;";
        public const string Enqueue = @"
            INSERT INTO [work].[QMessage] ([Id], [QueueName], [Payload], [Date], [DequeueCount], [State]) 
            VALUES (@Id, @QueueName, @Payload, @Date, @DequeueCount, 0);";
        public const string EnqueueRangeInsertStatement = @"INSERT INTO [work].[QMessage] ([Id], [QueueName], [Payload], [Date], [DequeueCount], [State]) VALUES";
        public const string EnqueueRangeValuesStatement = @"(NEWID(), @QueueName{0}, @Payload{0}, GETUTCDATE(), 0, 0)";
        public const string Peek = @"
            SELECT TOP(1) [Payload] 
            FROM [work].[QMessage] WITH (ROWLOCK, READPAST) 
            ORDER BY [Date] ASC;";
    }

    internal static class PostgreSqlMessageQueueQueries
    {
        public const string Count = @"
            SELECT COUNT(*) 
            FROM ""work"".""QMessage""
            WHERE ""QueueName"" = '@QueueName';";
        public const string Dequeue = @"
            DELETE FROM ""work"".""QMessage""
            USING(
                SELECT *
                FROM ""work"".""QMessage""
                WHERE ""QueueName"" = @QueueName
                LIMIT 1
                FOR UPDATE SKIP LOCKED
            ) q
            WHERE q.""Id"" = ""work"".""QMessage"".""Id""
            RETURNING ""work"".""QMessage"".*;";
        public const string Enqueue = @"
            INSERT INTO ""work"".""QMessage"" (""Id"", ""QueueName"", ""Payload"", ""Date"", ""DequeueCount"", ""State"")
            VALUES (@Id, @QueueName, @Payload, @Date, @DequeueCount, 0);";
        public const string EnqueueRangeInsertStatement = @"INSERT INTO ""work"".""QMessage"" (""Id"", ""QueueName"", ""Payload"", ""Date"", ""DequeueCount"", ""State"") VALUES";
        public const string EnqueueRangeValuesStatement = @"(MD5(RANDOM()::TEXT || CLOCK_TIMESTAMP()::TEXT)::UUID, @QueueName{0}, @Payload{0}, NOW(), 0, 0)";
        public const string Peek = @"
            SELECT *
            FROM ""work"".""QMessage""
            LIMIT 1
            FOR UPDATE SKIP LOCKED;";
    }
}