using KadastraalOnroerendeZakenEvents.API.Context;
using KadastraalOnroerendeZakenEvents.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KadastraalOnroerendeZakenEvents.API.Repositories
{
    public class KozTpoRepository : IKozTpoRepository
    {
        private readonly ApplicationDbContext context;

        public KozTpoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Add(KozTpo kozTpo)
        {
            context.Add(kozTpo);

            return (await context.SaveChangesAsync() > 0);
        }

        public async Task<List<KozTpo>> GetAsync(long id)
        {
            return await context.KozTpos.AsNoTracking()
                                        .Where(x => x.KozId == id)
                                        .ToListAsync();
        }
    }
}
