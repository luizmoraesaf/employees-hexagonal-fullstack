import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EmployeeService } from '../../shared/services/employee.service';
import { EmployeeDTO } from '../../shared/models/employee-dto.model';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule, DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  imports: [
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    DatePipe,
    CommonModule,
  ],
  styleUrl: './employees-list.component.scss',
})
export class EmployeesListComponent implements OnInit {
  employees: EmployeeDTO[] = [];

  constructor(
    private router: Router,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data;
    });
  }

  create(): void {
    this.router.navigate(['/create-employee']);
  }

  editEmployee(employeeId: string): void {
    this.router.navigate(['/edit-employee', employeeId]);
  }

  deleteEmployee(employeeId: string): void {
    this.employeeService.deleteEmployee(employeeId).subscribe(() => {
      this.employees = this.employees.filter((employee) => employee.id !== employeeId);
    });
  }

  navigateToCreateEmployee(): void {
    this.router.navigate(['/create-employee']);
  }

  logout(): void {
    localStorage.removeItem('token'); // Remove JWT token
    this.router.navigate(['/login']); // Redirect to login
  }
}
