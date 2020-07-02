namespace KadastraalOnroerendeZaken.API.Domain.Events
{
    public class KoopsomDto
    {
        public long Bedrag { get; set; }
        public int Koopjaar { get; set; }
        public bool IndicatieMetMeerObjectenVerkregen { get; set; }
    }
}
