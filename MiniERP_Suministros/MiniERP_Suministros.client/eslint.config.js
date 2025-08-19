// RUTA: MiniERP_Suministros/MiniERP_Suministros.client/eslint.config.js
// Configuración principal de ESLint para el cliente Angular. 
// Define reglas y extensiones para mantener la calidad y consistencia del código TypeScript y las plantillas HTML.
// Incluye recomendaciones de ESLint, TypeScript y Angular, así como reglas de estilo para selectores de componentes y directivas.

/*
 * Referencias:
 *   - https://eslint.org/
 *   - https://typescript-eslint.io/
 *   - https://github.com/angular-eslint/angular-eslint
 */

const eslint = require("@eslint/js"); // Configuración base de ESLint
const tseslint = require("typescript-eslint"); // Configuración para TypeScript
const angular = require("angular-eslint"); // Configuración para Angular

module.exports = tseslint.config(
  // CONFIGURACIÓN PARA ARCHIVOS TYPESCRIPT
  {
    files: ["**/*.ts"], // Aplica a todos los archivos TypeScript
    extends: [
      eslint.configs.recommended, // Reglas recomendadas de ESLint
      ...tseslint.configs.recommended, // Reglas recomendadas de TypeScript
      ...tseslint.configs.stylistic, // Reglas de estilo para TypeScript
      ...angular.configs.tsRecommended, // Reglas recomendadas de Angular para TypeScript
    ],
    processor: angular.processInlineTemplates, // Procesa plantillas inline en componentes Angular
    rules: {
      // Reglas para los selectores de directivas Angular
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute", // Debe usarse como atributo
          prefix: "app", // Prefijo obligatorio
          style: "camelCase", // Estilo camelCase
        },
      ],
      // Reglas para los selectores de componentes Angular
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element", // Debe usarse como elemento
          prefix: "app", // Prefijo obligatorio
          style: "kebab-case", // Estilo kebab-case
        },
      ],
    },
  },
  // CONFIGURACIÓN PARA ARCHIVOS DE PLANTILLA HTML
  {
    files: ["**/*.html"], // Aplica a todos los archivos HTML de plantillas
    extends: [
      ...angular.configs.templateRecommended, // Reglas recomendadas para plantillas Angular
      ...angular.configs.templateAccessibility, // Reglas de accesibilidad para plantillas Angular
    ],
    rules: {}, // Se pueden agregar reglas adicionales específicas para plantillas aquí
  }
);
