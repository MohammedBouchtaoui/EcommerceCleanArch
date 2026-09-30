import { registerLocaleData } from '@angular/common';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import localeFr from '@angular/common/locales/fr';
import { ApplicationConfig, LOCALE_ID, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';

registerLocaleData(localeFr);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    // L'ordre compte : authInterceptor est le plus proche du réseau, il voit donc
    // l'erreur HTTP brute (401) avant que errorInterceptor ne la convertisse en message.
    provideHttpClient(withFetch(), withInterceptors([errorInterceptor, authInterceptor])),
    { provide: LOCALE_ID, useValue: 'fr-FR' },
  ],
};
