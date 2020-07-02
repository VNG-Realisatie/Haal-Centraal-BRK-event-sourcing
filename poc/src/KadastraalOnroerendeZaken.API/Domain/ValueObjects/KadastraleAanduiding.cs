namespace KadastraalOnroerendeZaken.API.Domain.ValueObjects
{
    public class KadastraleAanduiding
    {
        public int? AppartementsrechtVolgnummer { get; }
        public int KadastraleGemeenteCode { get; }
        public int Perceelnummer { get; }
        public string Sectie { get; }

        protected KadastraleAanduiding()
        {
        }

        public KadastraleAanduiding(int kadastraleGemeenteCode,
                                    int perceelnummer,
                                    string sectie,
                                    int? appartementsrechtVolgnummer)
            : this()
        {
            KadastraleGemeenteCode = kadastraleGemeenteCode;
            Perceelnummer = perceelnummer;
            Sectie = sectie;
            AppartementsrechtVolgnummer = appartementsrechtVolgnummer;
        }
    }
}
