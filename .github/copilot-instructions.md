---
applyTo: "**"
---

- Referencias obligatorias:
- #solution.
- #file:'Copilot-Instructions.md' siempre revisar antes de editar.
- #file:'Copilot-Log-Fixes.md', siempre utilizar como almacén de experiencia para consulta.

- Reglas de inclusión en #file:'Copilot-Log-Fixes.md':
        - Registrar solo entradas que aporten lecciones nuevas y relevantes.
        - Crear nueva entrada solo si el tema es distinto; para refinamientos, actualizar entrada existente e indicar "última-revisión:YYYY-MM-DD".
        - Formato de entrada (no mas de 5 lineas): YYYY-MM-DD | Archivo(s) | Tema | Cambio/Lección | Fallo conocido (opc.) | Vars | Etiquetas.
        - Evitar entradas triviales o duplicadas; priorizar precisión y brevedad.
        - Si se necesita código, referenciar ubicaciones o líneas; no volcar fragmentos largos.
        - Mantener tono operativo, directo y conciso.

- Referencias opcionales; urilizar en caso de necesidad de informacion de estructura y tecnologias aplicadas por capa:
    - Referencia #file:'Tecnologias (Client).md' para informacion de la capa cliente/SPA (componentes Angular, servicios HTTP, rutas, i18n).
    - Referencia #file:'Tecnologías (Server).md' para informacion de la capa API/servidor (controladores, ViewModels/DTOs, AutoMapper, validación, autorización).
     - Referencia #file:'Tecnologias (Core).md' para informacion de la capa de dominio y servicios compartidos (entidades, interfaces, servicios de negocio, ApplicationDbContext).

- Reglas generales:
    - Indicar cuando se detecte necesidad de nueva migración (las migraciones son manuales).

- Reglas de comentariado y documentacion:
    - Usar encabezados de sección solo cuando aporten claridad. Para contenido pequeño, basta un comentario en línea. 
    - Todos los archivos deben iniciar con un encabezado que incluya:
        - **RUTA** (ubicación relativa).
        - **Descripción** del propósito general (actualizar si cambia).
        - Ejemplo:
```
RUTA: MYAPP/EJEMPLO.SERVER/PROGRAM.CS
Configuración y arranque de la aplicación ASP.NET Core. Incluye servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y pipeline HTTP.
```
- Traducir siempre; a menos que afecte alguna referencia.
- No eliminar texto en comentarios existentes. Solo mejorar formato y redacción si aporta claridad.
- Mantener tono operativo, directo y conciso en comentarios.
- Los comentarios de una sola línea deben ir al final de la línea de código, no encima.
- En archivos de código (clases, servicios, componentes): limitar comentarios a lógica no obvia. Preferir documentación XML.
- En archivos de configuración (JSON, XML, YAML): incluir comentarios explicativos y ejemplos de opciones posibles para cada propiedad.
- Dividir grandes bloques de código en secciones con encabezado en formato:
- Ejemplo:
  ```
  - PUNTO DE MONTAJE DE ANGULAR
    El componente raíz <app-root> es donde Angular renderiza la aplicación, además de incluir un mensaje y animación de carga mientras la app se inicializa.
  ```
