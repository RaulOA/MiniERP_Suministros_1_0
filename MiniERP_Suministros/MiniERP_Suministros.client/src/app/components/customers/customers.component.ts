// RUTA: MiniERP_Suministros/MiniERP_Suministros.client/src/app/components/customers/customers.component.ts
// Componente principal de clientes. Gestiona la vista y lógica asociada a la sección de clientes.

import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { TodoDemoComponent } from '../controls/todo-demo.component';
import { CustomersWidgetComponent } from '../widgets/customers-widget.component'; // Widget de clientes

@Component({
    standalone: true,
    selector: 'app-customers',
    templateUrl: './customers.component.html',
    styleUrl: './customers.component.scss',
    animations: [fadeInOut],
    imports: [TodoDemoComponent, CustomersWidgetComponent, TranslateModule]
})
export class CustomersComponent {

}
