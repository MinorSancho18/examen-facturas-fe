# Sistema de Facturación - Frontend

Frontend MVC .NET 8 con Clean Architecture para consumir API de facturación.

## Requisitos
- .NET 8 SDK
- Backend API corriendo en https://localhost:7001

## Configuración

1. Clonar repositorio
   ```bash
   git clone https://github.com/MinorSancho18/examen-facturas-fe.git
   cd examen-facturas-fe
   ```

2. Descargar las librerías JavaScript requeridas:
   
   **Opción A - Usando el script (recomendado):**
   ```bash
   ./download-libs.sh
   ```
   
   **Opción B - Manualmente:**
   - Descargar jQuery 3.7.1 de https://code.jquery.com/jquery-3.7.1.min.js
   - Descargar Bootstrap 5.3.3 de https://getbootstrap.com/
   - Descargar DataTables 2.0 de https://datatables.net/
   - Descargar SweetAlert2 de https://sweetalert2.github.io/
   - Descargar jQuery Validation de https://jqueryvalidation.org/
   
   Colocar todos los archivos en sus respectivos directorios dentro de `src/Frontend.Web/wwwroot/lib/`

3. Configurar URL del API en `src/Frontend.Web/appsettings.json`:
   ```json
   "ApiSettings": {
     "BaseUrl": "https://localhost:7001"
   }
   ```

4. Restaurar paquetes:
   ```bash
   dotnet restore
   ```

5. Ejecutar proyecto:
   ```bash
   cd src/Frontend.Web
   dotnet run
   ```

6. Abrir navegador en `https://localhost:5001`

## Estructura del Proyecto

El proyecto sigue Clean Architecture con 4 capas:

```
/src
  /Frontend.Domain          - Modelos de dominio
    /Models                 - Cliente, Producto, Factura, LineaFactura, Tipo, EstadoFactura
  
  /Frontend.Application     - DTOs e Interfaces
    /Common                 - ApiResult<T>
    /DTOs                   - DTOs que mapean 1:1 con el API
      /Commands             - Comandos (Crear*, Actualizar*, Anular*)
    /Interfaces             - Interfaces de servicios API
  
  /Frontend.Infrastructure  - Implementaciones
    /Configuration          - ApiSettings
    /Http                   - HttpJsonClient
    /Services               - Servicios que consumen el API
  
  /Frontend.Web            - Capa de presentación MVC
    /Controllers            - Controladores MVC
    /Views                  - Vistas Razor
    /wwwroot                - Archivos estáticos (CSS, JS, librerías)
```

## Funcionalidades

### Clientes
- Listar clientes con paginación
- Crear nuevo cliente
- Editar cliente existente
- Eliminar cliente
- Activar/desactivar cliente

### Productos
- Listar productos con paginación
- Crear nuevo producto
- Editar producto existente
- Eliminar producto
- Clasificar por tipos (Producto/Servicio)
- Gestión de precio e impuesto

### Facturas
- Crear factura en estado Borrador
- Filtrar por cliente, fecha y estado
- Ver detalle completo de factura
- Agregar/editar/eliminar líneas (solo en Borrador)
- Emitir factura (requiere al menos 1 línea)
- Anular factura con motivo
- Cálculo automático de totales

## Reglas de Negocio Implementadas

1. **Factura solo editable en estado Borrador**: Los controles se deshabilitan automáticamente si la factura no está en estado Borrador
2. **No se puede emitir sin líneas**: Se valida que exista al menos una línea antes de permitir la emisión
3. **Cálculo automático de totales**: El API calcula automáticamente subtotales, impuestos y total
4. **Cliente debe estar activo**: Solo se muestran clientes activos en los selectores
5. **Estados de factura con badges**: 
   - Borrador (secundario/gris)
   - Emitida (éxito/verde)
   - Anulada (peligro/rojo)

## Tecnologías Utilizadas

- **Backend**: ASP.NET Core MVC 8.0
- **Frontend**:
  - jQuery 3.7.1
  - Bootstrap 5.3.3
  - DataTables.net 2.0
  - SweetAlert2
- **Arquitectura**: Clean Architecture
- **Comunicación HTTP**: HttpClient con JSON
- **Patrón de diseño**: Repository Pattern, Dependency Injection

## Endpoints Consumidos

### Clientes
- `GET /api/clientes` - Listar con paginación
- `GET /api/clientes/{id}` - Obtener por ID
- `POST /api/clientes` - Crear
- `PUT /api/clientes/{id}` - Actualizar
- `DELETE /api/clientes/{id}` - Eliminar

### Productos
- `GET /api/productos` - Listar con paginación
- `GET /api/productos/{id}` - Obtener por ID
- `POST /api/productos` - Crear
- `PUT /api/productos/{id}` - Actualizar
- `DELETE /api/productos/{id}` - Eliminar

### Facturas
- `GET /api/facturas` - Listar con filtros y paginación
- `GET /api/facturas/{id}` - Obtener por ID con detalle
- `POST /api/facturas` - Crear
- `PUT /api/facturas/{id}` - Actualizar
- `POST /api/facturas/{id}/emitir` - Emitir
- `POST /api/facturas/{id}/anular` - Anular

### Líneas de Factura
- `GET /api/lineas-factura/por-factura/{idFactura}` - Obtener líneas
- `POST /api/lineas-factura` - Agregar línea
- `PUT /api/lineas-factura/{id}` - Actualizar línea
- `DELETE /api/lineas-factura/{id}` - Eliminar línea

### Catálogos
- `GET /api/tipos` - Listar tipos (Producto/Servicio)
- `GET /api/estados-factura` - Listar estados de factura

## Características Destacadas

- **DataTables con paginación server-side**: Optimizado para grandes volúmenes de datos
- **Modales Bootstrap**: Para crear, editar y ver detalles sin recargar la página
- **Validación en tiempo real**: Mensajes de error claros y específicos
- **SweetAlert2**: Alertas y confirmaciones elegantes
- **Helper AJAX centralizado**: Manejo consistente de peticiones HTTP
- **Responsive Design**: Compatible con dispositivos móviles
- **Manejo de errores robusto**: Captura y muestra errores del API de forma amigable

## Desarrollo

### Compilar el proyecto
```bash
dotnet build
```

### Ejecutar en modo desarrollo
```bash
cd src/Frontend.Web
dotnet run
```

### Configurar puerto
El proyecto se ejecuta por defecto en:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

Para cambiar los puertos, editar `src/Frontend.Web/Properties/launchSettings.json` (se crea automáticamente).

## Notas Importantes

- **NO** usar Docker ni contenedores
- **NO** crear global.json
- El backend API debe estar corriendo antes de ejecutar el frontend
- Las credenciales y configuraciones sensibles deben manejarse mediante appsettings.json
- Los archivos estáticos están en wwwroot/ y se sirven automáticamente

## Autor

Sistema desarrollado como examen de facturación frontend con Clean Architecture.

## Licencia

Este proyecto es de uso educativo.