import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ProblemListDto, ProblemResponseDto, ProblemRequest } from '../../shared/models/problem.model';

@Injectable({ providedIn: 'root' })
export class ContentService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.contentUrl}/problems`;

  public getProblems(): Observable<ProblemListDto[]> {
    return this.http.get<ProblemListDto[]>(this.apiUrl);
  }

  public getProblemBySlug(slug: string): Observable<ProblemResponseDto> {
    return this.http.get<ProblemResponseDto>(`${this.apiUrl}/${slug}`);
  }

  public createProblem(request: ProblemRequest): Observable<string> {
    return this.http.post<string>(this.apiUrl, request);
  }

  public deleteProblem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  public updateProblem(id: string, request: ProblemRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, { id, ...request });
  }
}