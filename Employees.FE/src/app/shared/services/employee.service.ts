import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmployeeDTO } from '../models/employee-dto.model';
import { Observable, shareReplay } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiBaseUrl = environment.apiBaseUrl;
  private employees$: Observable<EmployeeDTO[]> | null = null;

  constructor(private http: HttpClient) {}

  private fetchEmployees(): Observable<EmployeeDTO[]> {
    return this.http.get<EmployeeDTO[]>(`${this.apiBaseUrl}/employee`);
  }

  getEmployees(): Observable<EmployeeDTO[]> {
    if (!this.employees$) {
      this.employees$ = this.fetchEmployees().pipe(shareReplay(1));
    }
    return this.employees$;
  }

  refetchEmployees(): void {
    this.employees$ = this.fetchEmployees().pipe(shareReplay(1));
  }

  getEmployeeById(id: string): Observable<EmployeeDTO> {
    return this.http.get<EmployeeDTO>(`${this.apiBaseUrl}/employee/${id}`);
  }

  createEmployee(employeeDto: EmployeeDTO) {
    return this.http.post(`${this.apiBaseUrl}/employee`, employeeDto);
  }

  updateEmployee(id: string, employeeDto: EmployeeDTO): Observable<EmployeeDTO> {
    return this.http.put<EmployeeDTO>(`${this.apiBaseUrl}/employee/${id}`, employeeDto);
  }

  deleteEmployee(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiBaseUrl}/employee/${id}`);
  }
}
