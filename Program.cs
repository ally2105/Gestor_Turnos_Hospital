using GestionDeTurnos.Web.Data;
using GestionDeTurnos.Web.Hubs;
using GestionDeTurnos.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configurar conexión a la base de datos (MySQL en este caso)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

// 🔹 Inyectar servicios para que puedan ser usados en los controladores
builder.Services.AddScoped<TurnService>();

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // Añadir servicios de SignalR para la comunicación en tiempo real

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Ruta por defecto para los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapear el Hub de SignalR a la ruta "/turnoshub"
app.MapHub<TurnosHub>("/turnoshub");

// Aplicar migraciones automáticamente al iniciar con reintentos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    int retries = 5;
    while (retries > 0)
    {
        try
        {
            // Aplicar migraciones pendientes
            logger.LogInformation("Intentando conectar a la base de datos...");
            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Aplicando migraciones...");
                context.Database.Migrate();
            }
            else 
            {
                 // Asegurar que se puede conectar aunque no haya migraciones pendientes
                 if(!context.Database.CanConnect()) 
                 {
                     throw new Exception("No se pudo conectar a la base de datos");
                 }
            }

            // Sembrar datos iniciales (Cajas) si no existen
            if (!context.Boxes.Any())
            {
                logger.LogInformation("Creando Cajas por defecto...");
                context.Boxes.AddRange(
                    new GestionDeTurnos.Web.Models.Box { Name = "Caja 1", IsOpen = true, AssignedUser = "Operador 1" },
                    new GestionDeTurnos.Web.Models.Box { Name = "Caja 2", IsOpen = true, AssignedUser = "Operador 2" },
                    new GestionDeTurnos.Web.Models.Box { Name = "Caja 3", IsOpen = true, AssignedUser = "Operador 3" }
                );
                context.SaveChanges();
            }
            
            logger.LogInformation("Base de datos lista y conectada correctly.");
            break; // Éxito, salir del bucle
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0)
            {
                logger.LogError(ex, "Ocurrió un error crítico al conectar con la base de datos después de varios intentos.");
            }
            else
            {
                logger.LogWarning($"Fallo al conectar con la base de datos. Reintentando en 3 segundos... (Intentos restantes: {retries})");
                System.Threading.Thread.Sleep(3000);
            }
        }
    }
}

app.Run();