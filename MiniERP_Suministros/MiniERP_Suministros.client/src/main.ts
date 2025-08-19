/// <reference types="@angular/localize" />

/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/main.ts
Descripción: Punto de arranque de la aplicación Angular. Inicializa el AppComponent usando la configuración global de la app y registra errores de arranque en consola.
*/

import { bootstrapApplication } from '@angular/platform-browser'; // Importa la función para arrancar la aplicación Angular en el navegador.
import { appConfig } from './app/app.config'; // Configuración global de la aplicación.
import { AppComponent } from './app/app.component'; // Componente raíz de la aplicación.

bootstrapApplication(AppComponent, appConfig) // Arranca la aplicación utilizando el componente raíz y la configuración especificada.
  .catch((err) => console.error(err)); // Registra errores de arranque en la consola.
