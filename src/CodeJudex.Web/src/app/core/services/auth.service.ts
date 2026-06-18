import { Injectable, signal, computed, inject } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly router = inject(Router);
  private readonly STORAGE_KEY = 'codejudex_auth_status';

  private readonly _isLoggedIn = signal<boolean>(this.getInitialAuthStatus());

  public readonly isLoggedIn = this._isLoggedIn.asReadonly();

  public login(): void {
    localStorage.setItem(this.STORAGE_KEY, 'true');
    this._isLoggedIn.set(true);
    this.router.navigate(['/app/dashboard']);
  }

  public logout(): void {
    localStorage.removeItem(this.STORAGE_KEY);
    this._isLoggedIn.set(false);
    this.router.navigate(['/']);
  }

  private getInitialAuthStatus(): boolean {
    return localStorage.getItem(this.STORAGE_KEY) === 'true';
  }
}