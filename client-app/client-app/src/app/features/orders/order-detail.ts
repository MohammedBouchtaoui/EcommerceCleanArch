import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Order, STATUS_LABELS } from '../../core/models/order.model';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-order-detail',
  imports: [CurrencyPipe, DatePipe, RouterLink],
  templateUrl: './order-detail.html',
  styleUrl: './orders.css',
})
export class OrderDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly orderService = inject(OrderService);
  private readonly id = Number(this.route.snapshot.paramMap.get('id'));

  readonly labels = STATUS_LABELS;
  readonly order = signal<Order | null>(null);
  readonly loading = signal(true);
  readonly busy = signal(false);
  readonly error = signal<string | null>(null);

  constructor() {
    this.load();













  }

  pay(): void {
    this.busy.set(true);
    this.error.set(null);
    this.orderService.pay(this.id).subscribe({
      next: (o) => {
        this.order.set(o);
        this.busy.set(false);
      },
      error: (e: Error) => {
        this.error.set(e.message);
        this.busy.set(false);
      },
 













   });
  }

  cancel(): void {
    this.busy.set(true);
    this.error.set(null);
    this.orderService.cancel(this.id).subscribe({
      next: () => {
        this.busy.set(false);
        this.load();
      },











      error: (e: Error) => {
        this.error.set(e.message);
        this.busy.set(false);
      },
    });
  }

  private load(): void {
 







   this.orderService.getById(this.id).subscribe({
      next: (o) => {
        this.order.set(o);
        this.loading.set(false);
      },
      error: (e: Error) => {
 





       this.error.set(e.message);
        this.loading.set(false);
      },
    });
  }
}
