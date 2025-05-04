import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../services/loading.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="loading$ | async" class="loading-overlay">
      <div class="spinner-container">
        <div class="spinner">
          <div class="double-bounce1"></div>
          <div class="double-bounce2"></div>
        </div>
        <div class="loading-text">Loading...</div>
      </div>
    </div>
  `,
  styles: [
    `
      .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.4);
        z-index: 9999;
        display: flex;
        justify-content: center;
        align-items: center;
        backdrop-filter: blur(3px);
        transition: all 0.3s ease;
      }

      .spinner-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        background-color: white;
        padding: 20px;
        border-radius: 12px;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
        animation: fade-in 0.3s ease;
      }

      .spinner {
        width: 60px;
        height: 60px;
        position: relative;
        margin-bottom: 10px;
      }

      .double-bounce1,
      .double-bounce2 {
        width: 100%;
        height: 100%;
        border-radius: 50%;
        background-color: #3f51b5;
        opacity: 0.6;
        position: absolute;
        top: 0;
        left: 0;
        animation: sk-bounce 2s infinite ease-in-out;
      }

      .double-bounce2 {
        animation-delay: -1s;
        background-color: #ff4081;
      }

      .loading-text {
        color: #333;
        font-size: 16px;
        font-weight: 500;
        margin-top: 10px;
        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      }

      @keyframes sk-bounce {
        0%,
        100% {
          transform: scale(0);
        }
        50% {
          transform: scale(1);
        }
      }

      @keyframes fade-in {
        from {
          opacity: 0;
          transform: translateY(-20px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }
    `,
  ],
})
export class LoadingComponent implements OnInit {
  loading$?: Observable<boolean>;

  constructor(private readonly loadingService: LoadingService) {}

  ngOnInit(): void {
    this.loading$ = this.loadingService.isLoading$;
  }
}
