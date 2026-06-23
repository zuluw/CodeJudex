import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthService);
  
  public isLoading = signal(false);

  public registerForm = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required, 
      Validators.minLength(6),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z]).*$/) 
    ]],
    role: [1]
  });

  public onRegister(): void {
    if (this.registerForm.invalid) return;

    this.isLoading.set(true);
    this.auth.register(this.registerForm.value).subscribe({
      next: () => this.isLoading.set(false),
      error: (err) => {
        this.isLoading.set(false);
        alert(err.error?.message || err.error?.Message || 'Registration failed');
      }
    });
  }
}