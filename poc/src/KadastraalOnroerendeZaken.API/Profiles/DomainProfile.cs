using AutoMapper;

namespace KadastraalOnroerendeZaken.API.Profiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Domain.Entities.KadastraalOnroerendeZaak,
                      Domain.Events.KadastraalOnroerendeZaakDto>();

            CreateMap<Domain.Entities.ZakelijkGerechtigde,
                      Domain.Events.ZakelijkGerechtigdeDto>();

            CreateMap<Domain.ValueObjects.KadastraleAanduiding,
                      Domain.Events.KadastraleAanduidingDto>();

            CreateMap<Domain.ValueObjects.Koopsom,
                      Domain.Events.KoopsomDto>();
        }
    }
}
