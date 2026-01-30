using Microsoft.AspNetCore.SignalR;

namespace GestionDeTurnos.Web.Hubs
{
    public class TurnosHub : Hub
    {
        // Este método puede ser llamado por el servidor para notificar a los clientes.
        public async Task ActualizarTurnos()
        {
            await Clients.All.SendAsync("ActualizarTurnos");
        }
    }
}