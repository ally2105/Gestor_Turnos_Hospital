using GestionDeTurnos.Web.Data;
using GestionDeTurnos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeTurnos.Web.Services
{
    public class BoxService
    {
        private readonly AppDbContext _context;

        public BoxService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Box>> GetAllAsync()
        {
            return await _context.Boxes.ToListAsync();
        }

        public async Task AddAsync(Box box)
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleStateAsync(int id)
        {
            var box = await _context.Boxes.FindAsync(id);
            if (box != null)
            {
                box.IsOpen = !box.IsOpen;
                await _context.SaveChangesAsync();
            }
        }
    }
}