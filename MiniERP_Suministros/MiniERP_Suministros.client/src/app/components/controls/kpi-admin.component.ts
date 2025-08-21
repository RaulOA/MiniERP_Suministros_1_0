/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/controls/kpi-admin.component.ts
Descripción: KPI de estadísticas para administrador. Replica el widget de demo pero alimentado desde la base de datos (pedidos) y agrega totales por mes.
*/

import { Component, OnInit, OnDestroy, inject, viewChild } from '@angular/core';
import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { BaseChartDirective } from 'ng2-charts';
import { ChartEvent, ChartType } from 'chart.js';
import { NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownButtonItem, NgbDropdownItem } from '@ng-bootstrap/ng-bootstrap';
import { OrdersEndpoint } from '../../services/orders-endpoint.service';
import { OrderVM } from '../../models/order.model';

interface ChartEventArgs { event: ChartEvent; active: object[] }

@Component({
  selector: 'app-kpi-admin',
  templateUrl: './kpi-shared.component.html',
  styleUrl: './kpi-shared.component.scss',
  imports: [BaseChartDirective, NgbDropdown, NgbDropdownToggle, NgbDropdownMenu, NgbDropdownButtonItem, NgbDropdownItem]
})
export class KpiAdminComponent implements OnInit, OnDestroy {
  private alertService = inject(AlertService);
  private ordersEndpoint = inject(OrdersEndpoint);

  chartOptions: object | undefined;
  chartType: ChartType = 'line';
  chartLabels: string[] = [];
  chartData: { data: number[]; label: string; fill: 'origin' | false }[] = [];

  timerReference: ReturnType<typeof setInterval> | undefined;

  readonly chart = viewChild.required(BaseChartDirective);

  ngOnInit() {
    this.refreshChartOptions();
    this.loadData();
    this.timerReference = setInterval(() => this.randomize(), 60000); // refresco cada 60s
  }

  ngOnDestroy() { clearInterval(this.timerReference); }

  private buildBaseOptions() {
    return {
      responsive: true,
      maintainAspectRatio: false,
      title: { display: false, fontSize: 16, text: 'KPIs' }
    };
  }

  refreshChartOptions() {
    const baseOptions = this.buildBaseOptions();

    if (this.chartType !== 'line') {
      this.chartOptions = baseOptions;
    } else {
      const lineChartOptions = { elements: { line: { tension: 0.5 } } };
      this.chartOptions = { ...baseOptions, ...lineChartOptions };
    }
  }

  private monthKey(dt: Date) {
    return `${dt.getFullYear()}-${(dt.getMonth() + 1).toString().padStart(2, '0')}`; // YYYY-MM
  }

  private getLastMonths(count: number) {
    const labels: { key: string; label: string }[] = [];
    const now = new Date();
    for (let i = count - 1; i >= 0; i--) {
      const d = new Date(now.getFullYear(), now.getMonth() - i, 1);
      const key = this.monthKey(d);
      const label = d.toLocaleString(undefined, { month: 'short' });
      labels.push({ key, label });
    }
    return labels;
  }

  private computeTotals(orders: OrderVM[]) {
    const months = this.getLastMonths(6);

    const sums = new Map<string, number>();
    const counts = new Map<string, number>();

    for (const m of months) { sums.set(m.key, 0); counts.set(m.key, 0); }

    for (const o of orders) {
      if (!o?.createdDate) continue;
      const created = new Date(o.createdDate);
      const key = this.monthKey(new Date(created.getFullYear(), created.getMonth(), 1));
      if (!sums.has(key)) continue; // fuera de rango

      const subtotal = o.subtotal ?? (o.items?.reduce((acc, i) => acc + (i.unitPrice * i.quantity), 0) ?? 0);
      const itemsDiscount = o.itemsDiscount ?? (o.items?.reduce((acc, i) => acc + (i.discount * i.quantity), 0) ?? 0);
      const headerDiscount = o.discount ?? 0;
      const total = (o.total ?? (subtotal - itemsDiscount - headerDiscount)) || 0;

      sums.set(key, (sums.get(key) || 0) + total);
      counts.set(key, (counts.get(key) || 0) + 1);
    }

    const totals = months.map(m => +(sums.get(m.key) || 0).toFixed(2));
    const qty = months.map(m => counts.get(m.key) || 0);
    const average = months.map((_, idx) => {
      const c = qty[idx];
      return +((c ? totals[idx] / c : 0).toFixed(2));
    });

    this.chartLabels = months.map(m => m.label);
    this.chartData = [
      { data: totals, label: 'Total Ventas', fill: 'origin' },
      { data: qty, label: 'Pedidos', fill: 'origin' },
      { data: average, label: 'Ticket Promedio', fill: 'origin' }
    ];
  }

  private loadData() {
    this.ordersEndpoint.getAll().subscribe({
      next: (orders) => {
        this.computeTotals(orders || []);
        setTimeout(() => this.chart()?.update());
      },
      error: (err) => {
        this.alertService.showMessage('Error', err?.message ?? 'Error loading KPI admin', MessageSeverity.error);
      }
    });
  }

  randomize(): void { this.loadData(); }

  changeChartType(type: ChartType) { this.chartType = type; this.refreshChartOptions(); }

  showMessage(msg: string): void { this.alertService.showMessage('KPI Admin', msg, MessageSeverity.info); }

  showDialog(msg: string): void {
    this.alertService.showDialog(msg, DialogType.prompt, (val) => this.configure(true, val), () => this.configure(false));
  }

  configure(response: boolean, value?: string) {
    if (response) {
      this.alertService.showStickyMessage('Simulating...', '', MessageSeverity.wait);
      setTimeout(() => {
        this.alertService.resetStickyMessage();
        this.alertService.showMessage('KPI Admin', `Your settings was successfully configured to "${value}"`, MessageSeverity.success);
      }, 2000);
    } else {
      this.alertService.showMessage('KPI Admin', 'Operation cancelled by user', MessageSeverity.default);
    }
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  chartHovered(e: ChartEventArgs): void { /* Demo */ }
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  chartClicked(e: Partial<ChartEventArgs>): void { /* Demo */ }
}
