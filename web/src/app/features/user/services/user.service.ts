import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group, Permission, User } from '../models/user.model';
import { ApiResponse } from '@/shared/models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl = `${environment.apiUrl}/users`;

  constructor(private readonly http: HttpClient) {}

  getList() {
    return this.http.get<ApiResponse<User[]>>(`${environment.apiUrl}/users`);
  }

  getById(userId: number): Observable<ApiResponse<User>> {
    return this.http.get<ApiResponse<User>>(
      `${environment.apiUrl}/users/${userId}`
    );
  }

  getPermissions(): Observable<ApiResponse<Permission[]>> {
    return this.http.get<ApiResponse<Permission[]>>(
      `${environment.apiUrl}/users/permissions`
    );
  }

  getGroups(): Observable<ApiResponse<Group[]>> {
    return this.http.get<ApiResponse<Group[]>>(
      `${environment.apiUrl}/users/groups`
    );
  }

  toggleBlock(userId: number, isBlocked: boolean): Observable<void> {
    const endpoint = isBlocked ? 'unblock' : 'block';
    return this.http.post<void>(
      `${environment.apiUrl}/users/${userId}/${endpoint}`,
      {}
    );
  }
}
