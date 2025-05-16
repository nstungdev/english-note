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
          if (event.status / 100 === 2) {
            const response: ApiResponse<any> = event.body;
            if (response?.message && response?.message !== 'Success') {
              this.showSuccessMessage(response.message);
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
          const errorMessage = error.error?.message;
          if (errorMessage) {
            this.showErrorMessage(errorMessage);
          } else if (error.status === 401) {
            this.showErrorMessage('Unauthorized access. Please log in again.');
          } else if (error.status === 403) {
            this.showErrorMessage(
              'Forbidden access. You do not have permission to perform this action.'
            );
          } else {
            this.showErrorMessage(
              'Unknown error occurred. Please try again later.'
            );
          }
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
