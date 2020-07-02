using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using KadastraalOnroerendeZakenEvents.API.Models;
using KadastraalOnroerendeZakenEvents.API.Repositories;
using KadastraalOnroerendeZakenEvents.API.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZakenEvents.API.Services.Kafka
{
    public class BackgroundConsumer : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BackgroundConsumer> logger;

        public BackgroundConsumer(IConfiguration configuration,
                                  IServiceProvider serviceProvider,
                                  ILogger<BackgroundConsumer> logger)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() =>
            {
                try
                {
                    logger.LogInformation("Enter ExecuteAsync");

                    var config = GetConsumerConfig();
                    var schemaRegistryConfig = GetSchemaRegistryConfig();

                    using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
                    using (var consumer = new ConsumerBuilder<string, Kadaster.KadastraalOnroerendeZaakEvent>(config)
                        .SetValueDeserializer(new Confluent.SchemaRegistry.Serdes.AvroDeserializer<Kadaster.KadastraalOnroerendeZaakEvent>(schemaRegistry).AsSyncOverAsync())
                        .Build())
                    {
                        consumer.Subscribe("kadastraalonroerendezaken");
                        logger.LogInformation("Subscribed to topic kadastraalonroerendezaken");

                        while (!stoppingToken.IsCancellationRequested)
                        {
                            var result = consumer.Consume();

                            if (result?.Message?.Value?.NieuweToestand != null)
                            {
                                var kozTpo = new KozTpo
                                {
                                    KozId = result.Message.Value.NieuweToestand.Id,
                                    Topic = result.Topic,
                                    Partition = result.Partition.Value,
                                    Offset = result.Offset,
                                    CreationTime = result.Message.Timestamp.UtcDateTime.ToLocalTime()
                                };
                                using (var scope = serviceProvider.CreateScope())
                                {
                                    var repository = scope.ServiceProvider.GetRequiredService<IKozTpoRepository>();

                                    var success = repository.Add(kozTpo).Result;
                                    logger.LogInformation($"Kafka => KozTpo: {kozTpo.SerializeToJson()}: {success}");
                                }
                            }
                        }

                        logger.LogInformation("Background consuming cancelled");
                        consumer.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception thrown in BackgroundConsumer.ExecuteAsync");
                }
            });

            return Task.CompletedTask;
        }

        private ConsumerConfig GetConsumerConfig(string groupId = "")
        {
            return new ConsumerConfig
            {
                GroupId = !string.IsNullOrWhiteSpace(groupId)
                    ? groupId
                    : Guid.NewGuid().ToString(),
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                EnablePartitionEof = false,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };
        }

        private SchemaRegistryConfig GetSchemaRegistryConfig()
        {
            return new SchemaRegistryConfig
            {
                Url = configuration["Kafka:SchemaRegistryServers"]
            };
        }
    }
}
