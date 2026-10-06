import { CurrencyPipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { BasketService } from '../../core/services/basket.service';
import { OrderService } from '../../core/services/order.service';

@Component({
  selector: 'app-checkout',
  imports: [ReactiveFormsModule, CurrencyPipe, RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css',
})
export class CheckoutComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly router = inject(Router);
  private readonly orders = inject(OrderService);
  private readonly auth = inject(AuthService);
  readonly basket = inject(BasketService);

 








 readonly form = this.fb.group({
    fullName: [this.auth.user()?.displayName ?? '', [Validators.required, Validators.maxLength(100)]],
    line1: ['', [Validators.required, Validators.maxLength(200)]],
    city: ['', [Validators.required, Validators.maxLength(100)]],
    postalCode: ['', [Validators.required, Validators.maxLength(20)]],
    country: ['France', [Validators.required, Validators.maxLength(100)]],
  });

  readonly loading = signal(false);
  readonly error = signal<string | null>(null);











  invalid(name: string): boolean {
    const control = this.form.get(name);
    return !!control && control.touched && control.invalid;
  }

  submit(): void {
    if (this.form.invalid || this.basket.isEmpty()) {
 







     this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.orders
 







     .create({ basketKey: this.basket.basketKey, shippingAddress: this.form.getRawValue() })
      .subscribe({
        next: (order) => {
          this.basket.clearLocal();
          this.router.navigate(['/orders', order.id]);
        },
 





       error: (e: Error) => {
          this.error.set(e.message);
          this.loading.set(false);
          this.basket.load();   // le stock a pu changer : on rafraîchit le panier
 



       },
      });
  }
}
