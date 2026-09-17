import { Component } from '@angular/core';
import { ProductListComponent } from './features/catalog/product-list/product-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProductListComponent],
  template: `<app-product-list></app-product-list>`,
})
export class AppComponent {
  title = 'client-app';
}
