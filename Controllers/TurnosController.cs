using GestionDeTurnos.Web.Services;
using GestionDeTurnos.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GestionDeTurnos.Web.Controllers
{
    public class TurnosController : Controller
    {
        private readonly TurnService _turnService;
        private readonly IHubContext<TurnosHub> _hubContext;

        public TurnosController(TurnService turnService, IHubContext<TurnosHub> hubContext)
        {
            _turnService = turnService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var turnos = await _turnService.ObtenerUltimosAsync();
            return View(turnos);
        }

        public IActionResult Solicitar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SolicitarTurno()
        { 
            var nuevoTurno = await _turnService.CrearTurnoAsync();
            await _hubContext.Clients.All.SendAsync("ActualizarTurnos");
            return RedirectToAction("Confirmacion", new { numero = nuevoTurno.Number });
        }

        public IActionResult Confirmacion(int numero)
        {
            ViewBag.Numero = numero;
            return View();
        }

        public async Task<IActionResult> Siguiente()
        {
            // Verificar si existe al menos una caja
            var cajaExiste = await _turnService.ExisteCajaAsync(1);
            if (!cajaExiste)
            {
                TempData["MensajeError"] = "No hay cajas disponibles. Por favor, configure al menos una caja primero.";
                return RedirectToAction("Index");
            }

            var turno = await _turnService.LlamarSiguienteAsync(1);
            if (turno != null)
            {
                await _hubContext.Clients.All.SendAsync("ActualizarTurnos");
                TempData["MensajeExito"] = $"Turno {turno.Number} llamado a Caja 1.";
            }
            else
            {
                // Si no hay turnos, guardamos un mensaje para mostrarlo en la vista.
                TempData["MensajeError"] = "No hay turnos pendientes para llamar.";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Tv()
        {
            var turnos = await _turnService.ObtenerUltimosAsync();
            return View(turnos);
        }

        public async Task<IActionResult> Reiniciar()
        {
            await _turnService.ReiniciarTurnosAsync();
            await _hubContext.Clients.All.SendAsync("ActualizarTurnos");
            TempData["MensajeExito"] = "La numeración de turnos ha sido reiniciada.";
            return RedirectToAction("Index");
        }
    }
}
