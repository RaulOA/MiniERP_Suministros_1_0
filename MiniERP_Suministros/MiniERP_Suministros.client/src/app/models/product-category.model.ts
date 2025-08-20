/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/models/product-category.model.ts
Descripción: Modelo de categoría de producto alineado a ProductCategoryVM del backend.
*/

export interface ProductCategory {
  id: number;
  name: string | null;
  description?: string | null;
  icon?: string | null;
}
