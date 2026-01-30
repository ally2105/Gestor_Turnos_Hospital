using GestionDeTurnos.Web.Data;
using GestionDeTurnos.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeTurnos.Web.Controllers
{
    public class AfiliadosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AfiliadosController> _logger; // Añadir logger

        public AfiliadosController(AppDbContext context, ILogger<AfiliadosController> logger) // Inyectar logger
        {
            _context = context;
            _logger = logger; // Asignar logger
        }

        // Lista de todos los afiliados
        public async Task<IActionResult> Index()
        {
            var afiliados = await _context.Affiliates.ToListAsync();
            return View(afiliados);
        }

        // Muestra el formulario para crear un nuevo afiliado
        public IActionResult Crear()
        {
            return View();
        }

        // Procesa el formulario de creación
        [HttpPost]
        public async Task<IActionResult> Crear(Affiliate afiliado, string? fotoData)
        {
            if (ModelState.IsValid)
            {
                // Si se capturó una foto, la guardamos
                if (!string.IsNullOrEmpty(fotoData))
                {
                    try
                    {
                        var fotoBytes = Convert.FromBase64String(fotoData.Split(',')[1]);
                        var fileName = $"{Guid.NewGuid()}.jpg";
                        var fotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos");
                        
                        // Asegurarse de que el directorio exista
                        if (!Directory.Exists(fotosPath))
                        {
                            Directory.CreateDirectory(fotosPath);
                        }

                        var filePath = Path.Combine(fotosPath, fileName);
                        await System.IO.File.WriteAllBytesAsync(filePath, fotoBytes);
                        afiliado.PhotoPath = "/fotos/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al guardar la foto del afiliado.");
                        ModelState.AddModelError("Photo", "Error al procesar la foto. Intente de nuevo.");
                        return View(afiliado);
                    }
                }

                try
                {
                    _context.Add(afiliado);
                    await _context.SaveChangesAsync();
                    TempData["MensajeExito"] = "Afiliado registrado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al guardar el afiliado en la base de datos.");
                    ModelState.AddModelError("", "Error al guardar el afiliado. Verifique los datos e intente de nuevo.");
                }
            }
            else
            {
                // Loggear errores de validación si ModelState.IsValid es false
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError("Error de validación: {ErrorMessage}", error.ErrorMessage);
                    }
                }
                ModelState.AddModelError("", "Por favor, corrija los errores de validación.");
            }
            return View(afiliado);
        }

        // Muestra los detalles de un afiliado (y su carnet)
        public async Task<IActionResult> Carnet(int id)
        {
            var afiliado = await _context.Affiliates.FindAsync(id);
            if (afiliado == null)
            {
                return NotFound();
            }
            return View(afiliado);
        }

        // GET: Afiliados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var affiliate = await _context.Affiliates.FindAsync(id);
            if (affiliate == null)
            {
                return NotFound();
            }
            return View(affiliate);
        }

        // POST: Afiliados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Affiliate affiliate, string? fotoData)
        {
            if (id != affiliate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mantener datos que no vienen del formulario si es necesario
                    // Para esto es mejor recuperar la entidad original y actualizar sus propiedades
                    var existingAffiliate = await _context.Affiliates.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
                    
                    if (existingAffiliate != null)
                    {
                        affiliate.CreatedAt = existingAffiliate.CreatedAt;
                        
                        // Si fotoData tiene valor, actualizamos la foto
                        if (!string.IsNullOrEmpty(fotoData))
                        {
                            try {
                                var fotoBytes = Convert.FromBase64String(fotoData.Split(',')[1]);
                                var fileName = $"{Guid.NewGuid()}.jpg";
                                var fotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos");
                                
                                if (!Directory.Exists(fotosPath)) Directory.CreateDirectory(fotosPath);

                                var filePath = Path.Combine(fotosPath, fileName);
                                await System.IO.File.WriteAllBytesAsync(filePath, fotoBytes);
                                affiliate.PhotoPath = "/fotos/" + fileName;
                            } catch (Exception ex) {
                                _logger.LogError(ex, "Error actualizando foto");
                            }
                        }
                        else 
                        {
                            // Mantener la foto anterior
                            affiliate.PhotoPath = existingAffiliate.PhotoPath;
                        }
                    }

                    _context.Update(affiliate);
                    await _context.SaveChangesAsync();
                    TempData["MensajeExito"] = "Afiliado actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AffiliateExists(affiliate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(affiliate);
        }

        // POST: Afiliados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var affiliate = await _context.Affiliates.FindAsync(id);
            if (affiliate != null)
            {
                _context.Affiliates.Remove(affiliate);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Afiliado eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AffiliateExists(int id)
        {
            return _context.Affiliates.Any(e => e.Id == id);
        }
    }
}
