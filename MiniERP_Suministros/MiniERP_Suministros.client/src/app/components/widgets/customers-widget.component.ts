/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/customers-widget.component.ts
Descripción: Widget para visualizar, editar en línea y eliminar clientes usando ngx-datatable. Persiste datos por usuario en sesión mediante LocalStoreManager.
*/

import { Component, OnDestroy, OnInit, TemplateRef, inject, input, viewChild } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { AppTranslationService } from '../../services/app-translation.service';
import { LocalStoreManager } from '../../services/local-store-manager.service';
import { AuthService } from '../../services/auth.service';
import { Utilities } from '../../services/utilities';
import { AutofocusDirective } from '../../directives/autofocus.directive';
import { SearchBoxComponent } from '../controls/search-box.component';

interface Customer {
  $$index?: number;
  id: string;
  name: string;
  email: string;
  phone?: string;
}

@Component({
  selector: 'app-customers-widget',
  templateUrl: './customers-widget.component.html',
  styleUrl: './customers-widget.component.scss',
  imports: [SearchBoxComponent, NgxDatatableModule, FormsModule, AutofocusDirective, NgbTooltip, NgClass, TranslateModule]
})
export class CustomersWidgetComponent implements OnInit, OnDestroy {
  private alertService = inject(AlertService);
  private translationService = inject(AppTranslationService);
  private localStorage = inject(LocalStoreManager);
  private authService = inject(AuthService);

  public static readonly DBKey = 'customers-widget.customer_list';

  columns: TableColumn[] = [];
  rows: Customer[] = [];
  rowsCache: Customer[] = [];
  editing: Record<string, boolean> = {};
  isDataLoaded = false;
  loadingIndicator = true;
  private _currentUserId: string | undefined;

  readonly verticalScrollbar = input(false);

  readonly nameTemplate = viewChild.required<TemplateRef<unknown>>('nameTemplate');
  readonly emailTemplate = viewChild.required<TemplateRef<unknown>>('emailTemplate');
  readonly phoneTemplate = viewChild.required<TemplateRef<unknown>>('phoneTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  get currentUserId() {
    if (this.authService.currentUser) {
      this._currentUserId = this.authService.currentUser.id;
    }

    return this._currentUserId;
  }

  ngOnInit(): void {
    this.loadingIndicator = true;

    this.fetch(data => {
      this.refreshDataIndexes(data);
      this.rows = data;
      this.rowsCache = [...data];
      this.isDataLoaded = true;
      setTimeout(() => { this.loadingIndicator = false; }, 800);
    });

    const gT = (key: string) => this.translationService.getTranslation(key);

    this.columns = [
      { prop: 'name', name: gT('customers.management.Name') || 'Name', width: 200, cellTemplate: this.nameTemplate() },
      { prop: 'email', name: gT('customers.management.Email') || 'Email', width: 250, cellTemplate: this.emailTemplate() },
      { prop: 'phone', name: gT('customers.management.Phone') || 'Phone', width: 160, cellTemplate: this.phoneTemplate() },
      { name: '', width: 80, cellTemplate: this.actionsTemplate(), resizeable: false, canAutoResize: false, sortable: false, draggable: false }
    ];
  }

  ngOnDestroy(): void {
    this.saveToDisk();
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.name, r.email, r.phone));
  }

  updateValue(event: Event, cell: 'name' | 'email' | 'phone', row: Customer) {
    this.editing[row.$$index + '-' + cell] = false; // Finaliza edición en la celda
    (row as any)[cell] = (event.target as HTMLInputElement).value; // Actualiza el valor
    this.rows = [...this.rows]; // Trigger change detection
    this.saveToDisk(); // Persiste cambios
  }

  delete(row: Customer) {
    this.alertService.showDialog('Are you sure you want to delete the customer?', DialogType.confirm, () => this.deleteHelper(row));
  }

  private deleteHelper(row: Customer) {
    this.rowsCache = this.rowsCache.filter(item => item !== row);
    this.rows = this.rows.filter(item => item !== row);
    this.saveToDisk();
  }

  private fetch(callback: (data: Customer[]) => void) {
    let data = this.getFromDisk();

    if (data == null) {
      setTimeout(() => {
        data = this.getFromDisk();

        if (data == null) {
          data = [
            { id: 'C001', name: 'Acme Corp', email: 'info@acme.com', phone: '+1-202-555-0101' },
            { id: 'C002', name: 'Globex Inc', email: 'contact@globex.com', phone: '+1-202-555-0112' },
            { id: 'C003', name: 'Initech', email: 'support@initech.com', phone: '+1-202-555-0133' }
          ];
        }

        callback(data);
      }, 600);
    } else {
      callback(data);
    }
  }

  private refreshDataIndexes(data: Customer[]) {
    let index = 0;
    for (const i of data) {
      i.$$index = index++;
    }
  }

  private getFromDisk() {
    return this.localStorage.getDataObject<Customer[]>(`${CustomersWidgetComponent.DBKey}:${this.currentUserId}`);
  }

  private saveToDisk() {
    if (this.isDataLoaded) {
      this.localStorage.saveSyncedSessionData(this.rowsCache, `${CustomersWidgetComponent.DBKey}:${this.currentUserId}`);
    }
  }
}
