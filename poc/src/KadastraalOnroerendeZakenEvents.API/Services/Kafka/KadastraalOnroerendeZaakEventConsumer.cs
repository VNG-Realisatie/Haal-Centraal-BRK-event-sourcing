using AutoMapper;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KadastraalOnroerendeZakenEvents.API.Services.Kafka
{
    public class KadastraalOnroerendeZaakEventConsumer : IKadastraalOnroerendeZaakEventConsumer
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<KadastraalOnroerendeZaakEventConsumer> logger;
        private readonly IMapper mapper;

        public KadastraalOnroerendeZaakEventConsumer(IConfiguration configuration,
                               ILogger<KadastraalOnroerendeZaakEventConsumer> logger,
                               IMapper mapper)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.mapper = mapper;
        }

        public IEnumerable<Contracts.KadastraalOnroerendeZaakEvent> Consume(string consumerId, IEnumerable<string> topics, DateTimeOffset? van, bool includeVorigToestand)
        {
            var retval = new List<Contracts.KadastraalOnroerendeZaakEvent>();

            var config = GetConsumerConfig(consumerId);
            var schemaRegistryConfig = GetSchemaRegistryConfig();

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var consumer = new ConsumerBuilder<string, Kadaster.KadastraalOnroerendeZaakEvent>(config)
                .SetValueDeserializer(new AvroDeserializer<Kadaster.KadastraalOnroerendeZaakEvent>(schemaRegistry).AsSyncOverAsync())
                .Build())
            {
                consumer.SetSubscription(topics, van, logger);

                while (true)
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(5));

                    if (result == null || result.IsPartitionEOF)
                    {
                        break;
                    }

                    var data = mapper.Map<Contracts.KadastraalOnroerendeZaakEvent>(result);
                    if (includeVorigToestand &&
                        !string.IsNullOrWhiteSpace(result.Message.Value.VorigEventIdentificatie))
                    {
                        data.VorigToestandKadastraalOnroerendeZaak =
                            Consume(result.Message.Value.VorigEventIdentificatie).NieuweToestandKadastraalOnroerendeZaak;
                    }

                    retval.Add(data);
                }

                consumer.Close();
            }

            return retval;
        }

        public Contracts.KadastraalOnroerendeZaakEvent Consume(string identificatie, bool includeVorigToestand)
        {
            var (topic, partition, offset) = identificatie.ParseIdentificatie();

            return Consume(topic, partition, offset, includeVorigToestand);
        }

        public Contracts.KadastraalOnroerendeZaakEvent Consume(string topic, int partition, long offset, bool includeVorigToestand)
        {
            var config = GetConsumerConfig();
            var schemaRegistryConfig = GetSchemaRegistryConfig();

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var consumer = new ConsumerBuilder<string, Kadaster.KadastraalOnroerendeZaakEvent>(config)
                .SetValueDeserializer(new AvroDeserializer<Kadaster.KadastraalOnroerendeZaakEvent>(schemaRegistry).AsSyncOverAsync())
                .Build())
            {
                var retval = consumer.Consume(topic, partition, offset, includeVorigToestand, mapper);
                consumer.Close();
                return retval;
            }
        }

        private Contracts.KadastraalOnroerendeZaakEvent Consume(string identificatie)
        {
            var (topic, partition, offset) = identificatie.ParseIdentificatie();

            var config = GetConsumerConfig();
            var schemaRegistryConfig = GetSchemaRegistryConfig();

            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var consumer = new ConsumerBuilder<string, Kadaster.KadastraalOnroerendeZaakEvent>(config)
                .SetValueDeserializer(new AvroDeserializer<Kadaster.KadastraalOnroerendeZaakEvent>(schemaRegistry).AsSyncOverAsync())
                .Build())
            {
                var retval = consumer.Consume(topic, partition, offset, false, mapper);
                consumer.Close();
                return retval;
            }
        }

        private ConsumerConfig GetConsumerConfig(string groupId = "")
        {
            return new ConsumerConfig
            {
                GroupId = !string.IsNullOrWhiteSpace(groupId)
                    ? groupId
                    : Guid.NewGuid().ToString(),
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                EnablePartitionEof = true,
                AutoOffsetReset = !string.IsNullOrWhiteSpace(groupId)
                    ? AutoOffsetReset.Earliest
                    : AutoOffsetReset.Latest,
                EnableAutoCommit = !string.IsNullOrWhiteSpace(groupId)
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
