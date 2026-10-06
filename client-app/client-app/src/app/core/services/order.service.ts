import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateOrderRequest, Order } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/orders`;

  create(request: CreateOrderRequest): Observable<Order> {
    return this.http.post<Order>(this.url, request);
  }

  getMine(): Observable<Order[]> {
    return this.http.get<Order[]>(this.url);
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.url}/${id}`);
  }

  pay(id: number): Observable<Order> {
    return this.http.post<Order>(`${this.url}/${id}/pay`, {});
  }

 














 cancel(id: number): Observable<void> {
    return this.http.post<void>(`${this.url}/${id}/cancel`, {});
  }
}
