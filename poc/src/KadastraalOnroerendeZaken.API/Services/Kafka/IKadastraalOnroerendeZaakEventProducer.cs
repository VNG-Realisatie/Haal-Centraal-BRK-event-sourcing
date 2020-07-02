using KadastraalOnroerendeZaken.API.Domain.Events;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Services.Kafka
{
    public interface IKadastraalOnroerendeZaakEventProducer
    {
        Task<string> ProduceAsync(string topic, KadastraalOnroerendeZaakEvent value);
    }
}