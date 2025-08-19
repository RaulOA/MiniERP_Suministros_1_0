---
applyTo: "**"
---

- Todos los archivos deben iniciar con un encabezado que incluya:
  - **RUTA** (ubicación relativa).
  - **Descripción** del propósito general (actualizar si cambia).
  - Ejemplo:
    ```
    RUTA: MYAPP/EJEMPLO.SERVER/PROGRAM.CS
    Configuración y arranque de la aplicación ASP.NET Core. Incluye servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y pipeline HTTP.
    ```
- No eliminar comentarios existentes. Solo mejorar redacción si aporta claridad.  
- Traducir únicamente si no rompe referencias externas.  
- Indicar cuando se detecte necesidad de nueva migración (las migraciones son manuales).  
- Los comentarios de una sola línea deben ir al final de la línea de código, no encima.  
- En archivos de código (clases, servicios, componentes): limitar comentarios a lógica no obvia. Preferir documentación XML.  
- En archivos de configuración (JSON, XML, YAML): incluir comentarios explicativos y ejemplos de opciones posibles para cada propiedad.  
- Dividir grandes bloques de código en secciones con encabezado en formato:
- Ejemplo:
  ```
  - PUNTO DE MONTAJE DE ANGULAR
    El componente raíz <app-root> es donde Angular renderiza la aplicación, además de incluir un mensaje y animación de carga mientras la app se inicializa.
  ```
- Usar encabezados de sección solo cuando aporten claridad. Para contenido pequeño, basta un comentario en línea.

