## Estándar para crear nuevos componentes/entidades (Core + Server + Client)

Propósito: generar funcionalidades completas de forma consistente, evitando errores ya detectados y sin reanalizar desde cero cada vez.

Alcance: .NET Core (servicios, DI, AutoMapper, API) + Angular (modelos, servicios, widgets standalone) + i18n.

---

## Patrones estandarizados

### Backend – Core (capa de dominio)
- Contratos en `MiniERP_Suministros.Core/Services/<Area>/Interfaces/I<Entity>Service.cs`.
- Implementaciones en `MiniERP_Suministros.Core/Services/<Area>/<Entity>Service.cs`.
- Excepción específica en `MiniERP_Suministros.Core/Services/<Area>/Exceptions/<Entity>Exception.cs`.
- Consultas EF con `Include/ThenInclude` cuando corresponda; lecturas con `AsNoTracking()` y, si hay múltiples includes, `AsSingleQuery()` para evitar splitting indeseado.
- Operaciones: `Create`, `UpdatePartial` (parches parciales), `Delete` con validaciones de integridad (bloquear si existen dependencias/relaciones).
- Validaciones de negocio en el servicio (ej.: precios/stock >= 0, existencia de referencias padre/categoría, no eliminar si hay hijos o detalles de órdenes).
- En operaciones compuestas (ej.: pedidos/stock), transacción atómica; preparar futuro `RowVersion` para concurrencia optimista.

### Backend – Server (API, mapping, DI)
- Registrar servicios en DI en `MiniERP_Suministros.Server/Program.cs`.
- ViewModels en `MiniERP_Suministros.Server/ViewModels/<Area>/<Entity>VM.cs` (+ VMs hijas si aplica).
- Validación con FluentValidation para requisitos de entrada (ej.: `Name` requerido y longitudes).
- Mapeos en `MiniERP_Suministros.Server/Configuration/MappingProfile.cs` con AutoMapper (Model ↔ VM, colecciones y anidados).
- Controladores REST en `MiniERP_Suministros.Server/Controllers/<Entity>Controller.cs` con endpoints:
	- `GET` colección (con filtros/Include si aplica) y `GET {id}`.
	- `POST` crea entidad.
	- `PUT` parcial para actualizaciones sin requerir el objeto completo.
	- `DELETE` con retorno `409 Conflict` si existen dependencias (no forzar cascada).

### Frontend – Angular (modelos, servicios, widgets)
- Modelo en `MiniERP_Suministros.client/src/app/models/<entity>.model.ts`.
- Endpoints/servicio de dominio en `.../services/<entity>-*.ts` alineados al backend.
- Widget standalone en `.../components/widgets/<entity>-widget.*` con:
	- Buscador, alta inline, edición inline y eliminación.
	- Inputs/outputs tipados; evitar `any` (usar tipos o `Record<string, unknown>` cuando aplique).
	- Limpieza de recursos en `ngOnDestroy`.
	- Alerts con `MessageSeverity` (enum) del `AlertService`.
	- Plantillas HTML usando atributos estándar (`title` en lugar de `attr-title`).
- Integración del widget en el componente contenedor (ej.: `products/products.component.*`).

### i18n
- Claves separadas por widget/módulo (namespaces) y paridad entre `public/locale/es.json` y `public/locale/en.json`.
- Validar JSON y mantener orden/estructura para evitar errores de carga/parseo.

---

## Procedimiento paso a paso (checklist reusable)

1) Core (dominio)
- [ ] Crear `I<Entity>Service` con métodos: `GetAll/GetById`, `Create`, `UpdatePartial`, `Delete`.
- [ ] Implementar `<Entity>Service` con EF Core, `Include/ThenInclude` para relaciones necesarias.
- [ ] Añadir `<Entity>Exception` para casos de negocio (integridad, referencias inexistentes, etc.).
- [ ] Asegurar lecturas con `AsNoTracking()`; usar transacciones para operaciones que afectan stock u otras entidades.

2) Server (API)
- [ ] Registrar servicio en `Program.cs` (DI).
- [ ] Crear `ViewModels` y validadores (FluentValidation) con reglas de negocio básicas (requeridos, longitudes, rangos).
- [ ] Configurar AutoMapper en `MappingProfile` para entidad ↔ VM (incluidos anidados).
- [ ] Implementar `Controller` con endpoints: `GET`, `GET {id}`, `POST`, `PUT` parcial, `DELETE` con `409` si hay dependencias.
- [ ] Responder con códigos apropiados: `200/201` en altas, `400` en validaciones, `404` si no existe, `409` en conflictos.

3) Client (Angular)
- [ ] Crear modelo `models/<entity>.model.ts`.
- [ ] Implementar servicio HTTP (`services/<entity>s.service.ts`) alineado a la API.
- [ ] Crear widget standalone `components/widgets/<entity>s-widget.*` con: búsqueda, alta/edición inline, delete.
- [ ] Usar `MessageSeverity` para alerts y limpiar suscripciones en `ngOnDestroy`.
- [ ] Evitar atributos obsoletos en plantillas y mantener binding consistente con el backend.
- [ ] Integrar el widget en el componente contenedor correspondiente.

4) i18n
- [ ] Añadir claves nuevas en `public/locale/es.json` y `public/locale/en.json` bajo un namespace del widget (p. ej. `productCategoriesWidget.*`).
- [ ] Validar JSON (formatter/validator) y revisar diffs antes de guardar.

5) Verificación rápida
- [ ] Compilar backend y frontend; corregir lints y tipos.
- [ ] Probar alta: formulario completo, `201/200`, aparece en el listado tras refrescar, i18n correcto.
- [ ] Probar edición parcial: solo campos cambiados; backend acepta `PUT` parcial.
- [ ] Probar eliminación: si hay dependencias, API devuelve `409` y el UI muestra alerta adecuada.
- [ ] Revisar logs/alertas y que no haya warnings NG8002 ni errores de parseo JSON.

---

## Buenas prácticas a seguir
- Reutilizar el patrón de `Customer`/`ProductCategory`/`Orders` para nuevas entidades; acelera y reduce errores.
- Validar integridad en Core y volver a validar en Server con FluentValidation.
- Evitar `Delete` si hay dependencias; devolver `409 Conflict` con mensaje claro.
- Preferir `UpdatePartial` a reemplazos completos cuando el cliente envía parches parciales.
- Consultas de solo lectura con `AsNoTracking()`; usar `AsSingleQuery()` cuando múltiples `Include` lo requieran.
- Transacciones atómicas en operaciones que afectan stock u otras entidades relacionadas.
- Centralizar validación de longitudes y mensajes i18n para consistencia.
- En Angular, componentes `standalone`, tipado estricto, y limpieza en `ngOnDestroy`.
- Usar `MessageSeverity` y no métodos inexistentes del `AlertService`.
- Mantener paridad de i18n entre `es` y `en` y separar namespaces por widget.

---

## Errores comunes a evitar (histórico de incidencias)
- JSON de i18n truncado o con llaves desbalanceadas provoca fallos de parsing/build (validar antes de commitear).
- Uso de `attr-title` en lugar de `title` en plantillas Angular.
- Warnings/errores NG8002 por imports innecesarios o componentes no `standalone`.
- Llamadas a métodos inexistentes en `AlertService`; usar el enum `MessageSeverity`.
- `PUT` que exige objeto completo: habilitar y documentar `UpdatePartial` para parches parciales.
- Eliminación con FKs activas: devolver `409 Conflict` y bloquear `Delete` si hay hijos/dependencias.
- Desalineación de payloads/bindings entre widget y backend (mantener modelos y mapeos sincronizados).
- Olvido de registrar servicios en DI o de añadir mapeos en AutoMapper.
- Falta de `GET {id}` afectando vistas/ediciones puntuales.
- No restaurar stock al eliminar pedidos; envolver en transacción y revertir efectos.

---

## Definition of Done (DoD) por entidad/widget
- Core: contrato + servicio + excepción; validaciones y consultas con includes; pruebas manuales básicas.
- Server: DI + VM(s) + validadores + mapeos + controlador completo (GET/GET id/POST/PUT parcial/DELETE con 409).
- Client: modelo + servicio + widget standalone integrado, con i18n y alerts correctos; limpieza de recursos.
- i18n: claves nuevas en `es` y `en`, validadas.
- Verificación: build OK, sin NG8002, sin errores de parseo JSON, flujos de alta/edición/eliminación validados.

