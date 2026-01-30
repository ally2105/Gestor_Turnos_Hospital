# 🎫 Sistema de Gestión de Turnos

Sistema web para la gestión de turnos de atención al cliente en tiempo real, desarrollado con ASP.NET Core 8.0, Entity Framework Core y SignalR.

## 📋 Descripción

Este proyecto es una aplicación web que permite:
- ✅ Gestionar afiliados y sus datos personales
- ✅ Crear y administrar cajas de atención
- ✅ Generar turnos automáticamente
- ✅ Visualizar turnos en tiempo real mediante SignalR
- ✅ Generar códigos QR para los turnos
- ✅ Llamar y atender turnos desde las cajas

## 🛠️ Tecnologías Utilizadas

- **Framework**: ASP.NET Core 8.0 (MVC)
- **ORM**: Entity Framework Core 9.0.10
- **Base de Datos**: MySQL (Pomelo.EntityFrameworkCore.MySql)
- **Comunicación en Tiempo Real**: SignalR
- **Generación de QR**: QRCoder 1.4.3
- **Contenedores**: Docker

## 📦 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) 8.0 o superior
- [Docker](https://www.docker.com/get-started) (opcional, para ejecución con contenedores)
- Un IDE compatible (Visual Studio 2022, Visual Studio Code, JetBrains Rider)

### 🔧 Instalación de .NET 8.0 SDK (Ubuntu/Linux)

Si no tienes .NET SDK instalado, ejecuta:

```bash
# Actualizar repositorios
sudo apt update

# Instalar .NET 8.0 SDK
sudo apt install -y dotnet-sdk-8.0

# Verificar la instalación
dotnet --version
```

### 🗄️ Instalación de MySQL Server (Ubuntu/Linux)

Si no tienes MySQL instalado:

```bash
# Instalar MySQL Server
sudo apt install -y mysql-server

# Verificar que esté corriendo
systemctl status mysql

# Configurar la base de datos
sudo mysql -e "CREATE DATABASE IF NOT EXISTS gestiondeturnos;"
sudo mysql -e "CREATE USER IF NOT EXISTS 'Coder'@'localhost' IDENTIFIED BY 'Qwe.123*';"
sudo mysql -e "GRANT ALL PRIVILEGES ON gestiondeturnos.* TO 'Coder'@'localhost';"
sudo mysql -e "FLUSH PRIVILEGES;"
```

## ⚙️ Configuración Inicial

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd gestor_turnos
```

### 2. Configurar la Base de Datos

#### Opción A: MySQL Local

1. Asegúrate de que MySQL esté ejecutándose en tu sistema
2. Crea una base de datos llamada `gestiondeturnos`:

```sql
CREATE DATABASE gestiondeturnos;
```

3. Actualiza la cadena de conexión en `appsettings.json` si es necesario:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=gestiondeturnos;User=TU_USUARIO;Password=TU_CONTRASEÑA;AllowUserVariables=True;TreatTinyAsBoolean=true;"
  }
}
```

#### Opción B: MySQL con Docker

```bash
docker run --name mysql-turnos \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=gestiondeturnos \
  -e MYSQL_USER=Coder \
  -e MYSQL_PASSWORD=Qwe.123* \
  -p 3306:3306 \
  -d mysql:8.0
```

### 3. Aplicar Migraciones

Navega al directorio del proyecto web y ejecuta las migraciones:

```bash
cd GestionDeTurnos.Web

# Instalar la herramienta dotnet-ef si no la tienes
dotnet tool install --global dotnet-ef

# Agregar dotnet-ef al PATH (solo la primera vez)
export PATH="$PATH:$HOME/.dotnet/tools"

# Aplicar las migraciones
dotnet ef database update
```

Si no tienes la herramienta `dotnet-ef` instalada globalmente, el comando anterior la instalará automáticamente.

## 🚀 Ejecución del Proyecto

### Método 1: Ejecución Directa con .NET CLI

1. Navega al directorio del proyecto:

```bash
cd GestionDeTurnos.Web
```

2. Restaura las dependencias:

```bash
dotnet restore
```

3. Ejecuta la aplicación:

```bash
dotnet run
```

4. Abre tu navegador en:
   - **HTTP**: http://localhost:5000
   - **HTTPS**: https://localhost:5001

### Método 2: Ejecución con Docker Compose (Recomendado)

1. En la raíz del proyecto (donde está el archivo `docker-compose.yml`), ejecuta:

```bash
docker compose up --build -d
```

2. Accede a la aplicación en:
   - http://localhost:5000

3. Para detener los servicios:

```bash
docker compose down
```

### Método 3: Ejecución Individual con Docker

1. Construye la imagen Docker:

```bash
docker build -t gestor-turnos -f GestionDeTurnos.Web/Dockerfile .
```

2. Ejecuta el contenedor:

```bash
docker run -d -p 8080:8080 -p 8081:8081 \
  --name gestor-turnos-app \
  gestor-turnos
```

3. Accede a la aplicación en:
   - http://localhost:8080

**Nota**: Asegúrate de que el contenedor pueda conectarse a tu base de datos MySQL. Si usas MySQL en Docker, considera usar Docker Compose o una red de Docker.

### Método 3: Ejecución desde Visual Studio

1. Abre el proyecto en Visual Studio 2022
2. Selecciona `GestionDeTurnos.Web` como proyecto de inicio
3. Presiona `F5` o haz clic en el botón "Run"

## 📁 Estructura del Proyecto

```
gestor_turnos/
└── GestionDeTurnos.Web/
    ├── Controllers/          # Controladores MVC
    │   ├── AfiliadosController.cs
    │   ├── BoxesController.cs
    │   ├── HomeController.cs
    │   └── TurnosController.cs
    ├── Data/                 # Contexto de base de datos
    │   └── AppDbContext.cs
    ├── Hubs/                 # Hubs de SignalR
    │   └── TurnosHub.cs
    ├── Migrations/           # Migraciones de EF Core
    ├── Models/               # Modelos de datos
    │   ├── Affiliate.cs
    │   ├── Box.cs
    │   ├── Turn.cs
    │   └── ErrorViewModel.cs
    ├── Services/             # Servicios de negocio
    │   └── TurnService.cs
    ├── Views/                # Vistas Razor
    ├── wwwroot/              # Archivos estáticos (CSS, JS, imágenes)
    ├── appsettings.json      # Configuración de la aplicación
    ├── Dockerfile            # Configuración de Docker
    └── Program.cs            # Punto de entrada de la aplicación
```

## 🔧 Comandos Útiles

### Entity Framework Core

```bash
# Crear una nueva migración
dotnet ef migrations add NombreDeLaMigracion

# Aplicar migraciones pendientes
dotnet ef database update

# Revertir a una migración específica
dotnet ef database update NombreDeLaMigracion

# Eliminar la última migración
dotnet ef migrations remove

# Ver el SQL que se ejecutará
dotnet ef migrations script
```

### Compilación y Publicación

```bash
# Compilar el proyecto
dotnet build

# Compilar en modo Release
dotnet build -c Release

# Publicar la aplicación
dotnet publish -c Release -o ./publish

# Ejecutar la aplicación publicada
dotnet ./publish/GestionDeTurnos.Web.dll
```

### Docker

```bash
# Construir la imagen
docker build -t gestor-turnos -f GestionDeTurnos.Web/Dockerfile .

# Ver logs del contenedor
docker logs gestor-turnos-app

# Detener el contenedor
docker stop gestor-turnos-app

# Eliminar el contenedor
docker rm gestor-turnos-app

# Eliminar la imagen
docker rmi gestor-turnos
```

## 🌐 Funcionalidades Principales

### 1. Gestión de Afiliados
- ✅ CRUD Completo (Crear, Leer, Editar, Eliminar)
- 📸 Captura de Fotos con Webcam en tiempo real
- 🪪 Generación de **Carnet Digital** listo para imprimir
- 🔎 Búsqueda y listado optimizado

### 2. Gestión de Cajas
- Crear y administrar cajas de atención
- Asignar cajas a operadores

### 3. Gestión de Turnos
- Generar turnos automáticamente
- Visualizar turnos en tiempo real
- Llamar turnos desde las cajas
- Generar códigos QR para los turnos
- Actualización automática mediante SignalR

## 🐛 Solución de Problemas

### Error de conexión a la base de datos

Si obtienes un error de conexión:
1. Verifica que MySQL esté ejecutándose
2. Confirma que las credenciales en `appsettings.json` sean correctas
3. Asegúrate de que la base de datos `gestiondeturnos` exista
4. Verifica que el puerto 3306 esté disponible

### Error al aplicar migraciones

```bash
# Elimina la base de datos y vuelve a crearla
dotnet ef database drop
dotnet ef database update
```

### Puerto en uso

Si el puerto está en uso, puedes cambiar el puerto en `Properties/launchSettings.json` o usar:

```bash
dotnet run --urls "http://localhost:5050;https://localhost:5051"
```

### Docker: Error de Conectividad / DNS

Si al levantar Docker obtienes errores como `dial tcp: lookup registry-1.docker.io on ... i/o timeout`, es probable que tu daemon de Docker tenga problemas de DNS. Soluciónalo ejecutando:

```bash
echo '{"dns": ["8.8.8.8", "1.1.1.1"]}' | sudo tee /etc/docker/daemon.json
sudo systemctl restart docker
```

## 📝 Variables de Entorno

Puedes configurar las siguientes variables de entorno:

```bash
# Cadena de conexión
ConnectionStrings__DefaultConnection="Server=localhost;Database=gestiondeturnos;User=root;Password=password;"

# Nivel de logging
Logging__LogLevel__Default="Information"

# Entorno de ejecución
ASPNETCORE_ENVIRONMENT="Development"
```

## 🔒 Seguridad

⚠️ **Importante**: 
- No subas el archivo `appsettings.json` con credenciales reales a repositorios públicos
- Usa `appsettings.Development.json` para desarrollo local
- Configura variables de entorno o Azure Key Vault para producción
- Cambia las contraseñas por defecto antes de desplegar

## 📄 Licencia

Este proyecto es de uso educativo/interno.

## 👥 Contribución

Para contribuir al proyecto:
1. Crea un fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agrega nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crea un Pull Request

## 📞 Soporte

Para reportar problemas o solicitar nuevas funcionalidades, por favor abre un issue en el repositorio.

---

**Desarrollado con ❤️ usando ASP.NET Core 8.0**
