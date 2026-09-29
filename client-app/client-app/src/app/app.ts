import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  template: `
    <header class="app-header">
      <a routerLink="/catalog" class="brand">🛒 Ecommerce</a>
    </header>
    <main><router-outlet /></main>
  `,
  styles: `
    .app-header {
      background: #111827;
      padding: 1rem 2rem;
    }
    .brand {
      color: #fff;
      font-weight: 700;
      font-size: 1.25rem;
      text-decoration: none;
    }
  `,
})
export class AppComponent {}
