/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/products-endpoint.service.ts
Descripción: Endpoints HTTP para productos; integra autenticación y reintentos heredando de EndpointBase.
*/

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { ProductVM } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductsEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get url() { return this.configurations.baseUrl + '/api/Products'; }

  getAll(): Observable<ProductVM[]> {
    return this.http.get<ProductVM[]>(this.url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAll()))
    );
  }

  getById(id: number): Observable<ProductVM> {
    return this.http.get<ProductVM>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getById(id)))
    );
  }

  create(payload: Partial<ProductVM>): Observable<ProductVM> {
    return this.http.post<ProductVM>(this.url, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.create(payload)))
    );
  }

  update(id: number, payload: Partial<ProductVM>): Observable<ProductVM> {
    return this.http.put<ProductVM>(`${this.url}/${id}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.update(id, payload)))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.delete(id)))
    );
  }
}
