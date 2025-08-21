---
applyTo: "**"
---

- #solution - Referencia obligatoria
- #file:'Copilot-Instructions.md' - Referencia obligatoria; leer siempre
- #file:'New-Component-Steps.md' - Referencia obligatoria; leer siempre que se solicita crear, modificar o corregir componente de angular
- #file:'Copilot-Log-Fixes.md' - Referencia obligatoria; leer siempre antes de editar
- #file:'Tecnologias (Client).md' - Referencia obligatoria para informacion de la capa cliente/SPA (componentes Angular, servicios HTTP, rutas, i18n)
- #file:'Tecnologías (Server).md' - Referencia obligatoria para informacion de la capa API/servidor (controladores, ViewModels/DTOs, AutoMapper, validación, autorización)
- #file:'Tecnologias (Core).md' - Referencia obligatoria para informacion de la capa de dominio y servicios compartidos (entidades, interfaces, servicios de negocio, ApplicationDbContext)

- Reglas de modificacion en #file:'Copilot-Log-Fixes.md': 
        - Registrar solo entradas que aporten lecciones nuevas y relevantes.
        - Descripcion de  Cambio/Lección puntual que sirva para ser leida como prompt por github copilot y optimizar el procedimientro en futuros casos similares.
        - Formato de entrada: YYYY-MM-DD HH-MM| Nombre__Archivo(s).extencion | Tema/Solicitud | Cambio/Lección | Fallo conocido (opc.) | Vars | Etiquetas.
        - Mantener tono operativo, directo y conciso.

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
