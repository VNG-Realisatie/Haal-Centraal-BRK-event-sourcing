using KadastraalOnroerendeZakenEvents.API.Contracts;
using System;
using System.Collections.Generic;

namespace KadastraalOnroerendeZakenEvents.API.Services.Kafka
{
    public interface IKadastraalOnroerendeZaakEventConsumer
    {
        IEnumerable<KadastraalOnroerendeZaakEvent> Consume(string consumerId, IEnumerable<string> topics, DateTimeOffset? van, bool includeVorigToestand);
        KadastraalOnroerendeZaakEvent Consume(string identificatie, bool includeVorigToestand);
        KadastraalOnroerendeZaakEvent Consume(string topic, int partition, long offset, bool includeVorigToestand);
    }
}