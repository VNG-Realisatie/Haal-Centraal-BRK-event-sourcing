using KadastraalOnroerendeZakenEvents.API.Contracts;
using KadastraalOnroerendeZakenEvents.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public override async Task<ActionResult<KadastraalOnroerendeZaakEvents>> ZoekKadastraalOnroerendeZaakEventsInAbonnement([FromHeader] string abonnementIdentificatie,
                                                                                                                                [FromQuery] DateTimeOffset? vanafTijdstip,
                                                                                                                                [FromQuery] string vanafEventIdentificatie,
                                                                                                                                [FromQuery] int? maxAantalEvents = 1,
                                                                                                                                [FromQuery] bool? inclusiefVorigToestand = false)
        {
            var topics = new[] { "kadastraalonroerendezaken" };

            var retval = consumer.Consume(abonnementIdentificatie,
                                   topics,
                                   vanafTijdstip,
                                   vanafEventIdentificatie,
                                   maxAantalEvents.Value,
                                   inclusiefVorigToestand.Value);

            if (!string.IsNullOrWhiteSpace(retval.VolgendEventIdentificatie))
            {
                retval.VolgendeEventsLink = $"/abonnement/kadastraalonroerendezaakevents?vanafEventidentificatie={retval.VolgendEventIdentificatie}";
                if (inclusiefVorigToestand.Value)
                {
                    retval.VolgendeEventsLink += "&inclusiefVorigToestand=true";
                }
            }

            return Ok(retval);
        }
    }
}
