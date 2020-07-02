using AutoMapper;
using Confluent.Kafka;
using System;

namespace KadastraalOnroerendeZakenEvents.API.Profiles
{
    public class KafkaToWebApiProfile : Profile
    {
        public KafkaToWebApiProfile()
        {
            CreateMap<ConsumeResult<string, Kadaster.KadastraalOnroerendeZaakEvent>,
                      Contracts.KadastraalOnroerendeZaakEvent>()
                .ForMember(dest => dest.NieuweToestandKadastraalOnroerendeZaak,
                           opt => opt.MapFrom(src => src.Message.Value.NieuweToestand))
                .ForMember(dest => dest.VorigEventLink,
                           opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Message.Value.VorigEventIdentificatie)
                                ? $"/kadastraalonroerendezaakevents/{src.Message.Value.VorigEventIdentificatie}"
                                : null))
                .ForMember(dest => dest.Identificatie,
                           opt => opt.MapFrom(src => string.Format($"{src.Topic}-{src.TopicPartition.Partition.Value}-{src.Offset.Value}")))
                .ForMember(dest => dest.Tijdstip,
                           opt => opt.MapFrom(src => src.Message.Timestamp.UtcDateTime.ToLocalTime()));

            CreateMap<Kadaster.KadastraalOnroerendeZaakEvent,
                      Contracts.KadastraalOnroerendeZaakEvent>();

            CreateMap<Kadaster.KadastraalOnroerendeZaak,
                      Contracts.KadastraalOnroerendeZaakMutatie>()
                .ForMember(dest => dest.KadastraleAanduiding,
                           opt => opt.MapFrom(src => src.KadastraleAanduiding.ToStringEx()))
                .ForMember(dest => dest.Identificatie,
                           opt => opt.MapFrom(src => src.Id));

            CreateMap<Kadaster.Koopsom,
                      Contracts.TypeKoopsom>()
                .ForMember(dest => dest.Koopsom,
                           opt => opt.MapFrom(src => src.Bedrag));

            CreateMap<Kadaster.ZakelijkGerechtigde,
                      Contracts.ZakelijkGerechtigde>()
                .ForMember(dest => dest.Identificatie,
                           opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Aanvangsdatum,
                           opt => opt.MapFrom(src => DateTime.FromBinary(src.Aanvangsdatum).ToLocalTime()));
        }
    }

    public static class KadastraleAanduidingExtensions
    {
        public static string ToStringEx(this Kadaster.KadastraleAanduiding aanduiding)
        {
            string gemeente = aanduiding.KadastraleGemeenteCode switch
            {
                344 => "Utrecht",
                363 => "Amsterdam",
                518 => "Den Haag",
                599 => "Rotterdam",
                _ => "Onbekend",
            };

            return aanduiding.AppartementsrechtVolgnummer.HasValue
                ? $"{gemeente} {aanduiding.Sectie} {aanduiding.Perceelnummer} A{aanduiding.AppartementsrechtVolgnummer}"
                : $"{gemeente} {aanduiding.Sectie} {aanduiding.Perceelnummer}";
        }
    }
}
