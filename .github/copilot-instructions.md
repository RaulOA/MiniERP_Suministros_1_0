Todos los archivos deben incluir un encabezado al inicio con los siguientes campos:

- Ruta (ubicación relativa)
- Descripción o propósito general del archivo (actualizar si cambia la funcionalidad)

Ejemplo de encabezado:
Ruta: myapp\ejemplo.Server\Program.cs  
Configuración y arranque de la aplicación ASP.NET Core. Incluye servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y pipeline HTTP. Registra clientes OIDC y realiza el seeding de la base de datos.

Los comentarios descriptivos deben estar en español. No eliminar comentarios existentes; solo traducir y mejorar redacción para mayor claridad.

Traducir texto en inglés al español siempre que no afecte referencias externas. Solo traducir si se tiene certeza de que no genera errores.

Dividir el código en grandes secciones, cada una con un encabezado explicativo en el siguiente formato:

NOMBRE DE LA SECCIÓN ========================================================  
// Descripción breve y entendible con palabras simples del propósito de esta sección.  
=============================================================================

Seguir las convenciones de nomenclatura actuales para cada archivo. Mantener coherencia visual y funcional del sistema.

No romper el estándar de estructura de carpetas y archivos definido en la solución.

Mantener el uso de Bootstrap y Bootswatch ya implementado.

Actualizar los archivos `.json` en `MiniERP_Suministros.client\public\locale\` después de cualquier cambio que afecte el soporte de idiomas.

Las migraciones se realizan manualmente. Avisar cuando se detecte la necesidad de una nueva migración.

Para crear nuevas pestañas en la app, usar como referencia:  
`MiniERP_Suministros.client\src\app\components\home\`

Para crear nuevos widgets, usar como referencia `todo-demo.component` y ubicarlos en:  
`MiniERP_Suministros.client\src\app\components\controls\`

Para pruebas unitarias de componentes, usar como referencia:  
`MiniERP_Suministros.client\src\app\app.component.spec.ts`