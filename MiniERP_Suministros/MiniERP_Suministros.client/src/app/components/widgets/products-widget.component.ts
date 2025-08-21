/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/products-widget.component.ts
Descripción: Widget para visualizar, crear, editar en línea y eliminar productos usando ngx-datatable. Controla permisos por rol (solo administrador puede mutar).
*/

import { Component, OnDestroy, OnInit, TemplateRef, input, viewChild, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';

import { SearchBoxComponent } from '../controls/search-box.component';
import { AutofocusDirective } from '../../directives/autofocus.directive';
import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { AppTranslationService } from '../../services/app-translation.service';
import { Utilities } from '../../services/utilities';
import { ProductsService } from '../../services/products.service';
import { ProductVM } from '../../models/product.model';
import { AccountService } from '../../services/account.service';
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductCategory } from '../../models/product-category.model';

@Component({
  standalone: true,
  selector: 'app-products-widget',
  templateUrl: './products-widget.component.html',
  styleUrl: './products-widget.component.scss',
  imports: [CommonModule, TranslateModule, NgxDatatableModule, SearchBoxComponent, AutofocusDirective]
})
export class ProductsWidgetComponent implements OnInit, OnDestroy {
  columns: TableColumn[] = [];
  rows: (ProductVM & { $$index?: number })[] = [];
  rowsCache: (ProductVM & { $$index?: number })[] = [];
  editing: Record<string, boolean> = {};
  loadingIndicator = true;

  addingNew = false;
  savingNew = false;
  newProduct: Partial<ProductVM> = { name: '', description: '', icon: '', buyingPrice: 0, sellingPrice: 0, unitsInStock: 0, isActive: true, isDiscontinued: false, productCategoryId: 0, parentId: null };

  categories: ProductCategory[] = [];

  readonly verticalScrollbar = input(false);

  readonly nameTemplate = viewChild.required<TemplateRef<unknown>>('nameTemplate');
  readonly categoryTemplate = viewChild.required<TemplateRef<unknown>>('categoryTemplate');
  readonly priceTemplate = viewChild.required<TemplateRef<unknown>>('priceTemplate');
  readonly stockTemplate = viewChild.required<TemplateRef<unknown>>('stockTemplate');
  readonly flagsTemplate = viewChild.required<TemplateRef<unknown>>('flagsTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  isAdmin = signal(false);

  constructor(
    private service: ProductsService,
    private alertService: AlertService,
    private translation: AppTranslationService,
    private account: AccountService,
    private categoriesService: ProductCategoriesService
  ) {}

  ngOnInit(): void {
    const roles = this.account.currentUser?.roles || [];
    this.isAdmin.set(roles.some(r => (r || '').toLowerCase() === 'administrator'));

    this.loadingIndicator = true;
    this.loadProducts();
    this.loadCategories();

    const gT = (key: string) => this.translation.getTranslation(key) || key;

    this.columns = [
      { prop: 'name', name: gT('productsWidget.table.Name'), width: 220, sortable: true, cellTemplate: this.nameTemplate() },
      { prop: 'productCategoryName', name: gT('productsWidget.table.Category'), width: 180, sortable: true, cellTemplate: this.categoryTemplate() },
      { prop: 'sellingPrice', name: gT('productsWidget.table.Price'), width: 130, sortable: true, cellTemplate: this.priceTemplate() },
      { prop: 'unitsInStock', name: gT('productsWidget.table.Stock'), width: 120, sortable: true, cellTemplate: this.stockTemplate() },
      { name: gT('productsWidget.table.Flags'), width: 160, sortable: false, cellTemplate: this.flagsTemplate() },
      { name: '', width: 100, cellTemplate: this.actionsTemplate(), resizeable: false, canAutoResize: false, sortable: false, draggable: false }
    ];
  }

  ngOnDestroy(): void { this.editing = {}; }

  private loadProducts() {
    this.service.getAll().subscribe({
      next: data => {
        const ordered = [...data].sort((a, b) => (a.name || '').localeCompare(b.name || ''));
        this.rows = ordered.map((p, idx) => ({ ...p, $$index: idx }));
        this.rowsCache = [...this.rows];
        setTimeout(() => this.loadingIndicator = false, 200);
      },
      error: err => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Error', err?.message || 'Error loading products', MessageSeverity.error);
      }
    });
  }

  private loadCategories() {
    this.categoriesService.getAll().subscribe({
      next: data => this.categories = data,
      error: err => this.alertService.showStickyMessage('Error', err?.message || 'Error loading categories', MessageSeverity.error)
    });
  }

  onSearchChanged(value: string) {
    const v = (value || '').toLowerCase();
    this.rows = this.rowsCache.filter(r =>
      (r.name || '').toLowerCase().includes(v) ||
      (r.productCategoryName || '').toLowerCase().includes(v) ||
      (r.description || '').toLowerCase().includes(v)
    );
  }

  // Alta
  startAddNew() { if (!this.isAdmin()) return; this.addingNew = true; this.savingNew = false; this.newProduct = { name: '', description: '', icon: '', buyingPrice: 0, sellingPrice: 0, unitsInStock: 0, isActive: true, isDiscontinued: false, productCategoryId: 0, parentId: null }; }
  cancelAdd() { this.addingNew = false; this.savingNew = false; this.newProduct = { name: '', description: '', icon: '', buyingPrice: 0, sellingPrice: 0, unitsInStock: 0, isActive: true, isDiscontinued: false, productCategoryId: 0, parentId: null }; }

  onNewInputChange(event: Event, field: keyof ProductVM) {
    const valueRaw = (event.target as HTMLInputElement).value;
    const num = Number(valueRaw);
    switch (field) {
      case 'name': this.newProduct.name = valueRaw; break;
      case 'description': this.newProduct.description = valueRaw; break;
      case 'icon': this.newProduct.icon = valueRaw; break;
      case 'buyingPrice': this.newProduct.buyingPrice = isNaN(num) ? 0 : num; break;
      case 'sellingPrice': this.newProduct.sellingPrice = isNaN(num) ? 0 : num; break;
      case 'unitsInStock': this.newProduct.unitsInStock = isNaN(num) ? 0 : num; break;
      case 'productCategoryId': this.newProduct.productCategoryId = isNaN(num) ? 0 : num; break;
      case 'parentId': this.newProduct.parentId = null; break; // forzar null en todos los ámbitos
      default: break;
    }
  }

  private validateNew(): string | null {
    const gT = (k: string) => this.translation.getTranslation(k) || k;
    const name = (this.newProduct.name || '').trim();
    if (!name) return gT('productsWidget.editor.NameRequired');
    if ((this.newProduct.buyingPrice ?? -1) < 0) return gT('productsWidget.editor.BuyingPriceInvalid');
    if ((this.newProduct.sellingPrice ?? -1) < 0) return gT('productsWidget.editor.SellingPriceInvalid');
    if ((this.newProduct.unitsInStock ?? -1) < 0) return gT('productsWidget.editor.StockInvalid');
    if (!this.newProduct.productCategoryId || this.newProduct.productCategoryId <= 0) return gT('productsWidget.editor.CategoryRequired');
    return null;
  }

  create() {
    if (!this.isAdmin()) return;
    const validationError = this.validateNew();
    if (validationError) { this.alertService.showMessage(validationError, '', MessageSeverity.warn); return; }

    this.savingNew = true;
    const payload: Partial<ProductVM> = {
      name: (this.newProduct.name || '').trim(),
      description: (this.newProduct.description || '').trim() || null,
      icon: (this.newProduct.icon || '').trim() || null,
      buyingPrice: this.newProduct.buyingPrice ?? 0,
      sellingPrice: this.newProduct.sellingPrice ?? 0,
      unitsInStock: this.newProduct.unitsInStock ?? 0,
      isActive: this.newProduct.isActive ?? true,
      isDiscontinued: this.newProduct.isDiscontinued ?? false,
      productCategoryId: this.newProduct.productCategoryId!,
      parentId: null
    };

    this.service.create(payload).subscribe({
      next: () => { this.addingNew = false; this.savingNew = false; this.loadProducts(); },
      error: err => { this.savingNew = false; this.alertService.showMessage('Error', err?.message || 'Error creating product', MessageSeverity.error); }
    });
  }

  // Edición inline
  updateValue(event: Event, cell: keyof ProductVM, row: ProductVM & { $$index?: number }) {
    if (!this.isAdmin()) return;
    const obj = row as unknown as Record<string, unknown>;
    const previous = obj[cell] as unknown;
    const input = (event.target as HTMLInputElement);
    const raw = input.type === 'checkbox' ? (input as HTMLInputElement).checked : input.value;
    const tryNumber = Number(raw);
    const newValue = input.type === 'checkbox' ? raw : (isNaN(tryNumber) ? raw : tryNumber);

    this.editing[(row.$$index ?? 0) + '-' + String(cell)] = false;
    obj[cell as string] = newValue as never;
    this.rows = [...this.rows];

    const payload: Partial<ProductVM> = { [cell]: newValue } as Partial<ProductVM>;
    this.service.update(row.id, payload).subscribe({
      next: (updated) => {
        const idx = this.rowsCache.findIndex(r => r.id === row.id);
        if (idx > -1) this.rowsCache[idx] = { ...this.rowsCache[idx], ...updated };
      },
      error: (err) => {
        obj[cell as string] = previous as never; this.rows = [...this.rows];
        this.alertService.showMessage('Error', err?.message || 'Error updating product', MessageSeverity.error);
      }
    });
  }

  delete(row: ProductVM) {
    if (!this.isAdmin()) return;
    const confirmText = this.translation.getTranslation('productsWidget.dialog.DeleteConfirm');
    this.alertService.showDialog(confirmText, DialogType.confirm, () => this.deleteHelper(row));
  }

  private deleteHelper(row: ProductVM) {
    this.service.delete(row.id).subscribe({
      next: () => { this.rowsCache = this.rowsCache.filter(r => r.id !== row.id); this.rows = this.rows.filter(r => r.id !== row.id); },
      error: err => this.alertService.showMessage('Error', err?.message || 'Error deleting product', MessageSeverity.error)
    });
  }
}
