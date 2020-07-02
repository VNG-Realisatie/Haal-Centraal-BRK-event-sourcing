using KadastraalOnroerendeZaken.API.Domain.Enums;
using KadastraalOnroerendeZaken.API.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KadastraalOnroerendeZaken.API.Domain.Entities
{
    public class KadastraalOnroerendeZaak : Entity
    {
        public KadastraleAanduiding KadastraleAanduiding { get; private set; }
        public KadastraalOnroerendeZaakType Type { get; private set; }
        public Koopsom Koopsom { get; private set; }
        public ICollection<ZakelijkGerechtigde> ZakelijkGerechtigden { get; }

        protected KadastraalOnroerendeZaak()
        {
        }

        public KadastraalOnroerendeZaak(KadastraleAanduiding kadastraleAanduiding,
                                KadastraalOnroerendeZaakType type,
                                Koopsom koopsom)
            : this(kadastraleAanduiding, type, koopsom, new List<ZakelijkGerechtigde>())
        {
        }

        public KadastraalOnroerendeZaak(KadastraleAanduiding kadastraleAanduiding,
                                        KadastraalOnroerendeZaakType type,
                                        Koopsom koopsom,
                                        IEnumerable<ZakelijkGerechtigde> zakelijkGerechtigden)
            : this()
        {
            KadastraleAanduiding = kadastraleAanduiding;
            Type = type;
            Koopsom = koopsom;
            ZakelijkGerechtigden = zakelijkGerechtigden.ToList();
        }

        public void Update(KadastraalOnroerendeZaak data)
        {
            KadastraleAanduiding = data.KadastraleAanduiding;
            Type = data.Type;
            Koopsom = data.Koopsom;
        }

        public void CreerZakelijkGerechtigde(ZakelijkGerechtigde zakelijkGerechtigde)
        {
            ZakelijkGerechtigden.Add(zakelijkGerechtigde);
        }
    }
}
