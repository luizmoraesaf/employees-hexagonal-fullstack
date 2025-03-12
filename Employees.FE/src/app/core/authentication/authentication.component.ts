import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthenticationService } from '../../shared/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-authentication',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    CommonModule,
  ],
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.scss',
})
export class AuthenticationComponent {
  loginForm!: FormGroup;
  isLoading = false; // Show a spinner when logging in

  constructor(
    private fb: FormBuilder,
    private authenticationService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      documentNumber: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  login(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.authenticationService
        .authenticateUser(this.loginForm.value)
        .subscribe((response) => {
          this.isLoading = false;
          this.authenticationService.persistToken(response.token); // Save JWT token to local storage
          this.router.navigate(['/employees']); // Redirect to employees list
        });
    }
  }
}
