import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/api-response.model';
import { AuthData, LoginRequest } from '../models/auth.model';
import { StorageKeys } from '../../../shared/constants/storage-keys.constants';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;

  constructor(private readonly http: HttpClient) {}

  async login(credentials: LoginRequest): Promise<ApiResponse<AuthData>> {
    const response = await firstValueFrom(
      this.http.post<ApiResponse<AuthData>>(`${this.apiUrl}/login`, credentials)
    );

    if (response?.success && response?.data?.accessToken) {
      this.setAuthTokens(response.data.accessToken, response.data.refreshToken);
    }
    return response;
  }

  logout(): void {
    this.removeAuthTokens();
  }

  getAccessToken(): string | null {
    return localStorage.getItem(StorageKeys.ACCESS_TOKEN);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(StorageKeys.REFRESH_TOKEN);
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  private setAuthTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(StorageKeys.ACCESS_TOKEN, accessToken);
    localStorage.setItem(StorageKeys.REFRESH_TOKEN, refreshToken);
  }

  private removeAuthTokens(): void {
    localStorage.removeItem(StorageKeys.ACCESS_TOKEN);
    localStorage.removeItem(StorageKeys.REFRESH_TOKEN);
  }
}
