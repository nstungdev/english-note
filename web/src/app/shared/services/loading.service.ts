import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoadingService {
  private readonly loadingSubject = new BehaviorSubject<boolean>(false);
  private requestCount = 0;

  get isLoading$(): Observable<boolean> {
    return this.loadingSubject.asObservable();
  }

  show(): void {
    this.requestCount++;
    if (this.requestCount === 1) {
      this.loadingSubject.next(true);
    }
  }

  hide(): void {
    this.requestCount--;
    if (this.requestCount === 0) {
      this.loadingSubject.next(false);
    }
  }
}
