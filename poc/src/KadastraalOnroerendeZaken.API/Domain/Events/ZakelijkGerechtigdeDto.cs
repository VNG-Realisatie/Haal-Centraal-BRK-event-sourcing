using KadastraalOnroerendeZaken.API.Domain.Enums;
using System;

namespace KadastraalOnroerendeZaken.API.Domain.Events
{
    public class ZakelijkGerechtigdeDto
    {
        public long Id { get; set; }
        public ZakelijkGerechtigdeType Type { get; set; }
        public DateTimeOffset Aanvangsdatum { get; set; }
    }
}
