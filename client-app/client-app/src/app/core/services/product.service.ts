import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Pagination, Product, ProductParams } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/products`;

  getProducts(p: ProductParams): Observable<Pagination<Product>> {
    let params = new HttpParams().set('pageIndex', p.pageIndex).set('pageSize', p.pageSize);

    if (p.search) params = params.set('search', p.search);
    if (p.brand) params = params.set('brand', p.brand);
    if (p.category) params = params.set('category', p.category);
    if (p.sort) params = params.set('sort', p.sort);

    return this.http.get<Pagination<Product>>(this.baseUrl, { params });
  }

  getBrands(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/brands`);
  }

  getCategories(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/categories`);
  }
}
