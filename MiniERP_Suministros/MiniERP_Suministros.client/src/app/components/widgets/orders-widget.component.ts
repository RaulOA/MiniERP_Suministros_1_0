/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/orders-widget.component.ts
Descripción: Widget para listar y editar/crear pedidos con encabezado + líneas.
*/

import { Component, OnInit, OnDestroy, TemplateRef, viewChild, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import { TranslateModule } from '@ngx-translate/core';

import { OrdersService } from '../../services/orders.service';
import { OrderCreateItemVM, OrderCreateVM, OrderItemVM, OrderVM } from '../../models/order.model';
import { SearchBoxComponent } from '../controls/search-box.component';
import { AppTranslationService } from '../../services/app-translation.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

@Component({
  standalone: true,
  selector: 'app-orders-widget',
  templateUrl: './orders-widget.component.html',
  styleUrl: './orders-widget.component.scss',
  imports: [CommonModule, NgxDatatableModule, TranslateModule, SearchBoxComponent]
})
export class OrdersWidgetComponent implements OnInit, OnDestroy {
  rows: OrderVM[] = [];
  rowsCache: OrderVM[] = [];
  columns: TableColumn[] = [];
  loadingIndicator = true;

  editing = false;
  editingTitle = '';
  saving = false;
  editModel: OrderCreateVM = { customerId: 0, discount: 0, comments: '', items: [] };
  editItems: OrderCreateItemVM[] = [];

  readonly verticalScrollbar = input(false);

  readonly customerTemplate = viewChild.required<TemplateRef<unknown>>('customerTemplate');
  readonly dateTemplate = viewChild.required<TemplateRef<unknown>>('dateTemplate');
  readonly itemsTemplate = viewChild.required<TemplateRef<unknown>>('itemsTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  constructor(private service: OrdersService,
              private translation: AppTranslationService,
              private alertService: AlertService) {}

  ngOnInit(): void {
    const gT = (key: string) => this.translation.getTranslation(key);
    this.columns = [
      { prop: 'id', name: 'Id', width: 90 },
      { prop: 'createdDate', name: gT('ordersWidget.table.Date') || 'Date', width: 180, cellTemplate: this.dateTemplate() },
      { prop: 'customerId', name: gT('ordersWidget.table.Customer'), width: 250, cellTemplate: this.customerTemplate() },
      { name: gT('ordersWidget.table.Items'), width: 100, cellTemplate: this.itemsTemplate() },
      { prop: 'total', name: gT('ordersWidget.table.Total'), width: 140 },
      { name: '', width: 100, cellTemplate: this.actionsTemplate(), resizeable: false, canAutoResize: false, sortable: false, draggable: false }
    ];

    this.load();
  }

  ngOnDestroy(): void {}

  private load() {
    this.loadingIndicator = true;
    this.service.getAll().subscribe({
      next: data => {
        this.rows = data;
        this.rowsCache = [...data];
        setTimeout(() => this.loadingIndicator = false, 200);
      },
      error: err => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Error', err?.message || 'Error loading orders', MessageSeverity.error);
      }
    });
  }

  onSearchChanged(value: string) {
    const v = (value || '').toLowerCase();
    this.rows = this.rowsCache.filter(r =>
      (r.customerName || '').toLowerCase().includes(v) ||
      (r.comments || '').toLowerCase().includes(v) ||
      (r.id + '').includes(v));
  }

  startNew() {
    this.editing = true;
    this.editingTitle = this.translation.getTranslation('ordersWidget.editor.New') || 'New order';
    this.saving = false;
    this.editModel = { customerId: 0, discount: 0, comments: '', items: [] };
    this.editItems = [];
  }

  edit(row: OrderVM) {
    this.editing = true;
    this.editingTitle = this.translation.getTranslation('ordersWidget.editor.Edit') || 'Edit order';
    this.saving = false;
    this.editModel = { customerId: row.customerId, discount: row.discount, comments: row.comments || '', items: [] };
    this.editItems = (row.items || []).map(i => ({ productId: i.productId, quantity: i.quantity, unitPrice: i.unitPrice, discount: i.discount }));
  }

  cancelEdit() { this.editing = false; }

  onHeaderChange(event: Event, field: 'customerId' | 'discount' | 'comments') {
    const valueRaw = (event.target as HTMLInputElement).value;
    if (field === 'customerId') this.editModel.customerId = Number(valueRaw);
    if (field === 'discount') this.editModel.discount = Number(valueRaw);
    if (field === 'comments') this.editModel.comments = valueRaw;
  }

  addItem() { this.editItems.push({ productId: 0, quantity: 1, unitPrice: 0, discount: 0 }); }
  removeItem(index: number) { this.editItems.splice(index, 1); }

  onItemChange(index: number, field: keyof OrderCreateItemVM, event: Event) {
    const valueRaw = (event.target as HTMLInputElement).value;
    const num = Number(valueRaw);
    const item = this.editItems[index];
    if (field === 'productId') item.productId = num;
    if (field === 'quantity') item.quantity = num;
    if (field === 'unitPrice') item.unitPrice = num;
    if (field === 'discount') item.discount = num;
  }

  getLineTotal(i: OrderCreateItemVM) { const u = i.unitPrice || 0; const d = i.discount || 0; return (u - d) * (i.quantity || 0); }
  getSubtotal() { return this.editItems.reduce((s, i) => s + (i.unitPrice || 0) * (i.quantity || 0), 0); }
  getItemsDiscount() { return this.editItems.reduce((s, i) => s + (i.discount || 0) * (i.quantity || 0), 0); }
  getTotal() { return this.getSubtotal() - this.getItemsDiscount() - (this.editModel.discount || 0); }

  saveEdit() {
    // Validación mínima
    if (!this.editModel.customerId || this.editItems.length === 0) {
      this.alertService.showMessage(this.translation.getTranslation('ordersWidget.editor.Validation') || 'Invalid data', '', MessageSeverity.warn);
      return;
    }

    this.saving = true;
    const payload: OrderCreateVM = {
      customerId: this.editModel.customerId,
      discount: this.editModel.discount || 0,
      comments: this.editModel.comments || '',
      items: this.editItems.map(i => ({ productId: i.productId, quantity: i.quantity, unitPrice: i.unitPrice, discount: i.discount }))
    };

    this.service.create(payload).subscribe({
      next: () => { this.saving = false; this.editing = false; this.load(); },
      error: err => { this.saving = false; this.alertService.showStickyMessage('Error', err?.message || 'Error saving order', MessageSeverity.error); }
    });
  }

  remove(row: OrderVM) {
    this.service.delete(row.id).subscribe({
      next: () => { this.rowsCache = this.rowsCache.filter(r => r.id !== row.id); this.rows = this.rows.filter(r => r.id !== row.id); },
      error: err => this.alertService.showStickyMessage('Error', err?.message || 'Error deleting order', MessageSeverity.error)
    });
  }
}
