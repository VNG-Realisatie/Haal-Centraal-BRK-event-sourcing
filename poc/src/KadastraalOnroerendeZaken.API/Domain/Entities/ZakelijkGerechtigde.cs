using KadastraalOnroerendeZaken.API.Domain.Enums;
using System;

namespace KadastraalOnroerendeZaken.API.Domain.Entities
{
    public class ZakelijkGerechtigde : Entity
    {
        public ZakelijkGerechtigdeType Type { get; private set; }
        public DateTimeOffset Aanvangsdatum { get; private set; }

        protected ZakelijkGerechtigde()
        {
        }

        public ZakelijkGerechtigde(ZakelijkGerechtigdeType type,
                                   DateTimeOffset aanvangsdatum)
            : this()
        {
            Type = type;
            Aanvangsdatum = aanvangsdatum;
        }

        public void Update(ZakelijkGerechtigde input)
        {
            Type = input.Type;
            Aanvangsdatum = input.Aanvangsdatum;
        }
    }
}
