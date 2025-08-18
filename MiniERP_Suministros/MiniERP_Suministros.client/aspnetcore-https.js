/**
 * Este script configura HTTPS para la aplicación utilizando el certificado HTTPS de ASP.NET Core.
 * - Busca el certificado en la carpeta de usuario correspondiente.
 * - Si no existe, lo genera usando el comando 'dotnet dev-certs https'.
 *
 * @module aspnetcore-https
 */

const fs = require('fs');
const spawn = require('child_process').spawn;
const path = require('path');

/**
 * Determina la carpeta base donde se almacenan los certificados HTTPS de ASP.NET Core.
 * @type {string}
 */
const baseFolder =
  process.env.APPDATA !== undefined && process.env.APPDATA !== ''
    ? `${process.env.APPDATA}/ASP.NET/https`
    : `${process.env.HOME}/.aspnet/https`;

/**
 * Extrae el nombre del certificado desde los argumentos de línea de comandos o variables de entorno.
 * @type {string | undefined}
 */
const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : process.env.npm_package_name;

if (!certificateName) {
  console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
  process.exit(-1);
}

/**
 * Ruta al archivo del certificado (.pem).
 * @type {string}
 */
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);

/**
 * Ruta al archivo de la clave privada (.key).
 * @type {string}
 */
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Crea la carpeta base si no existe
if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

// Genera el certificado si no existe
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  spawn('dotnet', [
    'dev-certs',
    'https',
    '--export-path',
    certFilePath,
    '--format',
    'Pem',
    '--no-password',
  ], { stdio: 'inherit', })
  .on('exit', (code) => process.exit(code));
}
