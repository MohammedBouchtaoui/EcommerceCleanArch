import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'catalog' },
  {
    path: 'catalog',
    loadComponent: () =>
      import('./features/catalog/product-list/product-list').then((m) => m.ProductListComponent),
  },
  {
    path: 'basket',
    loadComponent: () => import('./features/basket/basket').then((m) => m.BasketComponent),
  },
  { path: '**', redirectTo: 'catalog' },
];
