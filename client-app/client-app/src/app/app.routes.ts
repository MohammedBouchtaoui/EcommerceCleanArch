import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'catalog' },
  {
    path: 'catalog',
    loadComponent: () =>
      import('./features/catalog/product-list/product-list').then((m) => m.ProductListComponent),
  },
  { path: '**', redirectTo: 'catalog' },
];
