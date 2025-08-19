/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/customers-widget.component.ts
Descripción: Widget para visualizar, crear, editar en línea y eliminar clientes usando ngx-datatable. Consume API backend de clientes y maneja i18n.
*/

import { Component, OnDestroy, OnInit, TemplateRef, input, viewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { AppTranslationService } from '../../services/app-translation.service';
import { Utilities } from '../../services/utilities';
import { AutofocusDirective } from '../../directives/autofocus.directive';
import { SearchBoxComponent } from '../controls/search-box.component';
import { CustomersService } from '../../services/customers.service';
import { Customer } from '../../models/customer.model';

@Component({
  standalone: true,
  selector: 'app-customers-widget',
  templateUrl: './customers-widget.component.html',
  styleUrl: './customers-widget.component.scss',
  imports: [SearchBoxComponent, NgxDatatableModule, AutofocusDirective, NgbTooltip, TranslateModule]
})
export class CustomersWidgetComponent implements OnInit, OnDestroy {
  columns: TableColumn[] = [];
  rows: (Customer & { $$index?: number })[] = [];
  rowsCache: (Customer & { $$index?: number })[] = [];
  editing: Record<string, boolean> = {};
  loadingIndicator = true;

  // Creación de nuevo cliente
  addingNew = false; // muestra/oculta formulario de alta
  savingNew = false; // estado de guardado
  newCustomer: Partial<Customer> = { name: '', email: '', phoneNumber: '' };

  readonly verticalScrollbar = input(false);

  readonly nameTemplate = viewChild.required<TemplateRef<unknown>>('nameTemplate');
  readonly emailTemplate = viewChild.required<TemplateRef<unknown>>('emailTemplate');
  readonly phoneTemplate = viewChild.required<TemplateRef<unknown>>('phoneTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  constructor(
    private customersService: CustomersService,
    private alertService: AlertService,
    private translationService: AppTranslationService
  ) {}

  ngOnInit(): void {
    this.loadingIndicator = true;

    this.loadCustomers();

    const gT = (key: string) => this.translationService.getTranslation(key);

    this.columns = [
      { prop: 'name', name: gT('customersWidget.table.Name') || 'Name', width: 200, sortable: true, cellTemplate: this.nameTemplate() },
      { prop: 'email', name: gT('customersWidget.table.Email') || 'Email', width: 250, sortable: true, cellTemplate: this.emailTemplate() },
      { prop: 'phoneNumber', name: gT('customersWidget.table.Phone') || 'Phone', width: 160, sortable: false, cellTemplate: this.phoneTemplate() },
      { name: '', width: 80, cellTemplate: this.actionsTemplate(), resizeable: false, canAutoResize: false, sortable: false, draggable: false }
    ];
  }

  // Sustituimos el método vacío para cumplir con eslint
  ngOnDestroy(): void { this.editing = {}; }

  private loadCustomers() {
    this.customersService.getAll().subscribe({
      next: (data) => {
        this.rows = data.map((c, idx) => ({ ...c, $$index: idx }));
        this.rowsCache = [...this.rows];
        setTimeout(() => { this.loadingIndicator = false; }, 300);
      },
      error: (err) => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          this.translationService.getTranslation('app.alerts.LoadingError'),
          err?.message ?? 'Error loading customers',
          MessageSeverity.error
        );
      }
    });
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.name || '', r.email || '', r.phoneNumber || ''));
  }

  // ===================== Alta de nuevo cliente =====================
  startAddNew() {
    this.addingNew = true;
    this.savingNew = false;
    this.newCustomer = { name: '', email: '', phoneNumber: '' };
  }

  cancelAdd() {
    this.addingNew = false;
    this.savingNew = false;
    this.newCustomer = { name: '', email: '', phoneNumber: '' };
  }

  onNewInputChange(event: Event, field: 'name' | 'email' | 'phoneNumber') {
    const value = (event.target as HTMLInputElement).value;
    (this.newCustomer as Record<string, unknown>)[field] = value;
  }

  private validateNew(): string | null {
    const name = (this.newCustomer.name || '').trim();
    const email = (this.newCustomer.email || '').trim();

    if (!name) {
      return this.translationService.getTranslation('customersWidget.editor.NameRequired') || 'Name is required';
    }

    if (!email) {
      return this.translationService.getTranslation('customersWidget.editor.EmailRequired') || 'Email is required';
    }

    const emailRegex = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    if (!emailRegex.test(email)) {
      return this.translationService.getTranslation('customersWidget.editor.InvalidEmail') || 'Invalid email';
    }

    return null;
  }

  create() {
    const validationError = this.validateNew();
    if (validationError) {
      this.alertService.showMessage(validationError, '', MessageSeverity.warn);
      return;
    }

    this.savingNew = true;
    const payload: Partial<Customer> = {
      name: (this.newCustomer.name || '').trim(),
      email: (this.newCustomer.email || '').trim(),
      phoneNumber: (this.newCustomer.phoneNumber ?? '')?.toString().trim() || null
    };

    this.customersService.create(payload).subscribe({
      next: () => {
        // Recargamos para mantener consistencia e índices
        this.loadCustomers();
        this.addingNew = false;
        this.savingNew = false;
        this.newCustomer = { name: '', email: '', phoneNumber: '' };
      },
      error: (err) => {
        this.savingNew = false;
        this.alertService.showMessage('Error', err?.message ?? 'Error creating customer', MessageSeverity.error);
      }
    });
  }
  // ================================================================

  updateValue(event: Event, cell: 'name' | 'email' | 'phoneNumber', row: Customer & { $$index?: number }) {
    const obj = row as unknown as Record<string, unknown>;
    const previous = obj[cell] as string | null | undefined;
    const newValue = (event.target as HTMLInputElement).value;

    this.editing[(row.$$index ?? 0) + '-' + cell] = false;
    obj[cell] = newValue;
    this.rows = [...this.rows];

    this.customersService.update(row.id, { [cell]: newValue }).subscribe({
      next: (updated) => {
        const idx = this.rowsCache.findIndex(r => r.id === row.id);
        if (idx > -1) {
          this.rowsCache[idx] = { ...this.rowsCache[idx], ...updated };
        }
      },
      error: (err) => {
        obj[cell] = previous ?? null;
        this.rows = [...this.rows];
        this.alertService.showMessage('Error', err?.message ?? 'Error updating customer', MessageSeverity.error);
      }
    });
  }

  delete(row: Customer) {
    const confirmText = this.translationService.getTranslation('customersWidget.dialog.DeleteConfirm');
    this.alertService.showDialog(confirmText, DialogType.confirm, () => this.deleteHelper(row));
  }

  private deleteHelper(row: Customer) {
    this.customersService.delete(row.id).subscribe({
      next: () => {
        this.rowsCache = this.rowsCache.filter(item => item !== row);
        this.rows = this.rows.filter(item => item !== row);
      },
      error: (err) => {
        this.alertService.showMessage('Error', err?.message ?? 'Error deleting customer', MessageSeverity.error);
      }
    });
  }
}
