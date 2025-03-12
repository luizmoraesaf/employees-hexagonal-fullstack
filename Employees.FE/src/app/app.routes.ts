import { Routes } from '@angular/router';
import { AuthenticationComponent } from './core/authentication/authentication.component';
import { AuthGuard } from './core/security/auth-guard.service';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, // Redirect root to login
  { path: 'login', component: AuthenticationComponent },
  {
    path: 'employees',
    loadComponent: () =>
      import('./pages/employees-list/employees-list.component').then(
        (m) => m.EmployeesListComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'create-employee',
    loadComponent: () =>
      import('./pages/create-employee/create-employee.component').then(
        (m) => m.CreateEmployeeComponent
      ),
    canActivate: [AuthGuard],
  },
  {
    path: 'edit-employee/:id',
    loadComponent: () =>
      import('./pages/create-employee/create-employee.component').then(
        (m) => m.CreateEmployeeComponent
      ),
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: 'login' }, // Redirect unknown routes to login
];
