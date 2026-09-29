import { CurrencyPipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EMPTY, Subject, catchError, debounceTime, switchMap } from 'rxjs';
import { Product, ProductParams } from '../../../core/models/product.model';
import { ProductService } from '../../../core/services/product.service';

const DEFAULT_PARAMS: ProductParams = {
  pageIndex: 1,
  pageSize: 6,
  search: '',
  brand: '',
  category: '',
  sort: 'name',
};

@Component({
  selector: 'app-product-list',
  imports: [CurrencyPipe],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductListComponent {
  private readonly productService = inject(ProductService);

  readonly products = signal<Product[]>([]);
  readonly totalCount = signal(0);
  readonly brands = signal<string[]>([]);
  readonly categories = signal<string[]>([]);
  readonly params = signal<ProductParams>({ ...DEFAULT_PARAMS });
  readonly isLoading = signal(true);
  readonly errorMessage = signal<string | null>(null);

  readonly totalPages = computed(() =>
    Math.max(1, Math.ceil(this.totalCount() / this.params().pageSize)),
  );

  readonly sortOptions = [
    { value: 'name', label: 'Nom (A → Z)' },
    { value: 'priceAsc', label: 'Prix croissant' },
    { value: 'priceDesc', label: 'Prix décroissant' },
  ];

  private readonly load$ = new Subject<void>();
  private readonly search$ = new Subject<string>();

  constructor() {
    this.load$
      .pipe(
        switchMap(() => {
          this.isLoading.set(true);
          this.errorMessage.set(null);
          return this.productService.getProducts(this.params()).pipe(
            catchError((err: Error) => {
              this.errorMessage.set(err.message);
              this.isLoading.set(false);
              return EMPTY;
            }),
          );
        }),
        takeUntilDestroyed(),
      )
      .subscribe((page) => {
        this.products.set(page.data);
        this.totalCount.set(page.count);
        this.isLoading.set(false);
      });

    this.search$
      .pipe(debounceTime(300), takeUntilDestroyed())
      .subscribe((value) => this.update({ search: value }));

    this.productService.getBrands().subscribe({
      next: (b) => this.brands.set(b),
      error: () => this.brands.set([]),
    });
    this.productService.getCategories().subscribe({
      next: (c) => this.categories.set(c),
      error: () => this.categories.set([]),
    });

    this.load$.next();
  }

  onSearch(value: string): void {
    this.search$.next(value.trim());
  }

  onBrand(value: string): void {
    this.update({ brand: value });
  }

  onCategory(value: string): void {
    this.update({ category: value });
  }

  onSort(value: string): void {
    this.update({ sort: value });
  }

  goToPage(pageIndex: number): void {
    if (pageIndex < 1 || pageIndex > this.totalPages()) return;
    this.update({ pageIndex }, false);
  }

  reset(): void {
    this.params.set({ ...DEFAULT_PARAMS });
    this.load$.next();
  }

  retry(): void {
    this.load$.next();
  }

  private update(patch: Partial<ProductParams>, resetPage = true): void {
    this.params.update((p) => ({
      ...p,
      ...patch,
      pageIndex: resetPage ? 1 : (patch.pageIndex ?? p.pageIndex),
    }));
    this.load$.next();
  }
}
