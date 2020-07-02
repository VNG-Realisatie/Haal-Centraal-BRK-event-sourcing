namespace KadastraalOnroerendeZaken.API.Domain.Events
{
    public class KadastraalOnroerendeZaakEvent
    {
        public bool IndicatieStukCompleet { get; set; }
        public string VorigEventIdentificatie { get; set; }
        public KadastraalOnroerendeZaakDto NieuweToestand { get; set; }
    }
}
