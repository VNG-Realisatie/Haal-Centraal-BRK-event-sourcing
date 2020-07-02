using KadastraalOnroerendeZaken.API.Contexts;
using KadastraalOnroerendeZaken.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Repositories
{
    public class KadastraalOnroerendeZakenRepository : IKadastraalOnroerendeZakenRepository
    {
        private readonly KadastraalOnroerendeZakenDbContext dbContext;

        public KadastraalOnroerendeZakenRepository(KadastraalOnroerendeZakenDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(KadastraalOnroerendeZaak entity)
        {
            dbContext.Add(entity);
            dbContext.SaveChanges();
        }

        public async Task<KadastraalOnroerendeZaak> GetAsync(long identificatie)
        {
            return await dbContext.KadastraalOnroerendeZaken
                                  .Include(k => k.Koopsom)
                                  .Include(k => k.KadastraleAanduiding)
                                  .Include(k => k.ZakelijkGerechtigden)
                                  .SingleOrDefaultAsync(x => x.Id == identificatie);
        }

        public async Task<IEnumerable<KadastraalOnroerendeZaak>> GetAllAsync()
        {
            return await dbContext.KadastraalOnroerendeZaken
                                  .Include(k => k.Koopsom)
                                  .Include(k => k.KadastraleAanduiding)
                                  .Include(k => k.ZakelijkGerechtigden)
                                  .ToListAsync();
        }

        public void Update(long id, KadastraalOnroerendeZaak kadastraalOnroerendeZaak)
        {
            if(dbContext.KadastraalOnroerendeZaken.AsNoTracking().SingleOrDefault(x => x.Id == id) == null)
            {
                throw new System.ArgumentException(nameof(kadastraalOnroerendeZaak));
            }
            kadastraalOnroerendeZaak.Id = id;

            dbContext.Entry(kadastraalOnroerendeZaak).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync() > 0);
        }
    }
}
