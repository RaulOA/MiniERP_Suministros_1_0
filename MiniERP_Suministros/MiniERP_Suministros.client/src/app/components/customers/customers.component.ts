// RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/customers/customers.component.ts
// Componente principal de clientes. Gestiona la vista y lógica asociada a la sección de clientes.

import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { CustomersWidgetComponent } from '../widgets/customers-widget.component'; // Widget de clientes

@Component({
    standalone: true,
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrl: './customers.component.scss',
    animations: [fadeInOut],
    imports: [CustomersWidgetComponent, TranslateModule] // Quitado TodoDemoComponent (no usado en plantilla)
})
export class CustomersComponent {

}
