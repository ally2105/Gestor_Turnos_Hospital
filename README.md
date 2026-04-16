# 🎫 Sistema de Gestión de Turnos

Sistema web moderno para la gestión de turnos de atención al cliente en tiempo real, desarrollado con ASP.NET Core 10.0, Entity Framework Core y SignalR.

## 📋 Descripción

Este proyecto es una aplicación web que permite:
- ✅ Gestionar afiliados y sus datos personales
- ✅ Crear y administrar cajas de atención
- ✅ Generar turnos automáticamente
- ✅ Visualizar turnos en tiempo real mediante SignalR
- ✅ Generar códigos QR para los turnos
- ✅ Llamar y atender turnos desde las cajas
- ✨ Interfaz moderna y responsive con diseño mejorado

## 🛠️ Tecnologías Utilizadas

- **Framework**: ASP.NET Core 10.0 (MVC)
- **ORM**: Entity Framework Core 10.0.0
- **Base de Datos**: SQLite (desarrollo local) / MySQL (producción)
- **Comunicación en Tiempo Real**: SignalR
- **Generación de QR**: QRCoder 1.4.3
- **Contenedores**: Docker
- **Diseño Frontend**: Bootstrap 5 + CSS3 Custom + Bootstrap Icons

## 📦 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started) (opcional, solo si usas MySQL con contenedores)
- Un IDE compatible (Visual Studio 2022, Visual Studio Code, JetBrains Rider)

### 🔧 Instalación de .NET 10.0 SDK

Si no tienes .NET 10 SDK instalado, descárgalo desde [aquí](https://dotnet.microsoft.com/download/dotnet/10.0).

Verifica la instalación:
```bash
dotnet --version
```

## ⚙️ Configuración Inicial

### 1. Clonar el Repositorio

```bash
git clone <url-del-repositorio>
cd Gestor_Turnos_Hospital
```

### 2. Restaurar Dependencias

```bash
dotnet restore
```

### 3. Migraciones (Automáticas en Desarrollo)

La aplicación aplica las migraciones automáticamente al iniciar en ambiente de desarrollo:

```bash
# Para crear una nueva migración manualmente (si es necesario)
dotnet ef migrations add NombreMigracion

# Para aplicar migraciones manualmente
dotnet ef database update
```

### 4. Base de Datos

#### Ambiente de Desarrollo (SQLite - Predeterminado)
- ✅ **Sin configuración requerida**
- Se crea automáticamente como `gestiondeturnos.db` (local) o `/app/data/gestiondeturnos.db` (Docker)
- Ideal para desarrollo local sin dependencias externas
- Compatible con Docker y desarrollo local

#### Docker (Producción)
Docker usa SQLite embebido en la aplicación:
- Base de datos: `/app/data/gestiondeturnos.db`
- Persiste en volumen: `app-data`
- Sin necesidad de MySQL o servicios externos

#### Ambiente Personalizado
Para cambiar la configuración de base de datos, edita el `Program.cs` o establece variables de entorno en el `docker-compose.yml`.

## 🚀 Ejecución del Proyecto

### Método 1: Ejecución con Docker Compose ✨ (Recomendado)

1. En la raíz del proyecto (donde está el archivo `docker-compose.yml`):

```bash
# Construir e iniciar todos los servicios
docker compose up --build -d

# Espera 5-10 segundos para que la aplicación inicie
```

2. **Accede a la aplicación en**: `http://localhost:5000`

3. Para detener los servicios:

```bash
docker compose down
```

**Ventajas**:
- ✅ Sin dependencias externas
- ✅ SQLite embebido en la aplicación
- ✅ Base de datos persistente
- ✅ Listo para producción

### Método 2: Ejecución Directa con .NET CLI (Desarrollo Local)

1. En el directorio raíz del proyecto:

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run
```

2. **Abre tu navegador en**: `http://localhost:5239`

La aplicación se ejecuta con SQLite automáticamente. La base de datos se crea en `gestiondeturnos.db`.

**Ventajas**:
- ✅ Inicio más rápido
- ✅ Ideal para desarrollo
- ✅ Sin contenedores

### Método 3: Ejecución desde Visual Studio

1. Abre el proyecto en Visual Studio 2022
2. Selecciona `GestionDeTurnos.Web` como proyecto de inicio
3. Presiona `F5` o haz clic en el botón "Run"
4. Se abrirá en `http://localhost:5239`

## 🌐 URLs de Acceso

| Página | Docker | Local |
|--------|--------|-------|
| **Home** | http://localhost:5000 | http://localhost:5239 |
| **Panel de Turnos** | http://localhost:5000/Turnos | http://localhost:5239/Turnos |
| **Solicitar Turno** | http://localhost:5000/Turnos/Solicitar | http://localhost:5239/Turnos/Solicitar |
| **Gestión de Afiliados** | http://localhost:5000/Afiliados | http://localhost:5239/Afiliados |
| **Pantalla TV** | http://localhost:5000/Turnos/Tv | http://localhost:5239/Turnos/Tv |

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
- Pantalla TV para mostrar turnos en tiempo real

### 4. Interfaz Moderna y Responsiva
- ✨ Diseño mejorado con gradientes y animaciones
- 🎨 Paleta de colores profesional (Indigo, Verde, Gris)
- 📱 Completamente responsivo para móviles y tablets
- ⚡ Efectos hover suaves y transiciones
- 🎯 Iconos Bootstrap Icons integrados
- 📊 Tableros con estadísticas en tiempo real

## 🐛 Solución de Problemas

### Error "No such file or directory" (sqlite)
```bash
# Elimina la base de datos y déjala recrear automáticamente
rm gestiondeturnos.db
dotnet run
```

### Docker: La aplicación no responde en http://localhost:5000
```bash
# Espera más tiempo (hasta 15 segundos)
# Ver los logs de la aplicación
docker logs gestor-turnos-app

# Reiniciar los contenedores
docker compose restart
```

### Error de migraciones pendientes
```bash
# El proyecto aplica migraciones automáticamente al iniciar
# Si necesitas hacerlo manualmente:
dotnet ef database update
```

### Puerto 5239 o 5000 en uso
```bash
# Cambiar puerto para dotnet run
dotnet run --urls "http://localhost:5050"

# Para Docker, edita docker-compose.yml y cambia "5000:8080"
```

### Docker: Volumen o datos inconsistentes
```bash
# Eliminar volumen y recrear
docker compose down -v
docker compose up --build -d
```

### Error al conectar en Docker logs
```bash
# Ver logs completos
docker logs gestor-turnos-app -f

# Reconstruir sin caché
docker compose up --build --no-cache -d
```

## � Cambios Recientes (v2.0.0)

### ✨ Mejoras Implementadas
- ⬆️ Actualizado a **.NET 10.0**
- 📦 **SQLite** como base de datos única (desarrollo local y Docker)
- 🎨 **Diseño Frontend Completamente Renovado**:
  - Gradientes modernos y animaciones suaves
  - Paleta de colores profesional
  - Componentes responsivos con Bootstrap 5
  - Iconos Bootstrap Icons integrados
  - Efectos hover interactivos
- 🐳 **Docker Compose Optimizado**:
  - Sin dependencias externas (MySQL eliminado)
  - SQLite embebido en la aplicación
  - Volumen persistente para datos
  - Inicio rápido (~10 segundos)
- 📊 Nuevas secciones de estadísticas
- ⚡ Mejor rendimiento y optimizaciones
- 🔧 Migraciones automáticas al iniciar

### 🔄 Compatibilidad
- ✅ SQLite para desarrollo local y Docker
- ✅ Docker Compose para producción
- ✅ .NET CLI para desarrollo rápido
- ✅ Visual Studio 2022 soportado

## 📝 Variables de Entorno

```bash
# Para desarrollo (SQLite - predeterminado en IsDevelopment=true)
# No se necesita configuración especial

# Para producción con MySQL
ConnectionStrings__DefaultConnection="Server=tu-host;Port=3306;Database=gestiondeturnos;User=usuario;Password=contraseña;"

# Nivel de logging
Logging__LogLevel__Default="Information"

# Entorno
ASPNETCORE_ENVIRONMENT="Development"
```

## 🔒 Seguridad

⚠️ **Importante**: 
- ✅ SQLite en desarrollo está habilitado de forma predeterminada
- ❌ No subas credenciales reales a repositorios públicos
- Usa `appsettings.Development.json` para desarrollo local
- Configura variables de entorno o Azure Key Vault para producción
- Cambia las credenciales por defecto antes de desplegar a producción

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

**Desarrollado con ❤️ usando ASP.NET Core 10.0**
