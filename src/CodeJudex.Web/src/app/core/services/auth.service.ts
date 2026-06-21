import { Injectable, signal, inject } from '@angular/core';
import { Router } from '@angular/router';

export type UserRole = 'Student' | 'Admin';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly router = inject(Router);
  private readonly STORAGE_KEY = 'codejudex_auth_status';
  private readonly ROLE_KEY = 'codejudex_user_role';

  private readonly _isLoggedIn = signal<boolean>(localStorage.getItem(this.STORAGE_KEY) === 'true');
  
  private readonly _userRole = signal<UserRole>(
    (localStorage.getItem(this.ROLE_KEY) as UserRole) || 'Student'
  );

  public readonly isLoggedIn = this._isLoggedIn.asReadonly();
  public readonly userRole = this._userRole.asReadonly();

  public login(role: UserRole = 'Student'): void {
    localStorage.setItem(this.STORAGE_KEY, 'true');
    localStorage.setItem(this.ROLE_KEY, role);
    this._isLoggedIn.set(true);
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