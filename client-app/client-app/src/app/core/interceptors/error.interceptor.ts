import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

interface ProblemDetails {
  title?: string;
  detail?: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const problem = err.error as ProblemDetails | null;
      const message =
        err.status === 0
          ? "Impossible de joindre l'API. Vérifiez qu'elle est démarrée."
          : (problem?.detail ?? problem?.title ?? 'Une erreur est survenue.');
      return throwError(() => new Error(message));
    }),
  );
