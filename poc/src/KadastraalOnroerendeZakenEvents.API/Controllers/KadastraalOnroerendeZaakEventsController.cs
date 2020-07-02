using KadastraalOnroerendeZakenEvents.API.Contracts;
using KadastraalOnroerendeZakenEvents.API.Repositories;
using KadastraalOnroerendeZakenEvents.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public override async Task<ActionResult<KadastraalOnroerendeZaakEvent>> RaadpleegKadastraalOnroerendeZaakEvent(string identificatie, [FromQuery] bool? inclusiefVorigToestand = false)
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

        public override async Task<ActionResult<ICollection<KadastraalOnroerendeZaakEvent>>> ZoekKadastraalOnroerendeZaakEvents([FromQuery] string kadastraalonroerendezaakidentificatie, [FromQuery] DateTimeOffset? van, [FromQuery] bool? inclusiefVorigToestand = false)
        {
            var kozTpos = await repository.GetAsync(long.Parse(kadastraalonroerendezaakidentificatie));
            if (!kozTpos.Any())
            {
                return NotFound($"Onbekend kadastraalOnroerendeZaakIdentificatie: {kadastraalonroerendezaakidentificatie}");
            }
            var retval = van.HasValue
                ? from kozTpo in kozTpos
                         where kozTpo.CreationTime >= van.Value
                         select consumer.Consume(kozTpo.Topic,
                                                 kozTpo.Partition,
                                                 kozTpo.Offset,
                                                 inclusiefVorigToestand.Value)
                : from kozTpo in kozTpos
                  select consumer.Consume(kozTpo.Topic,
                                          kozTpo.Partition,
                                          kozTpo.Offset,
                                          inclusiefVorigToestand.Value);

            return Ok(retval);
        }
    }
}
