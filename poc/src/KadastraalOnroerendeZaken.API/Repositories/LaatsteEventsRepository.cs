using KadastraalOnroerendeZaken.API.Contexts;
using KadastraalOnroerendeZaken.API.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZaken.API.Repositories
{
    public class LaatsteEventsRepository : ILaatsteEventsRepository
    {
        private readonly KadastraalOnroerendeZakenDbContext dbContext;

        public LaatsteEventsRepository(KadastraalOnroerendeZakenDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(LaatsteEvent laatsteEvent)
        {
            dbContext.Add(laatsteEvent);
            dbContext.SaveChanges();
        }

        public async Task<LaatsteEvent> GetAsync(long identificatie)
        {
            return await dbContext.LaatsteEvents.FindAsync(identificatie);
        }

        public void Update(LaatsteEvent laatsteEvent)
        {
            dbContext.Entry(laatsteEvent).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}
