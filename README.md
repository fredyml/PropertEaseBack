# PropertEase

PropertEase es una aplicación integral de gestión inmobiliaria diseñada para simplificar los procesos de administración, listado y búsqueda de propiedades. Este documento proporciona una visión general del proyecto, instrucciones de instalación y pautas de uso.

---

## Tabla de Contenidos

1. [Características](#caracteristicas)
2. [Tecnologías Utilizadas](#tecnologias-utilizadas)
3. [Estructura del Proyecto](#estructura-del-proyecto)
4. [Instalación](#instalacion)
5. [Uso](#uso)
6. [Pruebas](#pruebas)
7. [Documentación API](#documentacion-api)
8. [Licencia](#licencia)

---

## Características

- **Gestión de Propiedades:** Crear, actualizar, eliminar y visualizar propiedades.
- **Filtrado:** Buscar propiedades por nombre, dirección y rango de precios.
- **Paginación:** Visualizar resultados con paginación para mejor usabilidad.
- **Manejo de Errores:** Manejo centralizado de excepciones con respuestas detalladas.
- **Registro de Eventos:** Logs de aplicación usando Serilog para depuración y auditoría.
- **API Swagger:** Documentación interactiva de la API.

---

## Tecnologías Utilizadas

- **Framework Backend:** ASP.NET Core
- **Base de Datos:** MongoDB
- **Inyección de Dependencias:** DI incorporado en ASP.NET Core
- **Registro de Eventos:** Serilog
- **Documentación API:** Swashbuckle/Swagger
- **Framework de Pruebas:** xUnit con Moq

---

## Estructura del Proyecto

```
PropertEase/
|-- Controllers/        # Controladores API
|-- Filters/            # Filtros de excepciones globales
|-- Models/             # Modelos de respuesta de errores
|-- Application/
|   |-- Dtos/           # Objetos de transferencia de datos
|   |-- Interfaces/     # Interfaces de servicios y repositorios
|   |-- Services/       # Servicios de lógica de negocio
|   |-- Mapping/        # Perfiles de AutoMapper
|-- Domain/
|   |-- Entities/       # Modelos de dominio
|   |-- Exceptions/     # Excepciones personalizadas
|-- Infrastructure/
|   |-- Repositories/   # Implementaciones de repositorios para MongoDB
|-- Tests/              # Pruebas unitarias
```

---

## Instalación

### Requisitos Previos

1. [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
2. [MongoDB](https://www.mongodb.com/try/download/community)
3. [Node.js y Angular CLI](https://nodejs.org/en/) (si utilizas un frontend Angular)

### Pasos

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/username/PropertEase.git
   cd PropertEase
   ```

2. Instalar dependencias:
   ```bash
   dotnet restore
   ```

3. Configurar la cadena de conexión de MongoDB en `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "MongoDb": "mongodb://localhost:27017"
   }
   ```

4. Ejecutar la aplicación:
   ```bash
   dotnet run
   ```

---

## Uso

### Endpoints de la API

- **Obtener Propiedades Filtradas:**
  ```
  GET /api/properties
  ```
  Parámetros de Consulta:
  - `name` (opcional): Filtrar por nombre de propiedad
  - `address` (opcional): Filtrar por dirección
  - `minPrice` (opcional): Precio mínimo
  - `maxPrice` (opcional): Precio máximo
  - `page` (opcional): Número de página (por defecto: 1)
  - `pageSize` (opcional): Elementos por página (por defecto: 10)

- **Manejo de Errores:**
  Respuestas centralizadas con mensajes de error significativos.

### Swagger

1. Navega a `https://localhost:5001/swagger` (o el puerto configurado).
2. Explora e interactúa con los endpoints de la API.

---

## Pruebas

1. Navega al directorio `Tests/`:
   ```bash
   cd Tests
   ```

2. Ejecuta las pruebas:
   ```bash
   dotnet test
   ```

3. Revisa los resultados de las pruebas para identificar fallos.

---

## Documentación API

PropertEase utiliza Swagger para documentar la API. La documentación incluye todos los endpoints con descripciones detalladas y ejemplos de respuestas. Para acceder, ejecuta la aplicación y navega a:

```
https://localhost:5001/swagger
```

---

## Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo LICENSE para más detalles.

