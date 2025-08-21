/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/orders-widget.component.ts
Descripción: Widget para listar y editar/crear pedidos con encabezado + líneas. Usa [(ngModel)] para enlazar selects del editor y preseleccionar Cliente/Productos al editar.
*/
import { Component, OnInit, OnDestroy, TemplateRef, viewChild, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // [(ngModel)]
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import { TranslateModule } from '@ngx-translate/core';

import { OrdersService } from '../../services/orders.service';
import { OrderCreateItemVM, OrderCreateVM, OrderVM } from '../../models/order.model';
import { SearchBoxComponent } from '../controls/search-box.component';
import { AppTranslationService } from '../../services/app-translation.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { CustomersService } from '../../services/customers.service';
import { Customer } from '../../models/customer.model';
import { ProductsService } from '../../services/products.service';
import { ProductVM } from '../../models/product.model';

@Component({
  standalone: true,
  selector: 'app-orders-widget',
  templateUrl: './orders-widget.component.html',
  styleUrl: './orders-widget.component.scss',
  imports: [CommonModule, FormsModule, NgxDatatableModule, TranslateModule, SearchBoxComponent]
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
  editingId: number | null = null; // null = nuevo, number = editar existente

  customers: Customer[] = [];
  products: ProductVM[] = [];

  readonly verticalScrollbar = input(false);

  readonly customerTemplate = viewChild.required<TemplateRef<unknown>>('customerTemplate');
  readonly dateTemplate = viewChild.required<TemplateRef<unknown>>('dateTemplate');
  readonly itemsTemplate = viewChild.required<TemplateRef<unknown>>('itemsTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  constructor(private service: OrdersService,
              private customersService: CustomersService,
              private productsService: ProductsService,
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
    this.loadCustomers();
    this.loadProducts();
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

  private loadCustomers() {
    this.customersService.getAll().subscribe({
      next: data => this.customers = data,
      error: err => this.alertService.showStickyMessage('Error', err?.message || 'Error loading customers', MessageSeverity.error)
    });
  }

  private loadProducts() {
    this.productsService.getAll().subscribe({
      next: data => this.products = data,
      error: err => this.alertService.showStickyMessage('Error', err?.message || 'Error loading products', MessageSeverity.error)
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
    this.editingId = null;
  }

  edit(row: OrderVM) {
    this.editing = true;
    this.editingTitle = this.translation.getTranslation('ordersWidget.editor.Edit') || 'Edit order';
    this.saving = false;
    this.editingId = row.id;
    this.editModel = { customerId: row.customerId, discount: row.discount, comments: row.comments || '', items: [] };
    this.editItems = (row.items || []).map(i => ({ productId: i.productId, quantity: i.quantity, unitPrice: i.unitPrice, discount: 0 }));
  }

  cancelEdit() { this.editing = false; }

  onHeaderChange(value: any, field: 'customerId' | 'discount' | 'comments') {
    if (field === 'customerId') this.editModel.customerId = Number(value);
    if (field === 'discount') this.editModel.discount = Number(value);
    if (field === 'comments') this.editModel.comments = String(value ?? '');
  }

  addItem() { this.editItems.push({ productId: 0, quantity: 1, unitPrice: 0, discount: 0 }); }
  removeItem(index: number) { this.editItems.splice(index, 1); }

  onProductChange(index: number, value: any) {
    const productId = Number(value);
    const p = this.products.find(x => x.id === productId);
    const item = this.editItems[index];
    item.productId = productId;
    item.unitPrice = p ? p.sellingPrice : 0; // precio del catálogo
  }

  onItemChange(index: number, field: Exclude<keyof OrderCreateItemVM, 'productId' | 'unitPrice'>, event: Event) {
    const valueRaw = (event.target as HTMLInputElement).value;
    const num = Number(valueRaw);
    const item = this.editItems[index];
    if (field === 'quantity') item.quantity = num > 0 ? num : 1;
  }

  getLineTotal(i: OrderCreateItemVM) { const u = i.unitPrice || 0; return u * (i.quantity || 0); }
  getSubtotal() { return this.editItems.reduce((s, i) => s + (i.unitPrice || 0) * (i.quantity || 0), 0); }
  getTotal() { return this.getSubtotal() - (this.editModel.discount || 0); }

  private buildPayload(): OrderCreateVM {
    return {
      customerId: this.editModel.customerId,
      discount: this.editModel.discount || 0,
      comments: this.editModel.comments || '',
      items: this.editItems.map(i => ({ productId: i.productId, quantity: i.quantity, unitPrice: i.unitPrice, discount: 0 }))
    };
  }

  saveEdit() {
    // Validación mínima
    if (!this.editModel.customerId || this.editItems.length === 0) {
      this.alertService.showMessage(this.translation.getTranslation('ordersWidget.editor.Validation') || 'Invalid data', '', MessageSeverity.warn);
      return;
    }

    // Validar cantidades > 0
    if (this.editItems.some(i => (i.quantity || 0) <= 0)) {
      this.alertService.showMessage(this.translation.getTranslation('ordersWidget.editor.Validation') || 'Invalid data', 'Quantity must be > 0', MessageSeverity.warn);
      return;
    }

    this.saving = true;
    if (this.editingId == null) {
      const payload = this.buildPayload();
      this.service.create(payload).subscribe({
        next: () => { this.saving = false; this.editing = false; this.load(); },
        error: err => { this.saving = false; this.alertService.showStickyMessage('Error', err?.error?.message || err?.message || 'Error saving order', MessageSeverity.error); }
      });
    } else {
      // Update parcial; se envían cabecera + líneas (server debe soportarlo)
      const payload = this.buildPayload() as unknown as Partial<OrderVM>;
      this.service.update(this.editingId, payload).subscribe({
        next: () => { this.saving = false; this.editing = false; this.load(); },
        error: err => { this.saving = false; this.alertService.showStickyMessage('Error', err?.error?.message || err?.message || 'Error updating order', MessageSeverity.error); }
      });
    }
  }

  remove(row: OrderVM) {
    this.service.delete(row.id).subscribe({
      next: () => { this.rowsCache = this.rowsCache.filter(r => r.id !== row.id); this.rows = this.rows.filter(r => r.id !== row.id); },
      error: err => this.alertService.showStickyMessage('Error', err?.message || 'Error deleting order', MessageSeverity.error)
    });
  }
}
