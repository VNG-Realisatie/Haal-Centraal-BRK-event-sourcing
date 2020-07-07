using KadastraalOnroerendeZakenEvents.API.Contracts;
using KadastraalOnroerendeZakenEvents.API.Repositories;
using KadastraalOnroerendeZakenEvents.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZakenEvents.API.Controllers
{
    [ApiController]
    public class KadastraalOnroerendeZaakEventsController : KadastraalOnroerendeZaakEventsControllerBase
    {
        private readonly IKadastraalOnroerendeZaakEventConsumer consumer;
        private readonly IKozTpoRepository repository;

        public KadastraalOnroerendeZaakEventsController(IKadastraalOnroerendeZaakEventConsumer consumer,
                                                        IKozTpoRepository repository)
        {
            this.consumer = consumer;
            this.repository = repository;
        }

        public override async Task<ActionResult<KadastraalOnroerendeZaakEvent>> RaadpleegKadastraalOnroerendeZaakEvent(string identificatie,
                                                                                                                       [FromQuery] bool? inclusiefVorigToestand = false)
        {
            try
            {
                var retval = consumer.Consume(identificatie, inclusiefVorigToestand.Value);

                return Ok(retval);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        public override async Task<ActionResult<KadastraalOnroerendeZaakEvents>> ZoekKadastraalOnroerendeZaakEvents([FromQuery] string kadastraalOnroerendeZaakIdentificatie,
                                                                                                                    [FromQuery] DateTimeOffset? vanafTijdstip,
                                                                                                                    [FromQuery] string vanafEventIdentificatie,
                                                                                                                    [FromQuery] int? maxAantalEvents = 1,
                                                                                                                    [FromQuery] bool? inclusiefVorigToestand = false)
        {
            var kozTpos = await repository.GetAsync(long.Parse(kadastraalOnroerendeZaakIdentificatie));
            if (!kozTpos.Any())
            {
                return NotFound($"Onbekend kadastraalOnroerendeZaakIdentificatie: {kadastraalOnroerendeZaakIdentificatie}");
            }

            var retval = new KadastraalOnroerendeZaakEvents
            {
                Events = new System.Collections.Generic.List<KadastraalOnroerendeZaakEvent>()
            };

            var index = 0;

            if (!string.IsNullOrWhiteSpace(vanafEventIdentificatie))
            {
                var (topic, partition, offset) = vanafEventIdentificatie.ParseIdentificatie();

                index = kozTpos.FindIndex(x => x.Topic == topic && x.Partition == partition && x.Offset == offset);
                if(index == -1)
                {
                    return NotFound($"Onbekend vanafEventIdentificatie: {vanafEventIdentificatie}");
                }
            }
            else if(vanafTijdstip.HasValue)
            {
                var kozTpo = kozTpos.FirstOrDefault(x => x.CreationTime >= vanafTijdstip.Value);
                if(kozTpo == null)
                {
                    return NotFound($"Geen events voor kadastraal onroerende zaak {kadastraalOnroerendeZaakIdentificatie} vanaf {vanafTijdstip.Value}");
                }
                index = kozTpos.IndexOf(kozTpo);
            }

            for (int i = index; i < kozTpos.Count; i++)
            {
                var kozTpo = kozTpos[i];
                if (retval.Events.Count >= maxAantalEvents)
                {
                    retval.VolgendEventIdentificatie = $"{kozTpo.Topic}-{kozTpo.Partition}-{kozTpo.Offset}";
                    break;
                }

                retval.Events.Add(consumer.Consume(kozTpo.Topic,
                                                   kozTpo.Partition,
                                                   kozTpo.Offset,
                                                   inclusiefVorigToestand.Value));
            }

            if (!string.IsNullOrWhiteSpace(retval.VolgendEventIdentificatie))
            {
                retval.VolgendeEventsLink = $"/kadastraalonroerendezaakevents?kadastraalonroerendezaakidentificatie={kadastraalOnroerendeZaakIdentificatie}&vanafEventIdentificatie={retval.VolgendEventIdentificatie}";
                if (inclusiefVorigToestand.HasValue)
                {
                    retval.VolgendeEventsLink += "&inclusiefVorigToestand=true";
                }
            }

            return Ok(retval);
        }
    }
}
