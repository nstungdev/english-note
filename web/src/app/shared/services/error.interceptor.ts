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
            // Kiểm tra nếu status code là 4xx hoặc 5xx
            if (
              Math.floor(body.statusCode / 100) === 4 ||
              Math.floor(body.statusCode / 100) === 5
            ) {
              // Hiển thị thông báo lỗi qua snackbar
              this.showErrorMessage(body.message || 'Unknow error.');
            }
          }
        }
        return event;
      }),
      catchError((error) => {
        // Xử lý lỗi HTTP không phải từ response body
        if (error.error instanceof ErrorEvent) {
          // Lỗi client-side hoặc mạng
          this.showErrorMessage('Client error: ' + error.error.message);
        } else {
          // Lỗi từ backend
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
}
