# Reglas de Codificaci�n, Documentaci�n y Comentariado � Librer�a Universitaria (.NET 9 + Angular)

---

## 1. Encabezado obligatorio para cada archivo

- Todos los archivos deben incluir un encabezado al inicio con los siguientes campos:
- 
  - Ruta (ubicaci�n relativa)

  - Descripci�n o prop�sito del general del archivo (actualizar si cambia la funcionalidad)

  Ejemplo:

   ```
- 
 ================== MODELO DE ENCABEZADO ==================
Ruta: Claudes 2.0\Claudes_2._0.Server\Program.cs

Configuraci�n y arranque de la aplicaci�n ASP.NET Core. Incluye la configuraci�n de 
servicios, autenticaci�n, autorizaci�n, OpenIddict, Swagger, CORS, AutoMapper y 
pipeline de peticiones HTTP. Realiza el registro de clientes OIDC y el seeding de la 
base de datos al iniciar.
===========================================================

  ```

---

## 2. Reglas de formato y comentariado

  ```

- Los comentarios descriptivos deben estar en espa�ol, ubicados junto a la l�nea o secci�n relevante.
- No eliminar comentarios ya existentes, solo traducir y mejorar redaccion para entender de que trata
- Dividir el c�digo en grandes secciones, cada una con encabezado explicativo usando el formato:

  ```

  ======================================================== NOMBRE DE LA SECCI�N ========================================================
  // Descripci�n breve del prop�sito de esta secci�n.
  ==========================================================================================================================================
  // C�digo relacionado con la secci�n.

  ```

- Traducir texto en ingl�s al espa�ol, siempre que sean textos o variables que no afecten referencias exteriores, para traducir debes estar 100% seguro que no hay peligro de error.

---

## 3. Reglas de codificaci�n y buenas pr�cticas

- Seguir las convenciones de nomenclatura de C#, TypeScript y tecnolog�as del stack para c�digo y documentaci�n XML.
- Mantener la coherencia visual y funcional del sistema.
- No romper el est�ndar de estructura de carpetas y archivos definido en la solucion.

---


## 5. Reglas para migraciones de base de datos

- las migraciones se haran manualmente, avisa cuando se necesite hacer una 



---

## 7. Reglas para pruebas y calidad

- Escribir y ejecutar pruebas unitarias y de integraci�n (Jasmine/Karma para Angular, pruebas de endpoints en backend).
- las pruebas deben cubrir todos los casos de uso relevantes y ser f�cilmente ejecutables.
- Ejecutar an�lisis est�tico de c�digo (ESLint/angular-eslint para frontend).
- Probar la funcionalidad tras cada cambio estructural.

---

## 8. Reglas para internacionalizaci�n y accesibilidad

- hacer las modificaciones necesarias en los .json en MiniERP_Suministros.client\public\locale\ despues de cualquier cambio que afecte el soporte de idiomas

---

## 10. Reglas para integraci�n y consumo de APIs

- Usar servicios Angular para consumir endpoints CRUD y reportes.
- Implementar interceptores para manejo de JWT y protecci�n de rutas.
- Documentar y probar la integraci�n entre backend y frontend.

---

## 11. Reglas para dise�o y experiencia de usuario

- Mantener el uso de boostrap y bootswatch que yase utiliza en los componentes y estilos existentes.



---

## 11. pruebas unitarias

- utiliza MiniERP_Suministros.client\src\app\app.component.spec.ts como modelo para hacer pruebas unitarias de componentes