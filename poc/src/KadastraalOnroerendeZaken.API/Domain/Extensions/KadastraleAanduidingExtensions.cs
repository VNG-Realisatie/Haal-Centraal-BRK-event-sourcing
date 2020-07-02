using KadastraalOnroerendeZaken.API.Domain.ValueObjects;

namespace KadastraalOnroerendeZaken.API.Domain.Extensions
{
    public static class KadastraleAanduidingExtensions
    {
        public static string ToStringEx(this KadastraleAanduiding aanduiding)
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
