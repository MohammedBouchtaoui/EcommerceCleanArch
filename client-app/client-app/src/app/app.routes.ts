import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

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
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.LoginComponent),
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register').then((m) => m.RegisterComponent),
  },
  {
    path: 'account',
    canActivate: [authGuard],
    loadComponent: () => import('./features/account/account').then((m) => m.AccountComponent),
  },
  { path: '**', redirectTo: 'catalog' },
];
