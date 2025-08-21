import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { ProductCategoriesWidgetComponent } from '../widgets/product-categories-widget.component';
import { ProductsWidgetComponent } from '../widgets/products-widget.component';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrl: './products.component.scss',
    animations: [fadeInOut],
    imports: [TranslateModule, ProductCategoriesWidgetComponent, ProductsWidgetComponent]
})
export class ProductsComponent {
}
