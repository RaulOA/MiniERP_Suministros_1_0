# Reglas de Codificación, Documentación y Comentariado – Librería Universitaria (.NET 9 + Angular)

---

## 1. Encabezado obligatorio para cada archivo

- Todos los archivos deben incluir un encabezado al inicio con los siguientes campos:
  - Autor
  - Archivo (nombre real)
  - Solución
  - Proyecto
  - Ruta (ubicación relativa)
  - Descripción o propósito del archivo (actualizado si cambia la funcionalidad)
  - Historial de cambios (fecha y descripción de cada modificación significativa)
  - Alertas críticas (documentar observaciones relevantes, riesgos, recomendaciones)

  - Ejemplo:

 ================== MODELO DE ENCABEZADO ==================
Autor: Raul Ortega Acuña
Archivo: Program.cs
Solución: Claudes 2.0
Proyecto: Claudes_2._0.Server
Ruta: Claudes 2.0\Claudes_2._0.Server\Program.cs

Descripción o propósito del archivo:
Configuración y arranque de la aplicación ASP.NET Core. Incluye la configuración de 
servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y 
pipeline de peticiones HTTP. Realiza el registro de clientes OIDC y el seeding de la 
base de datos al iniciar.

Historial de cambios:
1. 23/07/2025 - Estructura inicial y configuración de servicios principales.
2. 02/07/2025 - Actualización de referencias a OIDC: uso de LibreriaUniversidadSpaClientID en lugar de QuickAppClientID.
               - Mejoras en comentarios y consistencia.

Alertas Críticas:
- 24/07/2025 - GitHub Copilot sugirió el uso de una instancia pública para el endpoint OIDC. Se recomienda revisar 
              y configurar acceso seguro mediante restricciones de origen.
- 24/07/2025 - Observación: el manejo de excepciones en AuthController carece de logging detallado. Puede 
              dificultar auditorías ante fallos de autenticación.
===========================================================
---

## 2. Reglas de formato y comentariado

- Los comentarios descriptivos deben estar en español, ubicados junto a la línea o sección relevante.
- No eliminar secciones comentadas como "Notas Importantes" o comentarios de código que contengan información relevante para mantenimiento, configuración, uso, desarrollo o descripción.
- Dividir el código en grandes secciones, cada una con encabezado y pie explicativo usando el formato:
  ```
  ======================================================== INICIO - NOMBRE DE LA SECCIÓN ========================================================
  // Descripción breve del propósito de esta sección.
  **********************************************************************************************************************************************
  // Código relacionado con la sección.
  ======================================================== FIN - NOMBRE DE LA SECCIÓN ===========================================================
  ```
- Reemplazar todas las referencias a nombres de la plantilla original (ebenmonney, emonney, quickapp, Quick Application, template, etc.) por "Librería Universidad" o "Raul O.A" según corresponda, siempre que no se rompan dependencias.
- Traducir texto en inglés al español, excepto nombres de variables o propiedades técnicas que deban mantenerse.
- Los metadatos deben ser congruentes con el contenido del archivo y actualizados según corresponda.
- Actualizar URLs en comentarios y documentación a las del repositorio del proyecto o documentación oficial de .NET 9, Angular 19, OpenIddict, etc.

---

## 3. Reglas de codificación y buenas prácticas

- Seguir las convenciones de nomenclatura de C#, TypeScript y tecnologías del stack para código y documentación XML.
- Mantener la coherencia visual y funcional del sistema.
- Priorizar la reutilización de componentes, servicios y estilos existentes.
- Mantener la modularidad y separación de responsabilidades entre backend y frontend, y entre lógica de negocio, datos y presentación.
- No romper el estándar de estructura de carpetas y archivos definido por la plantilla base.
- Documentar cambios relevantes para facilitar el mantenimiento y la colaboración.
- Considerar la seguridad y protección de datos en cada etapa del desarrollo.

---

## 4. Reglas para commits y control de versiones

- Realizar commits y push frecuentes, especialmente tras cambios estructurales, integración de dependencias, migraciones o implementación de funcionalidades clave.
- Los mensajes de confirmación deben seguir esta estructura:
  - Línea 1: resumen breve del cambio realizado (máx. 72 caracteres)
  - Línea 2: en blanco
  - Línea 3 en adelante: Archivo.extension: descripción completa del cambio con los detalles necesarios
- Si el commit cierra una incidencia, usar la sintaxis: `#123 closes`
- Ejemplo:
  ```
  Actualizado nombre de cliente OIDC a LibreriaUniversidadSpaClientID
  
  Program.cs: Se modificó para usar LibreriaUniversidadSpaClientID en lugar de QuickAppClientID. Se mejoraron comentarios y consistencia en los nombres del proyecto.
  ```

---

## 5. Reglas para migraciones de base de datos

- Cada migración debe tener un nombre descriptivo que refleje el cambio realizado.
- Las migraciones deben ser generadas automáticamente usando `dotnet ef migrations add NombreMigracion` desde el proyecto Libreria_Universidad.Server.
- No modificar manualmente las migraciones generadas.
- Aplicar las migraciones al entorno de desarrollo local antes de hacer push al repositorio.
- Ejecutar `dotnet ef database update` para aplicar los cambios en la base de datos.

---

## 6. Reglas para documentación técnica y manual de usuario

- Mantener la documentación actualizada y clara, incluyendo arquitectura, diagramas, explicación de componentes e instrucciones de instalación y ejecución.
- Documentar la estructura de carpetas y archivos, roles, permisos y flujos principales.
- Incluir referencias a documentación oficial y al repositorio del proyecto.

---

## 7. Reglas para pruebas y calidad

- Escribir y ejecutar pruebas unitarias y de integración (Jasmine/Karma para Angular, pruebas de endpoints en backend).
- Ejecutar análisis estático de código (ESLint/angular-eslint para frontend).
- Probar la funcionalidad tras cada cambio estructural.

---

## 8. Reglas para internacionalización y accesibilidad

- Usar @ngx-translate/core para soportar múltiples idiomas en el frontend.
- Asegurar que la interfaz sea responsiva y accesible en diferentes dispositivos.

---

## 9. Reglas para seguridad y roles

- Implementar autenticación y autorización basada en JWT, OIDC/OAuth2 y ASP.NET Core Identity.
- Restringir vistas y acciones según el tipo de usuario (Administrador, Vendedor, Cliente).
- Proteger endpoints y reportes según permisos.

---

## 10. Reglas para integración y consumo de APIs

- Usar servicios Angular para consumir endpoints CRUD y reportes.
- Implementar interceptores para manejo de JWT y protección de rutas.
- Documentar y probar la integración entre backend y frontend.

---

## 11. Reglas para diseño y experiencia de usuario

- Personalizar la interfaz, menús y dashboard para la temática de librería.
- Asegurar responsividad y usabilidad en todos los dispositivos.
- Mantener la coherencia visual y funcional en toda la aplicación.

---

Este documento resume y estructura todas las reglas de codificación, documentación, comentariado y buenas prácticas explícitas e implícitas en los archivos analizados, para asegurar calidad, coherencia y mantenibilidad en el desarrollo del sistema.
