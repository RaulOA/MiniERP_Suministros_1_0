// Ruta: MiniERP_Suministros\MiniERP_Suministros.client\src\proxy.conf.js
// Configuración del proxy para redirigir solicitudes de desarrollo Angular a la API .NET local.

/**
 * @file Configuración de proxy para el cliente Angular.
 * @description Redirige rutas específicas (API, Swagger, autenticación) al backend .NET durante el desarrollo.
 * Utiliza variables de entorno para determinar el puerto o URL de destino.
 */

const { env } = require('process');

/**
 * Determina la URL de destino para el proxy.
 * - Si existe ASPNETCORE_HTTPS_PORT, usa https://localhost:<puerto>.
 * - Si existe ASPNETCORE_URLS, usa la primera URL definida.
 * - Por defecto, usa https://localhost:7085.
 * @type {string}
 */
const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7085';

/**
 * Configuración de rutas a ser proxied hacia el backend.
 * @type {import('http-proxy-middleware').Options[]}
 */
const PROXY_CONFIG = [
  {
    /**
     * Lista de rutas que serán redirigidas al backend.
     * @type {string[]}
     */
    context: [
      "/api",
      "/swagger",
      "/connect",
      "/oauth",
      "/.well-known"
    ],
    /**
     * URL de destino para las rutas definidas.
     */
    target,
    /**
     * Permite conexiones inseguras (útil para certificados autofirmados en desarrollo).
     */
    secure: false
  }
]

/**
 * Exporta la configuración del proxy para ser utilizada por Angular CLI.
 */
module.exports = PROXY_CONFIG;
