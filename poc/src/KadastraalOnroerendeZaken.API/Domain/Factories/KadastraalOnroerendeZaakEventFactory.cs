using AutoMapper;
using KadastraalOnroerendeZaken.API.Domain.Entities;
using KadastraalOnroerendeZaken.API.Domain.Events;

namespace KadastraalOnroerendeZaken.API.Domain.Factories
{
    public class KadastraalOnroerendeZaakEventFactory : IKadastraalOnroerendeZaakEventFactory
    {
        private readonly IMapper mapper;

        public KadastraalOnroerendeZaakEventFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public KadastraalOnroerendeZaakEvent CreateFrom(KadastraalOnroerendeZaak koz)
        {
            return CreateFrom(koz, null);
        }

        public KadastraalOnroerendeZaakEvent CreateFrom(KadastraalOnroerendeZaak koz, LaatsteEvent laatsteEvent)
        {
            return new KadastraalOnroerendeZaakEvent
            {
                VorigEventIdentificatie = laatsteEvent?.EventIdentificatie,
                NieuweToestand = mapper.Map<KadastraalOnroerendeZaakDto>(koz)
            };
        }
    }
}
