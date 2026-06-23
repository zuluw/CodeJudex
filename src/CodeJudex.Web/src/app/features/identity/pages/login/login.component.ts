import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  public isLoading = signal(false);

  public loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  public onLogin(): void {
    if (this.loginForm.invalid) return;

    this.isLoading.set(true);
    this.auth.login(this.loginForm.value).subscribe({
      next: () => this.isLoading.set(false),
      error: (err) => {
        this.isLoading.set(false);
        alert(err.error?.message || err.error?.Message || 'Invalid credentials');
      }
    });
  }
}