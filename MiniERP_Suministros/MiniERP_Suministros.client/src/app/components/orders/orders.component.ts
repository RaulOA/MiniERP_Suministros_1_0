import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { OrdersWidgetComponent } from '../widgets/orders-widget.component';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.component.html',
    styleUrl: './orders.component.scss',
    animations: [fadeInOut],
    standalone: true,
    imports: [TranslateModule, OrdersWidgetComponent]
})
export class OrdersComponent { }
