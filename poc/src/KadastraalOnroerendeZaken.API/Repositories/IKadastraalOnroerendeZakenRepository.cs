using KadastraalOnroerendeZaken.API.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Repositories
{
    public interface IKadastraalOnroerendeZakenRepository
    {
        void Add(KadastraalOnroerendeZaak entity);
        Task<IEnumerable<KadastraalOnroerendeZaak>> GetAllAsync();
        Task<KadastraalOnroerendeZaak> GetAsync(long identificatie);
        Task<bool> SaveChangesAsync();
        void Update(long id, KadastraalOnroerendeZaak kadastraalOnroerendeZaak);
    }
}