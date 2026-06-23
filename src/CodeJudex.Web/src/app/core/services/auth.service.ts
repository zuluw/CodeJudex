import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../../../environments/environment';
import { AuthResponse, UserRole } from '../../shared/models/auth.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly apiUrl = `${environment.identityUrl}/auth`;

  private readonly _isLoggedIn = signal<boolean>(!!localStorage.getItem('token'));
  private readonly _userFullName = signal<string>(localStorage.getItem('fullName') || '');
  private readonly _userRole = signal<UserRole>((localStorage.getItem('role') as UserRole) || 'Student');

  public readonly isLoggedIn = this._isLoggedIn.asReadonly();
  public readonly userFullName = this._userFullName.asReadonly();
  public readonly userRole = this._userRole.asReadonly();

  public login(credentials: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, credentials).pipe(
      tap(res => this.setSession(res))
    );
  }

  public register(data: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap(res => this.setSession(res))
    );
  }

  private setSession(res: AuthResponse): void {
    const decoded: any = jwtDecode(res.accessToken);
    const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 'Student';

    localStorage.setItem('token', res.accessToken);
    localStorage.setItem('refreshToken', res.refreshToken);
    localStorage.setItem('fullName', res.fullName);
    localStorage.setItem('role', role);

    this._isLoggedIn.set(true);
    this._userFullName.set(res.fullName);
    this._userRole.set(role);

    const target = role === 'Admin' ? '/app/admin/dashboard' : '/app/dashboard';
    this.router.navigate([target]);
  }

  public logout(): void {
    localStorage.clear();
    this._isLoggedIn.set(false);
    this._userRole.set('Student');
    this.router.navigate(['/']);
  }
}