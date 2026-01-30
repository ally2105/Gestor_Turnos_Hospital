using GestionDeTurnos.Web.Data;
using GestionDeTurnos.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeTurnos.Web.Services
{
    public class TurnService(AppDbContext context)
    {
        // Crear un nuevo turno
        public async Task<Turn> CrearTurnoAsync()
        {
            int siguienteNumero = await context.Turns.AnyAsync()
                ? await context.Turns.MaxAsync(t => t.Number) + 1
                : 1;

            var turno = new Turn
            {
                Number = siguienteNumero,
                Status = "Pendiente", // Estado inicial
                CreatedAt = DateTime.UtcNow
            };

            context.Turns.Add(turno);
            await context.SaveChangesAsync();
            return turno;
        }

        // Obtener el siguiente turno pendiente de ser llamado
        private async Task<Turn?> ObtenerSiguientePendienteAsync()
        {
            return await context.Turns
                .Where(t => t.Status == "Pendiente")
                .OrderBy(t => t.Number)
                .FirstOrDefaultAsync();
        }

        // Llamar al siguiente turno
        public async Task<Turn?> LlamarSiguienteAsync(int boxId)
        {
            var turno = await ObtenerSiguientePendienteAsync();
            if (turno == null) return null; // No hay turnos pendientes

            turno.Status = "Llamado";
            turno.CalledAt = DateTime.UtcNow;
            turno.BoxId = boxId;

            await context.SaveChangesAsync();
            return turno;
        }

        // Verificar si existe una caja
        public async Task<bool> ExisteCajaAsync(int boxId)
        {
            return await context.Boxes.AnyAsync(b => b.Id == boxId);
        }

        // Obtener los últimos 10 turnos para mostrar en el panel y la TV
        public async Task<List<Turn>> ObtenerUltimosAsync()
        {
            return await context.Turns
                .OrderByDescending(t => t.CreatedAt)
                .Take(10)
                .ToListAsync();
        }

        // Reiniciar la numeración (borra todos los turnos)
        public async Task ReiniciarTurnosAsync()
        {
            context.Turns.RemoveRange(context.Turns);
            await context.SaveChangesAsync();
        }
    }
}
