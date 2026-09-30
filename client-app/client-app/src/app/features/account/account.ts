import { Component, inject } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-account',
  template: `
    <section class="account">
      <h2>Mon compte</h2>
      @if (auth.user(); as u) {
        <p>
          <strong>{{ u.displayName }}</strong> ({{ u.email }})
        </p>
        <p>Rôle(s) : {{ u.roles.join(', ') }}</p>
      }
      <button type="button" (click)="auth.logout()">Se déconnecter</button>
    </section>
  `,
  styles: `
    .account {
      max-width: 600px;
      margin: 2rem auto;
      padding: 2rem;
      background: #fff;
      border: 1px solid #e5e7eb;
      border-radius: 12px;
    }
    button {
      padding: 0.6rem 1rem;
      border: 1px solid #d1d5db;
      border-radius: 8px;
      background: #fff;
      cursor: pointer;
    }
  `,
})
export class AccountComponent {
  readonly auth = inject(AuthService);
}
