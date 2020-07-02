using KadastraalOnroerendeZakenEvents.API.Contracts;
using KadastraalOnroerendeZakenEvents.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZakenEvents.API.Controllers
{
    [ApiController]
    public class AbonnementenController : AbonnementenControllerBase
    {
        private readonly IKadastraalOnroerendeZaakEventConsumer consumer;

        public AbonnementenController(IKadastraalOnroerendeZaakEventConsumer consumer)
        {
            this.consumer = consumer;
        }

        public override async Task<ActionResult<ICollection<KadastraalOnroerendeZaakEvent>>> ZoekKadastraalOnroerendeZaakEventsInAbonnement([FromHeader] string abonnementIdentificatie, [FromQuery] DateTimeOffset? van, [FromQuery] bool? inclusiefVorigToestand = false)
        {
            return await Task.Run(() =>
                Ok(consumer.Consume(abonnementIdentificatie,
                                    new[] { "kadastraalonroerendezaken" },
                                    van,
                                    inclusiefVorigToestand.Value)));
        }
    }
}
