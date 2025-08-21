/**
 * Inicia Angular dev-server en HTTPS usando el certificado de ASP.NET Core.
 * Evita [DEP0190] eliminando la dependencia de "run-script-os" y sin usar shell=true.
 * Lanza @angular/cli local de forma robusta con require.resolve.
 */
const path = require('path');
const os = require('os');
const { spawn } = require('child_process');

// Determinar carpeta de certificados
const baseFolder = process.env.APPDATA && process.env.APPDATA !== ''
  ? path.join(process.env.APPDATA, 'ASP.NET', 'https')
  : path.join(os.homedir(), '.aspnet', 'https');

// Nombre del certificado = nombre del paquete npm
const certificateName = process.env.npm_package_name;
if (!certificateName) {
  console.error('No se pudo determinar npm_package_name para localizar el certificado.');
  process.exit(1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Argumentos para ng serve
const args = [
  'serve',
  '--ssl',
  '--ssl-cert', certFilePath,
  '--ssl-key', keyFilePath,
  '--host=localhost', // Alineado con SpaProxyServerUrl (https://localhost:4200)
  '--port=4200'
];

// Resolver el binario de @angular/cli y ejecutarlo con el runtime de Node actual
let ngEntry;
try {
  ngEntry = require.resolve('@angular/cli/bin/ng');
} catch (e) {
  console.error('No se encontró @angular/cli local. Ejecuta: npm i --save-dev @angular/cli');
  process.exit(1);
}

const child = spawn(process.execPath, [ngEntry, ...args], { stdio: 'inherit', shell: false });
child.on('exit', code => process.exit(code ?? 0));
