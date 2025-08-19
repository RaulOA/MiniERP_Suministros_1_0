/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/models/customer.model.ts
Descripción: Modelo de cliente del frontend alineado al CustomerVM del backend.
*/

export interface Customer {
  id: number;
  name: string | null;
  email: string | null;
  phoneNumber?: string | null;
  address?: string | null;
  city?: string | null;
  gender?: string | null;
}
