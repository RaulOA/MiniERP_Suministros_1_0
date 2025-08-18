# Tecnologías, herramientas y dependencias del cliente (Claudes_2._0.client)

Este documento describe en un solo apartado cada tecnología, herramienta, librería y dependencia utilizada en el frontend del proyecto, incluyendo su propósito, ejemplos reales de uso, contexto de manipulación por el programador, y los archivos/rutas relevantes en la solución.

---

## 1. Angular 19
- **¿Para qué sirve?**
  Framework principal para construir aplicaciones web SPA (Single Page Application) modernas, modulares y escalables.
- **Ejemplo real:**
  Estructura de componentes, servicios y rutas:
  ```typescript
  // src/app/app.component.ts
  @Component({ ... })
  export class AppComponent { ... }
  // src/app/app.routes.ts
  export const routes: Routes = [ ... ];
  ```
- **¿Cuándo lo manipula el programador?**
  - Al crear componentes, servicios, módulos y rutas.
  - Al implementar lógica de negocio y vistas.
- **Archivos y rutas relevantes:**
  - Componentes: `src/app/components/`
  - Servicios: `src/app/services/`
  - Rutas: `src/app/app.routes.ts`
  - Módulo principal: `src/app/app.module.ts`

---

## 2. Bootstrap y Bootswatch
- **¿Para qué sirve?**
  Proveen estilos y temas responsivos para la interfaz de usuario.
- **Ejemplo real:**
  Uso de clases Bootstrap en plantillas HTML:
  ```html
  <button class="btn btn-primary">Guardar</button>
  ```
- **¿Cuándo lo manipula el programador?**
  - Al diseñar la apariencia y el layout de la aplicación.
  - Al cambiar el tema visual.
- **Archivos y rutas relevantes:**
  - Temas: `src/app/assets/themes/`
  - Estilos globales: `src/styles.scss`

---

## 3. @ngx-translate/core
- **¿Para qué sirve?**
  Permite la traducción y localización de la aplicación a múltiples idiomas.
- **Ejemplo real:**
  Uso de pipe de traducción en plantillas:
  ```html
  <span>{{ 'LOGIN.TITLE' | translate }}</span>
  ```
- **¿Cuándo lo manipula el programador?**
  - Al agregar nuevos idiomas o textos traducibles.
  - Al gestionar archivos de traducción.
- **Archivos y rutas relevantes:**
  - Servicio: `src/app/services/app-translation.service.ts`
  - Archivos de idioma: `src/assets/i18n/`

---

## 4. RxJS
- **¿Para qué sirve?**
  Provee utilidades para programación reactiva y manejo de flujos de datos asíncronos.
- **Ejemplo real:**
  Suscripción a observables:
  ```typescript
  this.accountService.getUser().subscribe(user => { ... });
  ```
- **¿Cuándo lo manipula el programador?**
  - Al manejar peticiones HTTP, eventos y streams de datos.
- **Archivos y rutas relevantes:**
  - Usado en servicios y componentes: `src/app/services/`, `src/app/components/`

---

## 5. ng2-charts y chart.js
- **¿Para qué sirve?**
  Permiten crear gráficos interactivos y visualizaciones de datos.
- **Ejemplo real:**
  Mostrar un gráfico de barras:
  ```html
  <canvas baseChart [datasets]="barChartData" [labels]="barChartLabels"></canvas>
  ```
- **¿Cuándo lo manipula el programador?**
  - Al agregar o modificar visualizaciones de datos.
- **Archivos y rutas relevantes:**
  - Componentes de gráficos: `src/app/components/` (ej: dashboard)

---

## 6. @ng-bootstrap/ng-bootstrap
- **¿Para qué sirve?**
  Componentes UI avanzados y responsivos basados en Bootstrap para Angular.
- **Ejemplo real:**
  Uso de un modal:
  ```typescript
  constructor(private modalService: NgbModal) {}
  open(content) { this.modalService.open(content); }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al usar componentes como modals, tooltips, datepickers, etc.
- **Archivos y rutas relevantes:**
  - Usado en componentes: `src/app/components/`

---

## 7. @ng-select/ng-select
- **¿Para qué sirve?**
  Componente select avanzado para Angular, con búsqueda y selección múltiple.
- **Ejemplo real:**
  ```html
  <ng-select [items]="items" bindLabel="name" [(ngModel)]="selectedItem"></ng-select>
  ```
- **¿Cuándo lo manipula el programador?**
  - Al requerir selects avanzados en formularios.
- **Archivos y rutas relevantes:**
  - Usado en formularios: `src/app/components/`

---

## 8. @angular/cdk
- **¿Para qué sirve?**
  Provee utilidades y componentes de bajo nivel para construir interfaces avanzadas.
- **Ejemplo real:**
  Uso de drag-and-drop, overlays, etc.
- **¿Cuándo lo manipula el programador?**
  - Al implementar funcionalidades UI avanzadas.
- **Archivos y rutas relevantes:**
  - Usado en componentes personalizados: `src/app/components/`

---

## 9. Jasmine y Karma
- **¿Para qué sirve?**
  Frameworks para pruebas unitarias y de integración en Angular.
- **Ejemplo real:**
  Crear un test de componente:
  ```typescript
  describe('AppComponent', () => {
    it('should create the app', () => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.componentInstance;
      expect(app).toBeTruthy();
    });
  });
  ```
- **¿Cuándo lo manipula el programador?**
  - Al escribir o ejecutar pruebas automatizadas.
- **Archivos y rutas relevantes:**
  - Archivos de test: `src/app/**/*.spec.ts`
  - Configuración: `karma.conf.js`, `angular.json`

---

## 10. ESLint y angular-eslint
- **¿Para qué sirve?**
  Herramientas para análisis estático y aseguramiento de calidad del código.
- **Ejemplo real:**
  Ejecutar linting:
  ```bash
  npm run lint
  ```
- **¿Cuándo lo manipula el programador?**
  - Al mantener la calidad y consistencia del código.
- **Archivos y rutas relevantes:**
  - Configuración: `.eslintrc.json`, `angular.json`

---

**Stack principal del cliente:**
- Angular 19, Bootstrap 5, Bootswatch, RxJS, @ngx-translate/core, ng2-charts, chart.js, @ng-bootstrap/ng-bootstrap, @ng-select/ng-select, Jasmine, Karma, ESLint.

Este stack permite construir una SPA moderna, responsiva, multilenguaje, con pruebas automatizadas, visualización de datos y una experiencia de usuario avanzada.

---

La estructura típica del proyecto cliente (Claudes_2._0.client), basado en Angular y según los archivos encontrados, es la siguiente:

```
Claudes_2._0.client/
│
├── package.json                      # Configuración de dependencias NPM del proyecto Angular
├── angular.json                      # Configuración global del proyecto Angular CLI (compilación, estilos, assets, etc.)
├── tsconfig.json                     # Configuración del compilador TypeScript (rutas, niveles, exclusiones)
├── karma.conf.js                     # Configuración de pruebas unitarias con Karma
├── README.md                         # Documentación base del proyecto cliente
├── src/
│   ├── environments/
│   │   ├── environment.ts            # Configuración de entorno para desarrollo (API base, logging, etc.)
│   │   └── environment.prod.ts       # Configuración de entorno para producción
│   ├── app/
│   │   ├── app.component.ts          # Componente raíz de la aplicación Angular
│   │   ├── app.component.html        # Template HTML del componente raíz
│   │   ├── app.component.scss        # Estilos SCSS del componente raíz
│   │   ├── app.component.spec.ts     # Prueba unitaria del componente raíz
│   │   ├── app.config.ts             # Configuraciones de rutas, endpoints o constantes globales
│   │   ├── app.routes.ts             # Definición de rutas principales de la aplicación
│   │   ├── app-error.handler.ts      # Manejador global de errores en frontend
│   │   ├── models/
│   │   │   ├── Alertify.ts           # Modelo para manejar notificaciones con Alertify
│   │   │   ├── app-theme.model.ts    # Modelo de configuración de tema visual (colores, tipografías)
│   │   │   ├── enums.ts              # Enumeraciones reutilizables en toda la app
│   │   │   ├── environment.model.ts  # Modelo para representar propiedades de `environment`
│   │   │   └── login-response.model.ts # Modelo de respuesta de autenticación (tokens, usuario)
│   │   ├── services/
│   │   │   ├── account-endpoint.service.ts # Comunicación directa con la API AccountController
│   │   │   ├── account.service.ts          # Lógica general de autenticación y gestión de usuarios
│   │   │   ├── alert.service.ts            # Servicio para mostrar notificaciones (Alertify)
│   │   │   ├── animations.ts               # Configuración de animaciones globales
│   │   │   ├── app-title.service.ts        # Gestión del título de la aplicación por navegación
│   │   │   ├── app-translation.service.ts  # Servicio para manejar traducciones dinámicas
│   │   │   ├── auth-guard.ts               # Protección de rutas según autenticación y roles
│   │   │   ├── auth.service.ts             # Servicio de autenticación, manejo de tokens y roles
│   │   │   ├── configuration.service.ts    # Obtención de parámetros de configuración desde backend
│   │   │   ├── db-keys.ts                  # Claves centralizadas para almacenamiento local
│   │   │   ├── endpoint-base.service.ts    # Clase base para servicios HTTP (gestión de endpoints)
│   │   │   ├── jwt-helper.ts               # Utilidades para decodificación y validación de JWT
│   │   │   └── local-store-manager.service.ts # Servicio de almacenamiento local (localStorage/sessionStorage)
│   │   ├── directives/
│   │   │   ├── autofocus.directive.ts      # Directive para enfocar automáticamente un input
│   │   │   └── equal-validator.directive.ts # Validación personalizada para campos coincidentes
│   │   ├── pipes/
│   │   │   └── group-by.pipe.ts            # Pipe para agrupar elementos en listas por propiedad
│   │   ├── components/
│   │   │   ├── about/
│   │   │   │   ├── about.component.ts      # Lógica del componente "Acerca de"
│   │   │   │   ├── about.component.html    # Plantilla del componente "Acerca de"
│   │   │   │   └── about.component.scss    # Estilos del componente "Acerca de"
│   │   │   ├── controls/
│   │   │   │   ├── banner-demo.component.ts   # Componente demostrativo de banners
│   │   │   │   ├── banner-demo.component.html # Template de banners
│   │   │   │   └── banner-demo.component.scss # Estilos de banner-demo
│   │   │   ├── customers/
│   │   │   │   ├── customers.component.ts  # Componente para gestión y visualización de clientes
│   │   │   │   ├── customers.component.html# Vista HTML de clientes
│   │   │   │   └── customers.component.scss# Estilos del componente clientes
│   │   │   ├── home/
│   │   │   │   ├── home.component.ts       # Lógica de la vista principal de bienvenida
│   │   │   │   ├── home.component.html     # Plantilla HTML de la pantalla de inicio
│   │   │   │   └── home.component.scss     # Estilos visuales del home
│   │   │   └── ... (otros componentes)     # Otros módulos de pantalla: login, usuario, configuración, etc.
│   ├── assets/
│   │   ├── styles/
│   │   │   ├── alertify.bootstrap.css      # Tema visual de Alertify con estilo Bootstrap
│   │   │   ├── alertify.core.css           # Estilos base de Alertify.js
│   │   │   └── alertify.default.css        # Tema predeterminado de Alertify
│   │   ├── scripts/
│   │   │   └── alertify.js                 # Librería JS para notificaciones emergentes
│   │   ├── themes/
│   │   │   ├── cerulean.scss               # Tema visual cerulean
│   │   │   ├── cosmo.scss                  # Tema visual cosmo
│   │   │   ├── flatly.scss                 # Tema visual flatly
│   │   │   ├── journal.scss                # Tema visual journal
│   │   │   └── ... (otros temas)           # Otros archivos SCSS de temas visuales
│   │   └── i18n/
│   │       └── ... (archivos de traducción)# Archivos JSON para soporte de múltiples idiomas
│   ├── styles.scss                         # Archivo de estilos globales para la app Angular
│   └── ... (otros archivos de configuración)# Archivos adicionales: favicon, index.html, polyfills, etc.
```

**Notas:**
- La carpeta `src/app/components/` contiene los componentes de la aplicación.
- La carpeta `src/app/services/` contiene los servicios de negocio y utilidades.
- La carpeta `src/app/models/` contiene los modelos de datos.
- La carpeta `src/assets/` contiene recursos estáticos como estilos, scripts, temas e internacionalización.
- Los archivos de configuración principales (`angular.json`, `tsconfig.json`, etc.) están en la raíz del proyecto.