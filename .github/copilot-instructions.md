Todos los archivos deben incluir un encabezado al inicio con los siguientes campos:

+ RUTA (ubicación relativa)
- Descripción o propósito general del archivo (actualizar si cambia la funcionalidad)

Ejemplo de encabezado:
+ RUTA: MYAPP\EJEMPLO.SERVER\PROGRAM.CS
- Configuración y arranque de la aplicación ASP.NET Core. Incluye servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y pipeline HTTP.

Los comentarios descriptivos deben estar en español. No eliminar comentarios existentes; solo traducir y mejorar la redacción para mayor claridad.

Traducir texto en inglés al español siempre que no afecte referencias externas. Solo traducir si se tiene certeza de que no generará errores.

Seguir las convenciones de nomenclatura actuales para cada archivo. Mantener coherencia visual y funcional en el sistema.

No modificar la estructura de carpetas y archivos definida en la solución.

Mantener el uso de Bootstrap y Bootswatch ya implementado.

Actualizar los archivos `.json` en `MiniERP_Suministros.client\public\locale\` después de cualquier cambio que afecte el soporte de idiomas.

Las migraciones se realizan manualmente. Avisar cuando se detecte la necesidad de una nueva migración.

Los comentarios de una sola línea deben colocarse al final de la línea de código correspondiente, no en líneas separadas arriba.

En archivos de configuración (por ejemplo, JSON, XML, YAML), se recomienda incluir comentarios explicativos y ejemplos de opciones posibles, ya que estos archivos suelen requerir mayor claridad sobre el propósito y alternativas de cada propiedad.

En archivos de código (clases, componentes, servicios, etc.), evitar sobrecargar con comentarios. Limitarse a los necesarios para la comprensión del propósito o lógica no evidente. Preferir documentación XML si es la convención del lenguaje o framework.

Dividir el código en grandes secciones, cada una con un encabezado explicativo en el siguiente formato:

NOMBRE DE LA SECCIÓN
 Descripción breve y clara, usando palabras simples, sobre el propósito de la sección.

Ejemplo de encabezado de sección:

+ PUNTO DE MONTAJE DE ANGULAR
- El componente raíz <app-root> es donde Angular renderiza la aplicación, además de incluir un mensaje y animación de carga mientras la app se inicializa.

Las secciones deben usarse para dividir grandes bloques de código con propósitos generales claramente diferenciados (por ejemplo, en archivos de configuración extensos como web.config). Si una sección contiene poco contenido, basta con un comentario breve en línea.

No acumular encabezados de sección innecesariamente; solo usar cuando realmente ayuden a la organización y comprensión del archivo.

Para crear nuevas pestañas en la app, usar como referencia:  
`MiniERP_Suministros.client\src\app\components\home\`

Para crear nuevos widgets, usar como referencia `todo-demo.component` y ubicarlos en:  
`MiniERP_Suministros.client\src\app\components\controls\`

Para pruebas unitarias de componentes, usar como referencia:  
`MiniERP_Suministros.client\src\app\app.component.spec.ts`