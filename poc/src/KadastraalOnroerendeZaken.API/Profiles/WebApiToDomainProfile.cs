using AutoMapper;
using KadastraalOnroerendeZaken.API.Domain.Extensions;

namespace KadastraalOnroerendeZaken.API.Profiles
{
    public class WebApiToDomainProfile : Profile
    {
        public WebApiToDomainProfile()
        {
            CreateMap<Contracts.KadastraalOnroerendeZaakMetZakelijkGerechtigdenNieuw,
                      Domain.Entities.KadastraalOnroerendeZaak>();

            CreateMap<Contracts.KadastraalOnroerendeZaakNieuw,
                      Domain.Entities.KadastraalOnroerendeZaak>();

            CreateMap<Contracts.ZakelijkGerechtigdeNieuw,
                      Domain.Entities.ZakelijkGerechtigde>();

            CreateMap<Contracts.TypeKadastraleAanduiding,
                      Domain.ValueObjects.KadastraleAanduiding>()
                .ReverseMap();

            CreateMap<Contracts.TypeKadastraalOnroerendeZaak_enum,
                      Domain.Enums.KadastraalOnroerendeZaakType>()
                .ReverseMap();

            CreateMap<Contracts.TypeKoopsom,
                      Domain.ValueObjects.Koopsom>()
                .ForCtorParam("bedrag",
                              opt => opt.MapFrom(src => src.Koopsom));

            CreateMap<Contracts.TypeGerechtigde_enum,
                      Domain.Enums.ZakelijkGerechtigdeType>()
                .ReverseMap();

            CreateMap<Domain.Entities.KadastraalOnroerendeZaak,
                      Contracts.KadastraalOnroerendeZaak>()
                .ForMember(dest => dest.Identificatie,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.KadastraleAanduiding,
                           opt => opt.MapFrom(src => src.KadastraleAanduiding.ToStringEx()));

            CreateMap<Domain.Entities.ZakelijkGerechtigde,
                      Contracts.ZakelijkGerechtigde>()
                .ForMember(dest => dest.Identificatie,
                           opt => opt.MapFrom(src => src.Id));

            CreateMap<Domain.ValueObjects.Koopsom,
                      Contracts.TypeKoopsom>()
                .ForMember(dest => dest.Koopsom,
                           opt => opt.MapFrom(src => src.Bedrag));
        }
    }
}
