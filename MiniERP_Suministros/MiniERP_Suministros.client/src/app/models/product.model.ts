/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/models/product.model.ts
Descripción: Modelo de producto para la capa cliente. Alineado a ProductVM del servidor.
*/

export interface ProductVM {
  id: number;
  name: string | null;
  description?: string | null;
  icon?: string | null;
  buyingPrice: number;
  sellingPrice: number;
  unitsInStock: number;
  isActive: boolean;
  isDiscontinued: boolean;
  productCategoryId: number;
  parentId?: number | null;
  productCategoryName?: string | null;
  parentName?: string | null;
}
