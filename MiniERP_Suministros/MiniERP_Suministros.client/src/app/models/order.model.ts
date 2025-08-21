/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/models/order.model.ts
Descripción: Modelos de pedido alineados a OrderVM de la API.
*/

export interface OrderItemVM {
  productId: number;
  productName?: string | null;
  quantity: number;
  unitPrice: number;
  discount: number;
  lineTotal?: number;
}

export interface OrderVM {
  id: number;
  discount: number;
  comments?: string | null;
  customerId: number;
  customerName?: string | null;
  cashierId?: string | null;
  cashierName?: string | null;
  createdDate: string;
  items?: OrderItemVM[];
  subtotal?: number;
  itemsDiscount?: number;
  total?: number;
}

export interface OrderCreateItemVM {
  productId: number;
  quantity: number;
  unitPrice?: number;
  discount?: number;
}

export interface OrderCreateVM {
  customerId: number;
  discount: number;
  comments?: string | null;
  items: OrderCreateItemVM[];
}
