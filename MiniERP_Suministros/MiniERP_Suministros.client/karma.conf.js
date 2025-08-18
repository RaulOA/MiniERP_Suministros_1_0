/**
 * Configuración de Karma para el proyecto MiniERP_Suministros.client.
 *
 * Este archivo define la configuración para ejecutar pruebas unitarias con Karma y Jasmine.
 * - Define los frameworks, plugins y reportes utilizados.
 * - Configura la cobertura de código y la salida de los reportes.
 * - Permite la ejecución automática de pruebas al detectar cambios en los archivos.
 *
 * @param {import('karma').Config} config - Objeto de configuración de Karma.
 */
module.exports = function (config) {
  config.set({
    basePath: '',
    frameworks: ['jasmine', '@angular-devkit/build-angular'],
    plugins: [
      require('karma-jasmine'),
      require('karma-chrome-launcher'),
      require('karma-jasmine-html-reporter'),
      require('karma-coverage'),
      require('@angular-devkit/build-angular/plugins/karma')
    ],
    client: {
      jasmine: {
        // Puede agregar opciones de configuración para Jasmine aquí.
        // Las opciones posibles están listadas en https://jasmine.github.io/api/edge/Configuration.html
        // Por ejemplo, puede deshabilitar la ejecución aleatoria con `random: false`
        // o establecer una semilla específica con `seed: 4321`
      },
      clearContext: false // Deja visible la salida del Jasmine Spec Runner en el navegador.
    },
    jasmineHtmlReporter: {
      suppressAll: true // Elimina los rastreos duplicados.
    },
    coverageReporter: {
      dir: require('path').join(__dirname, './coverage/'),
      subdir: '.',
      reporters: [
        { type: 'html' },
        { type: 'text-summary' }
      ]
    },
    reporters: ['progress', 'kjhtml'],
    port: 9876,
    colors: true,
    logLevel: config.LOG_INFO,
    autoWatch: true,
    browsers: ['Chrome'],
    singleRun: false,
    restartOnFileChange: true,
    listenAddress: 'localhost',
    hostname: 'localhost'
  });
};

