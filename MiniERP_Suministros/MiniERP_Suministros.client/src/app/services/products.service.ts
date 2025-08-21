/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/products.service.ts
Descripción: Servicio de dominio para productos. Orquesta endpoints y expone operaciones CRUD al widget.
*/

import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ProductsEndpoint } from './products-endpoint.service';
import { ProductVM } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private endpoint = inject(ProductsEndpoint);

  getAll(): Observable<ProductVM[]> { return this.endpoint.getAll(); }
  getById(id: number): Observable<ProductVM> { return this.endpoint.getById(id); }
  create(payload: Partial<ProductVM>): Observable<ProductVM> { return this.endpoint.create(payload); }
  update(id: number, payload: Partial<ProductVM>): Observable<ProductVM> { return this.endpoint.update(id, payload); }
  delete(id: number): Observable<void> { return this.endpoint.delete(id); }
}
