import { Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isLoggedIn = signal(false);

  constructor(private router: Router) {}

  login() {
    this.isLoggedIn.set(true);
    this.router.navigate(['/app/dashboard']);
  }

  logout() {
    this.isLoggedIn.set(false);
    this.router.navigate(['/']);
  }
}