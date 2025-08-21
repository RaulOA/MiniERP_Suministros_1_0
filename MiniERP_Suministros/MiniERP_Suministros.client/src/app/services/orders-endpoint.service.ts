/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/orders-endpoint.service.ts
Descripción: Endpoints HTTP para pedidos.
*/

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { OrderCreateVM, OrderVM } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrdersEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get url() { return this.configurations.baseUrl + '/api/Orders'; }

  getAll(): Observable<OrderVM[]> {
    return this.http.get<OrderVM[]>(this.url, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getAll()))
    );
  }

  getById(id: number): Observable<OrderVM> {
    return this.http.get<OrderVM>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getById(id)))
    );
  }

  create(payload: OrderCreateVM): Observable<OrderVM> {
    return this.http.post<OrderVM>(this.url, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.create(payload)))
    );
  }

  update(id: number, payload: Partial<OrderVM>): Observable<OrderVM> {
    return this.http.put<OrderVM>(`${this.url}/${id}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.update(id, payload)))
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.delete(id)))
    );
  }
}
