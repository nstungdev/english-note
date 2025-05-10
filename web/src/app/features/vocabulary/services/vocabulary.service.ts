import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  VocabularyDTO,
  CreateVocabularyRequest,
  UpdateVocabularyRequest,
} from '../models/vocabulary.model';
import { environment } from '@environments/environment';
import { map } from 'rxjs/operators';
import { Paging } from '@/shared/models/paging.model';
import { ApiResponse } from '@/shared/models/api-response.model';

@Injectable({
  providedIn: 'root',
})
export class VocabularyService {
  private readonly baseUrl = `${environment.apiUrl}/vocabularies`;

  constructor(private http: HttpClient) {}

  create(
    request: CreateVocabularyRequest
  ): Observable<{ message: string; statusCode: number }> {
    return this.http.post<{ message: string; statusCode: number }>(
      this.baseUrl,
      request
    );
  }

  update(
    request: UpdateVocabularyRequest
  ): Observable<{ message: string; statusCode: number }> {
    return this.http.put<{ message: string; statusCode: number }>(
      this.baseUrl,
      request
    );
  }

  getById(id: number): Observable<VocabularyDTO> {
    return this.http.get<VocabularyDTO>(`${this.baseUrl}/${id}`);
  }

  getList(
    page: number = 1,
    pageSize: number = 10
  ): Observable<ApiResponse<Paging<VocabularyDTO>>> {
    return this.http.get<ApiResponse<Paging<VocabularyDTO>>>(this.baseUrl, {
      params: { page: page.toString(), pageSize: pageSize.toString() },
    });
  }
}
