/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/customers-endpoint.service.ts
Descripción: Endpoints HTTP para clientes; integra autenticación y reintentos heredando de EndpointBase.
*/

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';
import { Customer } from '../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomersEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  private get customersUrl() { return this.configurations.baseUrl + '/api/customer'; }

  getCustomersEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.customersUrl, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getCustomersEndpoint<T>()))
    );
  }

  getCustomerEndpoint<T>(id: number): Observable<T> {
    return this.http.get<T>(`${this.customersUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getCustomerEndpoint<T>(id)))
    );
  }

  getCreateCustomerEndpoint<T>(payload: Partial<Customer>): Observable<T> {
    return this.http.post<T>(this.customersUrl, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getCreateCustomerEndpoint<T>(payload)))
    );
  }

  getUpdateCustomerEndpoint<T>(id: number, payload: Partial<Customer>): Observable<T> {
    return this.http.put<T>(`${this.customersUrl}/${id}`, JSON.stringify(payload), this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getUpdateCustomerEndpoint<T>(id, payload)))
    );
  }

  getDeleteCustomerEndpoint<T>(id: number): Observable<T> {
    return this.http.delete<T>(`${this.customersUrl}/${id}`, this.requestHeaders).pipe(
      catchError(error => this.handleError(error, () => this.getDeleteCustomerEndpoint<T>(id)))
    );
  }
}
