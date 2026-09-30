import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, finalize } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Basket } from '../models/basket.model';

const STORAGE_KEY = 'basket_key';

@Injectable({ providedIn: 'root' })
export class BasketService {
  private readonly http = inject(HttpClient);
  private readonly key = this.getOrCreateKey();
  private readonly url = `${environment.apiUrl}/basket/${this.key}`;

  private readonly basket = signal<Basket | null>(null);

  readonly items = computed(() => this.basket()?.items ?? []);
  readonly itemCount = computed(() => this.basket()?.totalItems ?? 0);
  readonly subtotal = computed(() => this.basket()?.subtotal ?? 0);
  readonly isEmpty = computed(() => this.items().length === 0);

  readonly busy = signal(false);
  readonly error = signal<string | null>(null);

  load(): void {
    this.run(this.http.get<Basket>(this.url));
  }

  add(productId: number, quantity = 1): void {
    this.run(this.http.post<Basket>(`${this.url}/items`, { productId, quantity }));
  }

  setQuantity(productId: number, quantity: number): void {
    this.run(this.http.put<Basket>(`${this.url}/items/${productId}`, { quantity }));
  }

  remove(productId: number): void {
    this.run(this.http.delete<Basket>(`${this.url}/items/${productId}`));
  }

  clear(): void {
    this.busy.set(true);
    this.error.set(null);
    this.http
      .delete<void>(this.url)
      .pipe(finalize(() => this.busy.set(false)))
      .subscribe({
        next: () => this.basket.set({ key: this.key, items: [], totalItems: 0, subtotal: 0 }),
        error: (e: Error) => this.error.set(e.message),
      });
  }

  private run(request$: Observable<Basket>): void {
    this.busy.set(true);
    this.error.set(null);
    request$.pipe(finalize(() => this.busy.set(false))).subscribe({
      next: (b) => this.basket.set(b),
      error: (e: Error) => this.error.set(e.message),
    });
  }

  private getOrCreateKey(): string {
    let key = localStorage.getItem(STORAGE_KEY);
    if (!key) {
      key = crypto.randomUUID();
      localStorage.setItem(STORAGE_KEY, key);
    }
    return key;
  }
}
