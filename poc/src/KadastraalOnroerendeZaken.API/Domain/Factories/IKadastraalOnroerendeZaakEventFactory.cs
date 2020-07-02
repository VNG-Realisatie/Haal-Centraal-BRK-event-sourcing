using KadastraalOnroerendeZaken.API.Domain.Entities;
using KadastraalOnroerendeZaken.API.Domain.Events;

namespace KadastraalOnroerendeZaken.API.Domain.Factories
{
    public interface IKadastraalOnroerendeZaakEventFactory
    {
        KadastraalOnroerendeZaakEvent CreateFrom(KadastraalOnroerendeZaak koz, LaatsteEvent laatsteEvent);
        KadastraalOnroerendeZaakEvent CreateFrom(KadastraalOnroerendeZaak koz);
    }
}