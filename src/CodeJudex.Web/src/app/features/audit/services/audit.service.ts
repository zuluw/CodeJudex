import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { AuditResponse } from '../../../shared/models/audit.models';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuditApiService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiUrl}/audit`; 

  public runAudit(sourceCode: string): Observable<AuditResponse> {
    const payload = { sourceCode, language: 'csharp' };

    return this.http.post<AuditResponse>(this.endpoint, payload).pipe(
      retry(1),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred during code audit.';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      errorMessage = `Backend returned code ${error.status}: ${error.error?.message || error.message}`;
    }

    console.error(`[AuditApiService] ${errorMessage}`);
    return throwError(() => new Error(errorMessage));
  }
}