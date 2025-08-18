// @ts-check

/**
 * Configuración de ESLint para el proyecto MiniERP_Suministros.client.
 *
 * Esta configuración aplica reglas recomendadas para JavaScript, TypeScript y Angular,
 * incluyendo estilos y buenas prácticas para archivos .ts y plantillas .html.
 *
 * - Para archivos TypeScript (*.ts):
 *   - Extiende las configuraciones recomendadas de ESLint, TypeScript y Angular.
 *   - Aplica reglas de estilo y convenciones para selectores de directivas y componentes Angular.
 *   - Usa el procesador para plantillas inline de Angular.
 *
 * - Para archivos de plantilla (*.html):
 *   - Extiende las configuraciones recomendadas de Angular para plantillas y accesibilidad.
 *
 * @see https://eslint.org/
 * @see https://typescript-eslint.io/
 * @see https://github.com/angular-eslint/angular-eslint
 */

const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");

module.exports = tseslint.config(
  {
    files: ["**/*.ts"],
    extends: [
      eslint.configs.recommended,
      ...tseslint.configs.recommended,
      ...tseslint.configs.stylistic,
      ...angular.configs.tsRecommended,
    ],
    processor: angular.processInlineTemplates,
    rules: {
      /**
       * Enforce directive selectors to be camelCase and prefixed with 'app'.
       */
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      /**
       * Enforce component selectors to be kebab-case and prefixed with 'app'.
       */
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],
    },
  },
  {
    files: ["**/*.html"],
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
    ],
    rules: {},
  }
);
