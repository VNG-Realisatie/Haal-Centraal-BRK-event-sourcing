using KadastraalOnroerendeZaken.API.Domain.Enums;
using System.Collections.Generic;

namespace KadastraalOnroerendeZaken.API.Domain.Events
{
    public class KadastraalOnroerendeZaakDto
    {
        public long Id { get; set; }
        public KadastraalOnroerendeZaakType Type { get; set; }
        public KadastraleAanduidingDto KadastraleAanduiding { get; set; }
        public KoopsomDto Koopsom { get; set; }
        public ICollection<ZakelijkGerechtigdeDto> ZakelijkGerechtigden { get; set; }
    }
}
