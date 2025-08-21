/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/orders.service.ts
Descripción: Servicio de dominio de pedidos; orquesta los endpoints y expone operaciones al widget.
*/

import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { OrdersEndpoint } from './orders-endpoint.service';
import { OrderCreateVM, OrderVM } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private endpoint = inject(OrdersEndpoint);

  getAll(): Observable<OrderVM[]> { return this.endpoint.getAll(); }
  getById(id: number): Observable<OrderVM> { return this.endpoint.getById(id); }
  create(payload: OrderCreateVM): Observable<OrderVM> { return this.endpoint.create(payload); }
  update(id: number, payload: Partial<OrderVM>): Observable<OrderVM> { return this.endpoint.update(id, payload); }
  delete(id: number): Observable<void> { return this.endpoint.delete(id); }
}
