/// <reference types="@angular/localize" />

/**
 * Importa la función para arrancar la aplicación Angular en el navegador.
 */
import { bootstrapApplication } from '@angular/platform-browser';

/**
 * Importa la configuración global de la aplicación.
 */
import { appConfig } from './app/app.config';

/**
 * Importa el componente raíz de la aplicación.
 */
import { AppComponent } from './app/app.component';

/**
 * Arranca la aplicación Angular utilizando el componente raíz y la configuración especificada.
 * Si ocurre un error durante el arranque, lo muestra en la consola.
 */
bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
