using AutoMapper;
using KadastraalOnroerendeZaken.API.Contracts;
using KadastraalOnroerendeZaken.API.Domain.Factories;
using KadastraalOnroerendeZaken.API.Repositories;
using KadastraalOnroerendeZaken.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Controllers
{
    [ApiController]
    public class KadastraalOnroerendeZakenController : KadastraalOnroerendeZakenControllerBase
    {
        private readonly IMapper mapper;
        private readonly IKadastraalOnroerendeZaakEventProducer producer;
        private readonly IKadastraalOnroerendeZakenRepository repository;
        private readonly ILaatsteEventsRepository laatsteEventsRepository;
        private readonly IKadastraalOnroerendeZaakEventFactory eventFactory;
        private readonly ILogger<KadastraalOnroerendeZakenController> logger;

        public KadastraalOnroerendeZakenController(IMapper mapper,
                                                   IKadastraalOnroerendeZaakEventProducer producer,
                                                   IKadastraalOnroerendeZakenRepository repository,
                                                   ILaatsteEventsRepository laatsteEventsRepository,
                                                   IKadastraalOnroerendeZaakEventFactory eventFactory,
                                                   ILogger<KadastraalOnroerendeZakenController> logger)
        {
            this.mapper = mapper;
            this.producer = producer;
            this.repository = repository;
            this.laatsteEventsRepository = laatsteEventsRepository;
            this.eventFactory = eventFactory;
            this.logger = logger;
        }

        public override async Task<ActionResult<KadastraalOnroerendeZaak>> CreerKadastraalOnroerendeZaak([FromBody] KadastraalOnroerendeZaakMetZakelijkGerechtigdenNieuw body)
        {
            var entity = mapper.Map<Domain.Entities.KadastraalOnroerendeZaak>(body);

            repository.Add(entity);

            var evt = eventFactory.CreateFrom(entity);

            var laatsteEventId = await producer.ProduceAsync("kadastraalonroerendezaken", evt);
            if (!string.IsNullOrWhiteSpace(laatsteEventId))
            {
                laatsteEventsRepository.Add(new Domain.LaatsteEvent
                {
                    KadastraalOnroerendeZaakIdentificatie = entity.Id,
                    EventIdentificatie = laatsteEventId
                });
            }

            return Ok(mapper.Map<KadastraalOnroerendeZaak>(entity));
        }

        public override async Task<IActionResult> VervangKadastraalOnroerendeZaak(string kadastraalOnroerendeZaakIdentificatie, [FromBody] KadastraalOnroerendeZaakNieuw body)
        {
            logger.LogInformation($"{kadastraalOnroerendeZaakIdentificatie}");

            var data = mapper.Map<Domain.Entities.KadastraalOnroerendeZaak>(body);
            var id = long.Parse(kadastraalOnroerendeZaakIdentificatie);

            var koz = await repository.GetAsync(id);
            if (koz == null)
            {
                return NotFound($"Onbekend kadastraalOnroerendeZaakIdentificatie: {kadastraalOnroerendeZaakIdentificatie}");
            }

            koz.Update(data);

            var laatsteEvent = await laatsteEventsRepository.GetAsync(id);

            var evt = eventFactory.CreateFrom(koz, laatsteEvent);

            var laatsteEventId = await producer.ProduceAsync("kadastraalonroerendezaken", evt);
            if (!string.IsNullOrWhiteSpace(laatsteEventId))
            {
                laatsteEvent.EventIdentificatie = laatsteEventId;
                await repository.SaveChangesAsync();
            }

            return Ok();
        }

        public override async Task<ActionResult<ICollection<KadastraalOnroerendeZaak>>> ZoekKadastraalOnroerendeZaken()
        {
            var retval = await repository.GetAllAsync();

            return Ok(mapper.Map<Contracts.KadastraalOnroerendeZaken>(retval));
        }
    }
}
