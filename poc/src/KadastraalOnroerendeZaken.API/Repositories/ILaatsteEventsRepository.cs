using KadastraalOnroerendeZaken.API.Domain;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Repositories
{
    public interface ILaatsteEventsRepository
    {
        void Add(LaatsteEvent laatsteEvent);
        Task<LaatsteEvent> GetAsync(long identificatie);
        void Update(LaatsteEvent laatsteEvent);
    }
}
