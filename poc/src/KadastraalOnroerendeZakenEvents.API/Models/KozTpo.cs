using System;

namespace KadastraalOnroerendeZakenEvents.API.Models
{
    public class KozTpo
    {
        public long Id { get; set; }
        public long KozId { get; set; }
        public string Topic { get; set; }
        public int Partition { get; set; }
        public long Offset { get; set; }
        public DateTime CreationTime { get; internal set; }
    }
}
