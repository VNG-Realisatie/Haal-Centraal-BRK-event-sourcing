using AutoMapper;
using KadastraalOnroerendeZaken.API.Contracts;
using KadastraalOnroerendeZaken.API.Domain.Factories;
using KadastraalOnroerendeZaken.API.Repositories;
using KadastraalOnroerendeZaken.API.Services.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Controllers
{
    [ApiController]
    public class ZakelijkGerechtigdenController : ZakelijkGerechtigdenControllerBase
    {
        private readonly IMapper mapper;
        private readonly IKadastraalOnroerendeZaakEventProducer producer;
        private readonly IKadastraalOnroerendeZakenRepository repository;
        private readonly ILaatsteEventsRepository laatsteEventsRepository;
        private readonly IKadastraalOnroerendeZaakEventFactory eventFactory;
        private readonly ILogger<ZakelijkGerechtigdenController> logger;

        public ZakelijkGerechtigdenController(IMapper mapper,
                                              IKadastraalOnroerendeZaakEventProducer producer,
                                              IKadastraalOnroerendeZakenRepository repository,
                                              ILaatsteEventsRepository laatsteEventsRepository,
                                              IKadastraalOnroerendeZaakEventFactory eventFactory,
                                              ILogger<ZakelijkGerechtigdenController> logger)
        {
            this.mapper = mapper;
            this.producer = producer;
            this.repository = repository;
            this.laatsteEventsRepository = laatsteEventsRepository;
            this.eventFactory = eventFactory;
            this.logger = logger;
        }

        public override async Task<ActionResult<ZakelijkGerechtigde>> CreerZakelijkGerechtigde(string kadastraalOnroerendeZaakIdentificatie, [FromBody] ZakelijkGerechtigdeNieuw body)
        {
            var zakelijkGerechtigde = mapper.Map<Domain.Entities.ZakelijkGerechtigde>(body);
            var id = long.Parse(kadastraalOnroerendeZaakIdentificatie);

            var koz = await repository.GetAsync(id);
            if(koz == null)
            {
                return NotFound($"Onbekend kadastraalOnroerendeZaakIdentificatie: {kadastraalOnroerendeZaakIdentificatie}");
            }

            koz.CreerZakelijkGerechtigde(zakelijkGerechtigde);
            await repository.SaveChangesAsync();

            var laatsteEvent = await laatsteEventsRepository.GetAsync(id);

            var evt = eventFactory.CreateFrom(koz, laatsteEvent);

            var laatsteEventId = await producer.ProduceAsync("kadastraalonroerendezaken", evt);
            if (!string.IsNullOrWhiteSpace(laatsteEventId))
            {
                laatsteEvent.EventIdentificatie = laatsteEventId;
                await repository.SaveChangesAsync();
            }

            return Ok(mapper.Map<ZakelijkGerechtigde>(zakelijkGerechtigde));
        }

        public override async Task<IActionResult> VervangZakelijkGerechtigde(string kadastraalOnroerendeZaakIdentificatie, string zakelijkGerechtigdeIdentificatie, [FromBody] ZakelijkGerechtigdeNieuw body)
        {
            var input = mapper.Map<Domain.Entities.ZakelijkGerechtigde>(body);
            var id = long.Parse(kadastraalOnroerendeZaakIdentificatie);

            var koz = await repository.GetAsync(id);
            if (koz == null)
            {
                return NotFound($"Onbekend kadastraalOnroerendeZaakIdentificatie: {kadastraalOnroerendeZaakIdentificatie}");
            }

            var zgId = long.Parse(zakelijkGerechtigdeIdentificatie);

            var zakelijkGerechtigde = koz.ZakelijkGerechtigden.SingleOrDefault(z => z.Id == zgId);
            if(zakelijkGerechtigde == null)
            {
                return NotFound($"Onbekend zakelijkGerechtigdeIdentificatie: {zakelijkGerechtigdeIdentificatie}");
            }

            zakelijkGerechtigde.Update(input);
            await repository.SaveChangesAsync();

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
    }
}
