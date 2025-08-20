/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/product-categories-endpoint.service.ts
Descripción: Endpoints HTTP para categorías de producto; integra autenticación y reintentos heredando de EndpointBase.
*/

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ProductCategory } from '../models/product-category.model';

@Injectable({ providedIn: 'root' })
export class ProductCategoriesEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get url() { return this.configurations.baseUrl + '/api/ProductCategory'; }

  getAll<T>(): Observable<T> {
    return this.http.get<T>(this.url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAll<T>()))
    );
  }

  getById<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getById<T>(id)))
    );
  }

  create<T>(payload: Partial<ProductCategory>): Observable<T> {
    return this.http.post<T>(this.url, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.create<T>(payload)))
    );
  }

  update<T>(id: number, payload: Partial<ProductCategory>): Observable<T> {
    return this.http.put<T>(`${this.url}/${id}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.update<T>(id, payload)))
    );
  }

  delete<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.delete<T>(id)))
    );
  }
}
