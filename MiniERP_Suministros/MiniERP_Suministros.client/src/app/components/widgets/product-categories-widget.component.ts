/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/widgets/product-categories-widget.component.ts
Descripción: Widget para visualizar, crear, editar en línea y eliminar categorías de producto usando ngx-datatable.
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
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductCategory } from '../../models/product-category.model';

@Component({
  standalone: true,
  selector: 'app-product-categories-widget',
  templateUrl: './product-categories-widget.component.html',
  styleUrl: './product-categories-widget.component.scss',
  imports: [SearchBoxComponent, NgxDatatableModule, AutofocusDirective, NgbTooltip, TranslateModule]
})
export class ProductCategoriesWidgetComponent implements OnInit, OnDestroy {
  columns: TableColumn[] = [];
  rows: (ProductCategory & { $$index?: number })[] = [];
  rowsCache: (ProductCategory & { $$index?: number })[] = [];
  editing: Record<string, boolean> = {};
  loadingIndicator = true;

  // Creación de nueva categoría
  addingNew = false;
  savingNew = false;
  newCategory: Partial<ProductCategory> = { name: '', description: '', icon: '' };

  readonly verticalScrollbar = input(false);

  readonly nameTemplate = viewChild.required<TemplateRef<unknown>>('nameTemplate');
  readonly descriptionTemplate = viewChild.required<TemplateRef<unknown>>('descriptionTemplate');
  readonly iconTemplate = viewChild.required<TemplateRef<unknown>>('iconTemplate');
  readonly actionsTemplate = viewChild.required<TemplateRef<unknown>>('actionsTemplate');

  constructor(
    private categoriesService: ProductCategoriesService,
    private alertService: AlertService,
    private translationService: AppTranslationService
  ) {}

  ngOnInit(): void {
    this.loadingIndicator = true;
    this.loadCategories();

    const gT = (key: string) => this.translationService.getTranslation(key);

    this.columns = [
      { prop: 'name', name: gT('productsWidget.table.Name') || 'Name', width: 220, sortable: true, cellTemplate: this.nameTemplate() },
      { prop: 'description', name: gT('productsWidget.table.Description') || 'Description', width: 420, sortable: false, cellTemplate: this.descriptionTemplate() },
      { prop: 'icon', name: gT('productsWidget.table.Icon') || 'Icon', width: 150, sortable: false, cellTemplate: this.iconTemplate() },
      { name: '', width: 80, cellTemplate: this.actionsTemplate(), resizeable: false, canAutoResize: false, sortable: false, draggable: false }
    ];
  }

  ngOnDestroy(): void { this.editing = {}; }

  private loadCategories() {
    this.categoriesService.getAll().subscribe({
      next: (data) => {
        this.rows = data.map((c, idx) => ({ ...c, $$index: idx }));
        this.rowsCache = [...this.rows];
        setTimeout(() => { this.loadingIndicator = false; }, 300);
      },
      error: (err) => {
        this.loadingIndicator = false;
        this.alertService.showStickyMessage(
          this.translationService.getTranslation('app.alerts.LoadingError'),
          err?.message ?? 'Error loading categories',
          MessageSeverity.error
        );
      }
    });
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r => Utilities.searchArray(value, false, r.name || '', r.description || '', r.icon || ''));
  }

  // ===================== Alta =====================
  startAddNew() {
    this.addingNew = true;
    this.savingNew = false;
    this.newCategory = { name: '', description: '', icon: '' };
  }

  cancelAdd() {
    this.addingNew = false;
    this.savingNew = false;
    this.newCategory = { name: '', description: '', icon: '' };
  }

  onNewInputChange(event: Event, field: 'name' | 'description' | 'icon') {
    const value = (event.target as HTMLInputElement).value;
    (this.newCategory as Record<string, unknown>)[field] = value;
  }

  private validateNew(): string | null {
    const name = (this.newCategory.name || '').trim();
    const description = (this.newCategory.description || '').trim();
    const icon = (this.newCategory.icon || '').trim();

    if (!name) return this.translationService.getTranslation('customersWidget.editor.NameRequired') || 'Name is required';
    if (name.length > 100) return this.translationService.getTranslation('customersWidget.editor.NameMax') || 'Name too long';

    if (description && description.length > 500) return 'Description too long';
    if (icon && icon.length > 256) return 'Icon too long';

    return null;
  }

  create() {
    const validationError = this.validateNew();
    if (validationError) {
      this.alertService.showMessage(validationError, '', MessageSeverity.warn);
      return;
    }

    this.savingNew = true;
    const payload: Partial<ProductCategory> = {
      name: (this.newCategory.name || '').trim(),
      description: (this.newCategory.description || '').trim() || null,
      icon: (this.newCategory.icon || '').trim() || null
    };

    this.categoriesService.create(payload).subscribe({
      next: () => {
        this.loadCategories();
        this.addingNew = false;
        this.savingNew = false;
        this.newCategory = { name: '', description: '', icon: '' };
      },
      error: (err) => {
        this.savingNew = false;
        this.alertService.showMessage('Error', err?.message ?? 'Error creating category', MessageSeverity.error);
      }
    });
  }
  // ================================================================

  updateValue(event: Event, cell: 'name' | 'description' | 'icon', row: ProductCategory & { $$index?: number }) {
    const obj = row as unknown as Record<string, unknown>;
    const previous = obj[cell] as string | null | undefined;
    const newValue = (event.target as HTMLInputElement).value;

    this.editing[(row.$$index ?? 0) + '-' + cell] = false;
    obj[cell] = newValue;
    this.rows = [...this.rows];

    this.categoriesService.update(row.id, { [cell]: newValue }).subscribe({
      next: (updated) => {
        const idx = this.rowsCache.findIndex(r => r.id === row.id);
        if (idx > -1) {
          this.rowsCache[idx] = { ...this.rowsCache[idx], ...updated };
        }
      },
      error: (err) => {
        obj[cell] = previous ?? null;
        this.rows = [...this.rows];
        this.alertService.showMessage('Error', err?.message ?? 'Error updating category', MessageSeverity.error);
      }
    });
  }

  delete(row: ProductCategory) {
    const confirmText = this.translationService.getTranslation('customersWidget.dialog.DeleteConfirm');
    this.alertService.showDialog(confirmText, DialogType.confirm, () => this.deleteHelper(row));
  }

  private deleteHelper(row: ProductCategory) {
    this.categoriesService.delete(row.id).subscribe({
      next: () => {
        this.rowsCache = this.rowsCache.filter(item => item !== row);
        this.rows = this.rows.filter(item => item !== row);
      },
      error: (err) => {
        const msg = err?.error?.message || err?.message || 'Error deleting category';
        this.alertService.showMessage('Error', msg, MessageSeverity.error);
      }
    });
  }
}
