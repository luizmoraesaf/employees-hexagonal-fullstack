import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LoginRequestDto } from '../models/login-request-dto.model';
import { EmployeeDTO } from '../models/employee-dto.model';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private apiBaseUrl = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  authenticateUser(
    loginRequest: LoginRequestDto
  ): Observable<{ token: string; employee: EmployeeDTO }> {
    return this.http
      .post<{ token: string; employee: EmployeeDTO }>(
        `${this.apiBaseUrl}/authentication/login`,
        loginRequest
      )
      .pipe(
        map((response) => ({
          token: response.token,
          employee: response.employee,
        }))
      );
  }

  persistToken(token: string): void {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
