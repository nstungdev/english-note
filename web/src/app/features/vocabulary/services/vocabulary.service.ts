import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  VocabularyDTO,
  CreateVocabularyRequest,
  UpdateVocabularyRequest,
} from '../models/vocabulary.model';
import { environment } from '@environments/environment';
import { Paging } from '@/shared/models/paging.model';
import { ApiResponse } from '@/shared/models/api-response.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class VocabularyService {
  private readonly baseUrl = `${environment.apiUrl}/vocabularies`;

  constructor(private readonly http: HttpClient) {}

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

  importVocabularyBulk(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<any>(`${this.baseUrl}/bulk-upload`, formData);
  }

  exportToJson(): Observable<{ blob: Blob; filename: string }> {
    return this.http
      .get(`${this.baseUrl}/export-json`, {
        responseType: 'blob',
        observe: 'response',
      })
      .pipe(
        map((response) => {
          const contentDisposition = response.headers.get(
            'content-disposition'
          );
          const filename = contentDisposition
            ? contentDisposition.split('filename=')[1]?.split(';')[0]?.trim() ||
              'vocabularies.json'
            : 'vocabularies.json';
          return { blob: response.body as Blob, filename };
        })
      );
  }
}
