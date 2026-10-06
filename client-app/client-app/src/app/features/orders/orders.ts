import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Order, STATUS_LABELS } from '../../core/models/order.model';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class OrdersComponent {
  private readonly orderService = inject(OrderService);

  readonly labels = STATUS_LABELS;
  readonly orders = signal<Order[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  constructor() {
    this.orderService.getMine().subscribe({
      next: (list) => {
        this.orders.set(list);
        this.loading.set(false);
      },
 













     error: (e: Error) => {
        this.error.set(e.message);
        this.loading.set(false);
      },
    });
  }
}
