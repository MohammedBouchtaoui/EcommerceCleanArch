import { Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { BasketService } from './core/services/basket.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink],
  template: `
    <header class="app-header">
      <a routerLink="/catalog" class="brand">🛒 Ecommerce</a>
      <nav>
        <a routerLink="/basket" class="nav-link">
          Panier <span class="count">{{ basket.itemCount() }}</span>
        </a>
      </nav>
    </header>
    <main><router-outlet /></main>
  `,
  styles: `
    .app-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: #111827;
      padding: 1rem 2rem;
    }
    .brand {
      color: #fff;
      font-weight: 700;
      font-size: 1.25rem;
      text-decoration: none;
    }
    nav {
      display: flex;
      gap: 1.25rem;
      align-items: center;
    }
    .nav-link {
      color: #e5e7eb;
      text-decoration: none;
    }
    .count {
      display: inline-block;
      min-width: 1.4rem;
      text-align: center;
      margin-left: 0.25rem;
      background: #f59e0b;
      color: #111827;
      border-radius: 999px;
      font-weight: 700;
      font-size: 0.8rem;
      padding: 0.1rem 0.4rem;
    }
  `,
})
export class AppComponent {
  readonly basket = inject(BasketService);

  constructor() {
    this.basket.load();
  }
}
