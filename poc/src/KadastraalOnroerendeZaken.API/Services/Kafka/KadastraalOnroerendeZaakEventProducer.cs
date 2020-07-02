using AutoMapper;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KadastraalOnroerendeZaken.API.Domain.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Services.Kafka
{
    public class KadastraalOnroerendeZaakEventProducer : IKadastraalOnroerendeZaakEventProducer
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<KadastraalOnroerendeZaakEventProducer> logger;

        public KadastraalOnroerendeZaakEventProducer(IConfiguration configuration,
                                                     IMapper mapper,
                                                     ILogger<KadastraalOnroerendeZaakEventProducer> logger)
        {
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<string> ProduceAsync(string topic, KadastraalOnroerendeZaakEvent value)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = configuration["Kafka:SchemaRegistryServers"]
            };

            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producer = new ProducerBuilder<string, Kadaster.KadastraalOnroerendeZaakEvent>(config)
                .SetValueSerializer(new AvroSerializer<Kadaster.KadastraalOnroerendeZaakEvent>(schemaRegistry))
                .Build();
            try
            {
                var message = new Message<string, Kadaster.KadastraalOnroerendeZaakEvent>
                {
                    Key = value.NieuweToestand.Id.ToString(),
                    Value = mapper.Map<Kadaster.KadastraalOnroerendeZaakEvent>(value)
                };
                var deliveryResult = await producer.ProduceAsync(topic, message);

                logger.LogInformation($"Event published to '{deliveryResult.TopicPartitionOffset}'");

                return $"{deliveryResult.Topic}-{deliveryResult.Partition.Value}-{deliveryResult.Offset.Value}";
            }
            catch (ProduceException<Null, string> e)
            {
                logger.LogError(e, "Failed to publish event");

                return string.Empty;
            }
            finally
            {
                producer.Flush(TimeSpan.FromSeconds(30));
            }
        }
    }
}
