# MiniERP_Suministros Client

Este proyecto es la interfaz de usuario (frontend) de MiniERP_Suministros, desarrollada con [Angular CLI](https://github.com/angular/angular-cli) versión 19.0.2.

## Requisitos previos

- [Node.js](https://nodejs.org/) (recomendado: versión 18 o superior)
- [Angular CLI](https://angular.dev/tools/cli) (versión 19.0.2 o superior)

## Instalación

1. Clona el repositorio y navega a la carpeta del cliente:
   ```bash
   git clone <url-del-repositorio>
   cd MiniERP_Suministros/MiniERP_Suministros.client
   ```
2. Instala las dependencias:
   ```bash
   npm install
   ```

## Servidor de desarrollo

Para iniciar el servidor de desarrollo, ejecuta:

```bash
ng serve
```

Luego abre tu navegador en `http://localhost:4200/`. La aplicación se recargará automáticamente al modificar los archivos fuente.

## Generación de código (Scaffolding)

Para generar un nuevo componente:

```bash
ng generate component nombre-componente
```

Para ver todos los esquemas disponibles (componentes, directivas, pipes, etc):

```bash
ng generate --help
```

## Construcción (Build)

Para compilar el proyecto:

```bash
ng build
```

Los artefactos de la compilación se almacenarán en la carpeta `dist/`. El build de producción optimiza la aplicación para mayor rendimiento.

## Pruebas unitarias

Para ejecutar pruebas unitarias con [Karma](https://karma-runner.github.io):

```bash
ng test
```

## Pruebas end-to-end (e2e)

Para pruebas end-to-end:

```bash
ng e2e
```

Angular CLI no incluye un framework e2e por defecto. Puedes elegir el que prefieras.

## Recursos adicionales

- [Documentación Angular CLI](https://angular.dev/tools/cli)
- [Documentación Angular](https://angular.dev/docs)

---

> **Nota:** Este frontend puede estar integrado con un backend .NET 9. Consulta la documentación del backend para detalles de integración.
