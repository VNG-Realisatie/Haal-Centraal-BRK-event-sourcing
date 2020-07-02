using KadastraalOnroerendeZakenEvents.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZakenEvents.API.Repositories
{
    public interface IKozTpoRepository
    {
        Task<bool> Add(KozTpo kozTpo);
        Task<IEnumerable<KozTpo>> GetAsync(long id);
    }
}
