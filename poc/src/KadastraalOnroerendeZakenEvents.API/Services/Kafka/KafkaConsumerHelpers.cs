using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace KadastraalOnroerendeZakenEvents.API.Services.Kafka
{
    public static class KafkaConsumerHelpers
    {
        public static void SetSubscription(this IConsumer<string, Kadaster.KadastraalOnroerendeZaakEvent> consumer,
                                           IEnumerable<string> topics,
                                           DateTimeOffset? van,
                                           string vanafEventIdentificatie,
                                           ILogger logger)
        {
            if (van.HasValue)
            {
                var timesForTopics =
                    (from topic in topics
                     select new TopicPartitionTimestamp(topic,
                                                        0,
                                                        new Timestamp(van.Value.UtcDateTime))).ToList();
                var offsets = consumer.OffsetsForTimes(timesForTopics, TimeSpan.FromSeconds(5));

                consumer.Assign(offsets, logger);
            }
            else if (!string.IsNullOrWhiteSpace(vanafEventIdentificatie))
            {
                consumer.Assign(new[] { vanafEventIdentificatie.ParseIdentificatie().ToTopicPartitionOffset() }, logger);
            }
            else
            {
                consumer.Subscribe(topics);
            }
        }

        private static void Assign(this IConsumer<string, Kadaster.KadastraalOnroerendeZaakEvent> consumer,
                                   IEnumerable<TopicPartitionOffset> offsets,
                                   ILogger logger)
        {
            foreach (var offset in offsets)
            {
                try
                {
                    consumer.Assign(offset);
                }
                catch (KafkaException ex)
                {
                    logger.LogError(ex, $"Assign({offset}) throws an exception");
                }
            }
        }

        public static Contracts.KadastraalOnroerendeZaakEvent Consume(this IConsumer<string, Kadaster.KadastraalOnroerendeZaakEvent> consumer,
                                                                      string topic,
                                                                      int partition,
                                                                      long offset,
                                                                      bool includeVorigToestand,
                                                                      IMapper mapper)
        {
            Contracts.KadastraalOnroerendeZaakEvent retval = null;

            consumer.Assign(new TopicPartitionOffset(topic, partition, offset));

            var result = consumer.Consume(TimeSpan.FromSeconds(5));

            if (result != null && !result.IsPartitionEOF)
            {
                retval = mapper.Map<Contracts.KadastraalOnroerendeZaakEvent>(result);
                var vorigEventIdentificatie = result.Message.Value.VorigEventIdentificatie;
                if (includeVorigToestand &&
                    !string.IsNullOrWhiteSpace(vorigEventIdentificatie))
                {
                    var (t, p, o) = vorigEventIdentificatie.ParseIdentificatie();

                    retval.VorigToestandKadastraalOnroerendeZaak = consumer.Consume(t, p, o, false, mapper).NieuweToestandKadastraalOnroerendeZaak;
                }
            }

            return retval;
        }

        public static (string, int, long) ParseIdentificatie(this string identificatie)
        {
            if (string.IsNullOrWhiteSpace(identificatie))
            {
                throw new ArgumentException($"Ongeldig identificatie: {identificatie}", nameof(identificatie));
            }

            var match = Regex.Match(identificatie, @"^(?<topic>.*)-(?<partition>\d*)-(?<offset>\d*)$");
            if (!match.Success)
            {
                throw new ArgumentException($"Ongeldig identificatie: {identificatie}", nameof(identificatie));
            }

            return (match.Groups["topic"].Value,
                    int.Parse(match.Groups["partition"].Value),
                    long.Parse(match.Groups["offset"].Value));
        }

        private static TopicPartitionOffset ToTopicPartitionOffset(this ValueTuple<string, int, long> input)
        {
            return new TopicPartitionOffset(input.Item1, input.Item2, input.Item3);
        }
    }
}
