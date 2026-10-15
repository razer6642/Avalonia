# Compendio del Aventurero

Trabajo Práctico 4 — Programación II (POO)

Aplicación de escritorio para gestionar personajes de Dungeons & Dragons. Permite listar, buscar, ver, crear, editar y borrar personajes, y **exportar la ficha de cada personaje a PDF**. Está dividida en un **cliente** (Avalonia UI) y un **servidor** (ASP.NET Core Web API con Entity Framework Core y SQLite).

- **Autor:** Rafael Fernandez

---

## Tecnologías

| Parte | Tecnología |
|---|---|
| Cliente | Avalonia UI 11, C#, patrón MVVM con CommunityToolkit.Mvvm |
| Servidor | ASP.NET Core Web API con Controllers (`net10.0`) |
| Acceso a datos | Entity Framework Core 10 (Code First, migraciones) |
| Base de datos | SQLite (archivo `dnd.db`) |
| Comunicación | HTTP + JSON (`HttpClient` en el cliente) |
| Exportación a PDF | QuestPDF (licencia Community), en el cliente |

## Estructura del repositorio

```
Avalonia/
├── MiSolucion.Api/          Servidor (Web API)
│   ├── Controllers/         PersonajesController, ClasesController, RazasController
│   ├── Data/                AppDbContext (DbSets y datos iniciales)
│   ├── Dtos/                PersonajeDto
│   ├── Models/              Entidades: Personaje, Clase, Raza, Habilidad, Hechizo
│   ├── Migrations/          Migraciones de EF Core
│   └── Program.cs
└── MyAvaloniaApp/           Cliente (Avalonia)
    ├── Models/              Personaje (modelo de la vista) y ApiModels (reflejan el JSON de la API)
    ├── Services/            ApiService (HttpClient) y PdfService (exportación de la ficha)
    ├── ViewModels/          Un ViewModel por pantalla + MainViewModel (navegación)
    └── Views/               Un archivo .axaml por pantalla
```

## Modelo de datos

Cinco entidades relacionales:

| Entidad | Campos principales | Relaciones |
|---|---|---|
| **Personaje** | Id, Nombre, Nivel | Pertenece a una **Clase** y a una **Raza**; tiene muchas **Habilidades** y muchos **Hechizos** |
| **Clase** | Id, Nombre | Una clase tiene muchos personajes |
| **Raza** | Id, Nombre | Una raza tiene muchos personajes |
| **Habilidad** | Id, Nombre, Descripcion | Pertenece a un personaje |
| **Hechizo** | Id, Nombre, Descripcion, Nivel | Pertenece a un personaje |

Todas las relaciones son uno a muchos y se resuelven por convención de EF Core (clave foránea + propiedad de navegación). Las clases y razas se cargan automáticamente en la base mediante `HasData` (datos iniciales).

## API REST

URL base en desarrollo: `http://localhost:5028` (el puerto se puede cambiar en `Properties/launchSettings.json`).

### Personajes (CRUD completo)

| Método | Ruta | Descripción | Respuestas |
|---|---|---|---|
| GET | `/api/Personajes` | Lista todos los personajes con su clase, raza, habilidades y hechizos | 200 |
| GET | `/api/Personajes/{id}` | Devuelve un personaje | 200, 404 |
| POST | `/api/Personajes` | Crea un personaje | 201, 400 |
| PUT | `/api/Personajes/{id}` | Edita un personaje | 204, 400, 404 |
| DELETE | `/api/Personajes/{id}` | Elimina un personaje | 204, 404 |

Cuerpo de `POST` y `PUT`:

```json
{ "nombre": "Miguel", "nivel": 2, "claseId": 1, "razaId": 1 }
```

### Clases

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/Clases` | Lista todas las clases |
| GET | `/api/Clases/{id}` | Devuelve una clase (404 si no existe) |

### Razas

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/Razas` | Lista todas las razas |
| GET | `/api/Razas/{id}` | Devuelve una raza (404 si no existe) |

## Pantallas del cliente

1. **Login:** acceso con usuario fijo (`admin` / `1234`). Es una validación local de prueba, sin autenticación real.
2. **Inicio:** menú principal con acceso a Personajes y cierre de sesión.
3. **Personajes:** lista de personajes leídos desde la API, con búsqueda por nombre o clase (consulta LINQ) y orden alfabético. Al seleccionar uno se abre su ficha.
4. **Ficha de personaje:** datos del personaje, pestañas de Habilidades y Hechizos, y botones para editar, borrar y **exportar a PDF**.
5. **Nuevo / Editar personaje:** formulario con listas desplegables de clase y raza cargadas desde la API. La misma pantalla se usa para crear (`POST`) y para editar (`PUT`).

La navegación la maneja `MainViewModel`, que cambia la página actual; el `ViewLocator` resuelve cada ViewModel con su vista.

## Funcionalidad adicional: exportar la ficha a PDF

Desde la ficha de un personaje, el botón **Exportar PDF** genera un documento A4 con el nombre, la raza, la clase, el nivel, las habilidades y los hechizos del personaje.

- El archivo se guarda en la carpeta **Documentos** del usuario con el nombre `Ficha_NombreDelPersonaje.pdf`, y se abre con el lector de PDF predeterminado. La ruta también se muestra en pantalla.
- Se genera en el cliente con la biblioteca **QuestPDF** (licencia Community, gratuita para uso personal y académico), en `MyAvaloniaApp/Services/PdfService.cs`. No requiere cambios en la API.
- Si el PDF no se puede generar, se muestra un mensaje de error sin cerrar la aplicación.

## Cómo ejecutarlo

### Requisitos

- SDK de .NET 10 (la API usa `net10.0`; el SDK 10 también compila el cliente)
- Herramienta de migraciones de EF Core:

```bash
dotnet tool install --global dotnet-ef
```

### Configuración de la conexión

- **Base de datos:** la cadena de conexión está en `MiSolucion.Api/appsettings.json`, en `ConnectionStrings:DefaultConnection`. Con SQLite alcanza con el nombre del archivo (`Data Source=dnd.db`); el archivo se crea en la carpeta de la API al aplicar las migraciones. No hay que instalar ningún servidor de base de datos.
- **Cliente a API:** la dirección de la API está en `MyAvaloniaApp/Services/ApiService.cs` (`BaseAddress`, por defecto `http://localhost:5028/`).

### 1. Levantar la API

```bash
cd MiSolucion.Api
dotnet ef database update     
dotnet run                    
```

Dejá esta terminal abierta mientras uses el cliente. El puerto que figura en la consola (`Now listening on: ...`) tiene que coincidir con el de `ApiService.cs`.

### 2. Levantar el cliente

En otra terminal:

```bash
cd MyAvaloniaApp
dotnet run
```

La primera vez, `dotnet run` restaura los paquetes (incluido QuestPDF), así que necesita conexión a internet.

Ingresá con `admin` / `1234`.

Si la API corre en otro puerto, hay que actualizar la dirección base en `MyAvaloniaApp/Services/ApiService.cs`.

## Migraciones

Las migraciones de EF Core están en `MiSolucion.Api/Migrations` e incluyen la creación de las cinco tablas y la carga de datos iniciales. Para generar una nueva después de modificar las entidades:

```bash
cd MiSolucion.Api
dotnet ef migrations add NombreDeLaMigracion
dotnet ef database update
```

## Decisiones de diseño

- **DTO para crear y editar:** `PersonajeDto` recibe solo nombre, nivel y los ids de clase y raza. Así el cliente no necesita enviar objetos anidados.
- **`[JsonIgnore]` en las propiedades de navegación inversa** (`Clase.Personajes`, `Raza.Personajes`, `Habilidad.Personaje`, `Hechizo.Personaje`), para evitar ciclos en el JSON.
- **Validación en la API:** `PersonajeDto` usa DataAnnotations (nombre obligatorio de 2 a 50 caracteres, nivel entre 1 y 20) y la API responde 400 automáticamente si no se cumplen. Además, al crear o editar se comprueba que la clase y la raza existan, y si no, devuelve 400.
- **Validación en el cliente:** el formulario valida nombre, clase, raza y nivel antes de llamar a la API, y muestra el error en pantalla.
- **Errores visibles:** si la API no responde, la lista, el formulario y la ficha muestran un mensaje en vez de cerrarse.
- **Consulta LINQ:** la lista filtra con `Where` por nombre o clase (sin distinguir mayúsculas) y ordena con `OrderBy`.
- **Modelos propios en el cliente:** las clases `...Api` reflejan el JSON del servidor, y `Personaje` es el modelo simplificado que usan las pantallas.

## Limitaciones conocidas

- El login es de prueba (usuario y contraseña fijos en el código).
- Las habilidades y hechizos se muestran en la ficha, pero todavía no se pueden crear ni editar desde el cliente ni desde la API.
- Clases y razas son de solo lectura (se cargan como datos iniciales).
- Borrar un personaje no pide confirmación.
- Si la API no está corriendo, el cliente muestra un mensaje de error de conexión y la lista queda vacía.
- El PDF de la ficha se guarda siempre en la carpeta Documentos, sin permitir elegir otra ubicación. Si el personaje no tiene habilidades o hechizos, el PDF lo indica.
