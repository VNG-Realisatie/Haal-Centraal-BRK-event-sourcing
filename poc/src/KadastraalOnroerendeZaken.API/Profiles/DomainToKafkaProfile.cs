using AutoMapper;
using System.Collections;
using System.Linq;

namespace KadastraalOnroerendeZaken.API.Profiles
{
    public class DomainToKafkaProfile : Profile
    {
        public DomainToKafkaProfile()
        {
            CreateMap<Domain.Events.KadastraalOnroerendeZaakEvent,
                      Kadaster.KadastraalOnroerendeZaakEvent>();

            CreateMap<Domain.Events.KadastraalOnroerendeZaakDto,
                      Kadaster.KadastraalOnroerendeZaak>()
                .ForMember(dest => dest.ZakelijkGerechtigden,
                           opt => opt.MapFrom(src => (IList)src.ZakelijkGerechtigden.ToList()));

            CreateMap<Domain.Events.KadastraleAanduidingDto,
                      Kadaster.KadastraleAanduiding>();

            CreateMap<Domain.Events.KoopsomDto,
                      Kadaster.Koopsom>();

            CreateMap<Domain.Events.ZakelijkGerechtigdeDto,
                      Kadaster.ZakelijkGerechtigde>()
                .ForMember(dest => dest.Aanvangsdatum,
                           opt => opt.MapFrom(src => src.Aanvangsdatum.UtcDateTime.ToBinary()))
                ;
        }
    }
}
