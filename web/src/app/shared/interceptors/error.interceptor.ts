import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Observable, catchError, map, throwError } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApiResponse } from '../models/api-response.model';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private readonly snackBar: MatSnackBar) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          const body = event.body as ApiResponse;

          if (body && body?.statusCode) {
            // Check status code 4xx, 5xx
            if (
              Math.floor(body.statusCode / 100) === 4 ||
              Math.floor(body.statusCode / 100) === 5
            ) {
              // Display error message if available
              this.showErrorMessage(body.message || 'Unknow error.');
            }
            // Check status code 2xx
            else if (Math.floor(body.statusCode / 100) === 2) {
              // Display success message if available
              const successMessage = body.message;
              if (successMessage && successMessage != 'Success') {
                this.showSuccessMessage(successMessage);
              }
            }
          }
        }
        return event;
      }),
      catchError((error) => {
        // Handle error response
        if (error.error instanceof ErrorEvent) {
          // Client-side error
          this.showErrorMessage('Client error: ' + error.error.message);
        } else {
          // Server-side error
          const errorMessage = error.error?.message ?? 'Unknow error.';
          this.showErrorMessage(errorMessage);
        }

        return throwError(() => error);
      })
    );
  }

  private showErrorMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      panelClass: ['error-snackbar'],
    });
  }

  private showSuccessMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      horizontalPosition: 'right',
      verticalPosition: 'bottom',
      panelClass: ['success-snackbar'],
    });
  }
}
