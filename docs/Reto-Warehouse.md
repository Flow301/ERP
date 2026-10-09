# Reto: CRUD de Warehouse

Todas las respuestas usan `Result<T>`. El código HTTP del encabezado se repite en `status` dentro del body.
En Scalar (`/scalar`) cada endpoint muestra sus códigos posibles y el esquema del JSON gracias a los
atributos `[ProducesResponseType]` de `WarehouseController`.

## Endpoints y resultados

| Petición | Código | Resultado |
|---|---|---|
| `GET /api/Warehouse` | 200 | 97 bodegas (7 del catálogo inicial + 90 nuevas) |
| `GET /api/Warehouse/1/10` | 200 | `totalRecords` 97, `totalPages` 10 |
| `GET /api/Warehouse/10/10` | 200 | 7 bodegas: de la 94 a la 100 |
| `GET /api/Warehouse/0/100` | 400 | `pageNumber` < 1 o `pageSize` fuera de 1–50 |
| `GET /api/Warehouse/94` | 200 | La bodega 94 |
| `GET /api/Warehouse/999` | 404 | No existe |
| `POST /api/Warehouse` | 201 | Creada, con encabezado `Location: /api/Warehouse/{id}` |
| `POST /api/Warehouse` sin `warehouseName` | 400 | Error de validación (DataAnnotations) |
| `POST /api/Warehouse` con nombre repetido | 409 | El nombre ya existe |
| `PUT /api/Warehouse/{id}` | 200 | Actualizada |
| `PUT /api/Warehouse/999` | 404 | No existe |
| `PUT /api/Warehouse/{id}` con el nombre de otra bodega | 409 | El nombre ya existe |
| `DELETE /api/Warehouse/{id}` sin productos | 200 | Eliminada; `data` es el id |
| `DELETE /api/Warehouse/1` (tiene productos) | 409 | No se puede eliminar |
| `DELETE /api/Warehouse/999` | 404 | No existe |

### Ejemplos de JSON

`GET /api/Warehouse/10/10` → **200**
```json
{
  "isSuccess": true,
  "status": 200,
  "title": "OK",
  "detail": "",
  "data": [
    { "idWarehouse": 94, "warehouseName": "Bodega Desarrollo Mora", "address": "San José, Mora" },
    "... 95 a 99 ...",
    { "idWarehouse": 100, "warehouseName": "Bodega Distribución Río Cuarto", "address": "Alajuela, Río Cuarto" }
  ],
  "pageNumber": 10,
  "pageSize": 10,
  "totalRecords": 97,
  "totalPages": 10
}
```

`GET /api/Warehouse/999` → **404**
```json
{ "isSuccess": false, "status": 404, "title": "Not Found", "detail": "Warehouse 999 doesn't exist!", "data": null }
```

`POST /api/Warehouse` con body `{ "warehouseName": "Bodega Prueba", "address": "Test" }` → **201**
```json
{
  "isSuccess": true, "status": 201, "title": "Created", "detail": "Record saved!",
  "data": { "idWarehouse": 101, "warehouseName": "Bodega Prueba", "address": "Test" }
}
```

`POST /api/Warehouse` con body `{ "warehouseName": "Bodega Central" }` → **409**
```json
{ "isSuccess": false, "status": 409, "title": "Conflict", "detail": "Warehouse 'Bodega Central' already exists!", "data": null }
```

`PUT /api/Warehouse/101` con body `{ "warehouseName": "Bodega Prueba", "address": "Nueva" }` → **200**
```json
{
  "isSuccess": true, "status": 200, "title": "OK", "detail": "Record updated!",
  "data": { "idWarehouse": 101, "warehouseName": "Bodega Prueba", "address": "Nueva" }
}
```

`DELETE /api/Warehouse/1` → **409**
```json
{ "isSuccess": false, "status": 409, "title": "Conflict", "detail": "Warehouse 1 has products and can't be deleted!", "data": 0 }
```

`DELETE /api/Warehouse/101` → **200**
```json
{ "isSuccess": true, "status": 200, "title": "OK", "detail": "Record deleted!", "data": 101 }
```

## Decisiones (para la defensa)

- **DTOs de entrada sin `IdWarehouse`.** En el POST lo asigna el repositorio y en el PUT viene de la ruta.
  Así el cliente no puede elegir ni cambiar el id. En `MapsterConfig` se ignora `IdWarehouse` por la misma razón.
- **DataAnnotations con los mismos largos que la tabla** (`WarehouseName` 80, `Address` 120 en `AppDbContext`).
  Así el error sale como 400 de validación y no como una excepción de SQL Server.
- **`WarehouseDto` como único DTO de salida.** La entidad solo tiene tres columnas, así que no hace falta un DTO de lista y otro de detalle como en Supplier.
- **El id se calcula con `MaxAsync + 1`.** `IdWarehouse` es `ValueGeneratedNever` (no es IDENTITY), igual que `IdSupplier`.
- **Nombre único → 409.** La tabla ya tiene el índice `UQ_Bodega_DescripcionBodega`. El servicio lo verifica antes con `AnyAsync` para responder 409 en vez de dejar que el `INSERT` falle con 500.
  - El nombre se compara después de `Trim()`.
  - En el PUT se excluye la propia bodega (`excludeId`), así se puede actualizar la dirección sin cambiar el nombre.
- **No se elimina una bodega con productos → 409.** Se usa `Product.IdWarehouse` (relación 1:N), con `AnyAsync` para no traer filas.
  En Supplier la relación es N:N y por eso allí se usa `ProductSuppliers`.
- **El orden de las verificaciones es 404 → 409.** No tiene sentido hablar de conflicto con un recurso que no existe.
- **Paginación con `OrderBy(IdWarehouse)` y tope `MaxPageSize = 50`.** `Skip`/`Take` necesitan un orden definido, y el tope evita pedir miles de registros. `totalRecords` sale de `CountAsync`.
- **`DELETE` con `ExecuteDeleteAsync`.** Envía el `DELETE` directo, sin cargar la entidad.
- **Datos con `HasData` en `CatalogSeeder` y la migración `AddMoreWarehouses`.** El `Up` solo inserta las bodegas 11–100 y el `Down` las borra, así que no hace falta borrar la base.
