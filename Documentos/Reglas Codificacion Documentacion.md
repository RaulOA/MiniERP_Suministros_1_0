# Reglas de Codificación, Documentación y Comentariado – Librería Universitaria (.NET 9 + Angular)

---

## 1. Encabezado obligatorio para cada archivo

- Todos los archivos deben incluir un encabezado al inicio con los siguientes campos:
- 
  - Ruta (ubicación relativa)

  - Descripción o propósito del general del archivo (actualizar si cambia la funcionalidad)

  Ejemplo:

   ```
- 
 ================== MODELO DE ENCABEZADO ==================
Ruta: Claudes 2.0\Claudes_2._0.Server\Program.cs

Configuración y arranque de la aplicación ASP.NET Core. Incluye la configuración de 
servicios, autenticación, autorización, OpenIddict, Swagger, CORS, AutoMapper y 
pipeline de peticiones HTTP. Realiza el registro de clientes OIDC y el seeding de la 
base de datos al iniciar.
===========================================================

  ```

---

## 2. Reglas de formato y comentariado

  ```

- Los comentarios descriptivos deben estar en español, ubicados junto a la línea o sección relevante.
- No eliminar comentarios ya existentes, solo traducir y mejorar redaccion para entender de que trata
- Dividir el código en grandes secciones, cada una con encabezado explicativo usando el formato:

  ```

  ======================================================== NOMBRE DE LA SECCIÓN ========================================================
  // Descripción breve del propósito de esta sección.
  ==========================================================================================================================================
  // Código relacionado con la sección.

  ```

- Traducir texto en inglés al español, siempre que sean textos o variables que no afecten referencias exteriores, para traducir debes estar 100% seguro que no hay peligro de error.

---

## 3. Reglas de codificación y buenas prácticas

- Seguir las convenciones de nomenclatura de C#, TypeScript y tecnologías del stack para código y documentación XML.
- Mantener la coherencia visual y funcional del sistema.
- No romper el estándar de estructura de carpetas y archivos definido en la solucion.

---


## 5. Reglas para migraciones de base de datos

- las migraciones se haran manualmente, avisa cuando se necesite hacer una 



---

## 7. Reglas para pruebas y calidad

- Escribir y ejecutar pruebas unitarias y de integración (Jasmine/Karma para Angular, pruebas de endpoints en backend).
- las pruebas deben cubrir todos los casos de uso relevantes y ser fácilmente ejecutables.
- Ejecutar análisis estático de código (ESLint/angular-eslint para frontend).
- Probar la funcionalidad tras cada cambio estructural.

---

## 8. Reglas para internacionalización y accesibilidad

- hacer las modificaciones necesarias en los .json en MiniERP_Suministros.client\public\locale\ despues de cualquier cambio que afecte el soporte de idiomas

---

## 10. Reglas para integración y consumo de APIs

- Usar servicios Angular para consumir endpoints CRUD y reportes.
- Implementar interceptores para manejo de JWT y protección de rutas.
- Documentar y probar la integración entre backend y frontend.

---

## 11. Reglas para diseño y experiencia de usuario

- Mantener el uso de boostrap y bootswatch que yase utiliza en los componentes y estilos existentes.



---

## 11. pruebas unitarias

- utiliza MiniERP_Suministros.client\src\app\app.component.spec.ts como modelo para hacer pruebas unitarias de componentes