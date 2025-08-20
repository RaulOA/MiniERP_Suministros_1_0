/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/product-categories.service.ts
Descripción: Servicio de dominio para categorías de producto. Orquesta endpoints y expone operaciones CRUD al widget.
*/

import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ProductCategoriesEndpoint } from './product-categories-endpoint.service';
import { ProductCategory } from '../models/product-category.model';

@Injectable({ providedIn: 'root' })
export class ProductCategoriesService {
  private endpoint = inject(ProductCategoriesEndpoint);

  getAll(): Observable<ProductCategory[]> {
    return this.endpoint.getAll<ProductCategory[]>();
  }

  getById(id: number): Observable<ProductCategory> {
    return this.endpoint.getById<ProductCategory>(id);
  }

  create(payload: Partial<ProductCategory>): Observable<ProductCategory> {
    return this.endpoint.create<ProductCategory>(payload);
  }

  update(id: number, payload: Partial<ProductCategory>): Observable<ProductCategory> {
    return this.endpoint.update<ProductCategory>(id, payload);
  }

  delete(id: number): Observable<ProductCategory> {
    return this.endpoint.delete<ProductCategory>(id);
  }
}
