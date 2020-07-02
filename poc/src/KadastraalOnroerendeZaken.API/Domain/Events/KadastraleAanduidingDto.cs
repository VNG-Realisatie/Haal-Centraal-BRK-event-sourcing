namespace KadastraalOnroerendeZaken.API.Domain.Events
{
    public class KadastraleAanduidingDto
    {
        public int? AppartementsrechtVolgnummer { get; set; }
        public int KadastraleGemeenteCode { get; set; }
        public int Perceelnummer { get; set; }
        public string Sectie { get; set; }
    }
}
