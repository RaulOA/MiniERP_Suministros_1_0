/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/customers-widget.component.ts
Descripción: Widget para visualizar, editar en línea y eliminar clientes usando ngx-datatable. Consume API backend de clientes y maneja i18n.
*/

import { Component, OnDestroy, OnInit, TemplateRef, input, viewChild } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';

import { AlertService, DialogType } from '../../services/alert.service';
import { AppTranslationService } from '../../services/app-translation.service';
import { Utilities } from '../../services/utilities';
import { AutofocusDirective } from '../../directives/autofocus.directive';
import { SearchBoxComponent } from '../controls/search-box.component';
import { CustomersService } from '../../services/customers.service';
import { Customer } from '../../models/customer.model';

@Component({
  selector: 'app-customers-widget',
  templateUrl: './customers-widget.component.html',
  styleUrl: './customers-widget.component.scss',
  imports: [SearchBoxComponent, NgxDatatableModule, AutofocusDirective, NgbTooltip, TranslateModule]
})
export class CustomersWidgetComponent implements OnInit, OnDestroy {
  columns: TableColumn[] = [];
  rows: Customer[] = [];
  rowsCache: Customer[] = [];
  editing: Record<string, boolean> = {};
  loadingIndicator = true;

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

  ngOnDestroy(): void {
    // No-op
  }

  private loadCustomers() {
    this.customersService.getAll().subscribe({
      next: (data) => {
        // Asegura índices para la edición inline
        this.rows = data.map((c, idx) => ({ ...c, $$index: idx }));
        this.rowsCache = [...this.rows];
        setTimeout(() => { this.loadingIndicator = false; }, 300);
      },
      error: (err) => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          this.translationService.getTranslation('app.alerts.LoadingError'),
          err?.message ?? 'Error loading customers',
          this.alertService.getDialogType(AlertService) as any // fallback; showStickyMessage signature compatible
        );
      }
    });
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.name || '', r.email || '', r.phoneNumber || ''));
  }

  updateValue(event: Event, cell: 'name' | 'email' | 'phoneNumber', row: Customer & { $$index?: number }) {
    const previous = (row as any)[cell];
    const newValue = (event.target as HTMLInputElement).value;

    this.editing[row.$$index + '-' + cell] = false;
    (row as any)[cell] = newValue;
    this.rows = [...this.rows];

    this.customersService.update(row.id, { [cell]: newValue }).subscribe({
      next: (updated) => {
        // Sincroniza rowsCache
        const idx = this.rowsCache.findIndex(r => r.id === row.id);
        if (idx > -1) {
          this.rowsCache[idx] = { ...this.rowsCache[idx], ...updated };
        }
      },
      error: (err) => {
        // Revertir valor y notificar
        (row as any)[cell] = previous;
        this.rows = [...this.rows];
        this.alertService.showMessage('Error', err?.message ?? 'Error updating customer', 3);
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
        this.alertService.showMessage('Error', err?.message ?? 'Error deleting customer', 3);
      }
    });
  }
}
