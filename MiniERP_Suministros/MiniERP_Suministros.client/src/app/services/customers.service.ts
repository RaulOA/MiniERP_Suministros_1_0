/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/services/customers.service.ts
Descripción: Servicio de dominio para clientes. Orquesta endpoints y expone operaciones CRUD al widget.
*/

import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { CustomersEndpoint } from './customers-endpoint.service';
import { Customer } from '../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private endpoint = inject(CustomersEndpoint);

  getAll(): Observable<Customer[]> {
    return this.endpoint.getCustomersEndpoint<Customer[]>();
  }

  getById(id: number): Observable<Customer> {
    return this.endpoint.getCustomerEndpoint<Customer>(id);
  }

  create(payload: Partial<Customer>): Observable<Customer> {
    return this.endpoint.getCreateCustomerEndpoint<Customer>(payload);
  }

  update(id: number, payload: Partial<Customer>): Observable<Customer> {
    return this.endpoint.getUpdateCustomerEndpoint<Customer>(id, payload);
  }

  delete(id: number): Observable<Customer> {
    return this.endpoint.getDeleteCustomerEndpoint<Customer>(id);
  }
}
