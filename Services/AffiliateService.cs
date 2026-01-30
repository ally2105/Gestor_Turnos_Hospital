using GestionDeTurnos.Web.Data;
using GestionDeTurnos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeTurnos.Web.Services
{
    public class AffiliateService(AppDbContext context)
    {
        public async Task<List<Affiliate>> GetAllAsync()
        {
            return await context.Affiliates.ToListAsync();
        }

        public async Task AddAsync(Affiliate affiliate)
        {
            context.Affiliates.Add(affiliate);
            await context.SaveChangesAsync();
        }

        public async Task<Affiliate?> GetByIdAsync(int id)
        {
            return await context.Affiliates.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var affiliate = await context.Affiliates.FindAsync(id);
            if (affiliate != null)
            {
                context.Affiliates.Remove(affiliate);
                await context.SaveChangesAsync();
            }
        }
    }
}