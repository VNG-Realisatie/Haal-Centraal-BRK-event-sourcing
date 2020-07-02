namespace KadastraalOnroerendeZaken.API.Domain.ValueObjects
{
    public class Koopsom
    {
        public long Bedrag { get; }
        public int Koopjaar { get; }
        public bool IndicatieMetMeerObjectenVerkregen { get; }

        protected Koopsom()
        {
        }

        public Koopsom(long bedrag,
                       int koopjaar,
                       bool indicatieMetMeerObjectenVerkregen)
            : this()
        {
            Bedrag = bedrag;
            Koopjaar = koopjaar;
            IndicatieMetMeerObjectenVerkregen = indicatieMetMeerObjectenVerkregen;
        }
    }
}
