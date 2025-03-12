import { Component } from '@angular/core';
import { EmployeeDTO } from '../../shared/models/employee-dto.model';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormArray,
  ReactiveFormsModule,
} from '@angular/forms';
import { EmployeeRole } from '../../shared/models/employee-role.enum';
import { EmployeeService } from '../../shared/services/employee.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatOptionModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-create-employee',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatIconModule,
    MatSelectModule,
    MatOptionModule,
    CommonModule,
  ],
  templateUrl: './create-employee.component.html',
  styleUrl: './create-employee.component.scss',
})
export class CreateEmployeeComponent {
  employee: EmployeeDTO | null = null; // Para edição
  employees: EmployeeDTO[] = [];
  employeeForm!: FormGroup;
  isEditMode = false;

  employeeRoles = [
    { value: EmployeeRole.Employee, label: 'Employee' },
    { value: EmployeeRole.Leader, label: 'Leader' },
    { value: EmployeeRole.Director, label: 'Director' },
  ];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  findEmployeeById(id: string): void {
    this.employeeService.getEmployeeById(id).subscribe((employee) => {
      this.employee = employee;
      this.employee.birthDate = new Date(this.employee?.birthDate || '')
        .toISOString()
        .split('T')[0];
      this.employeeForm.patchValue(employee);
      this.employeeForm.setControl(
        'phones',
        this.fb.array(
          employee.phones.map((phone) =>
            this.fb.group({ phoneNumber: [phone, Validators.required] })
          )
        )
      );
    });
  }

  findAllEmployees(): void {
    this.employeeService.getEmployees().subscribe((employees) => {
      this.employees = employees;
    });
  }

  ngOnInit(): void {
    this.findAllEmployees();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.findEmployeeById(id);
    }

    this.isEditMode = !!id;

    this.employeeForm = this.fb.group({
      firstName: [this.employee?.firstName || '', Validators.required],
      lastName: [this.employee?.lastName || '', Validators.required],
      email: [
        this.employee?.email || '',
        [Validators.required, Validators.email],
      ],
      documentNumber: [
        {
          value: this.employee?.documentNumber || '',
          disabled: this.isEditMode,
        },
        Validators.required,
      ],
      birthDate: [this.employee?.birthDate, Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      phones: this.fb.array(
        (this.employee?.phones || []).map((phone) =>
          this.fb.group({ phoneNumber: [phone, Validators.required] })
        )
      ),
      managerId: [this.employee?.managerId || ''],
      role: [this.employee?.role || EmployeeRole.Employee, Validators.required],
    });
  }

  get phones(): FormArray {
    return this.employeeForm.get('phones') as FormArray;
  }

  addPhone(): void {
    this.phones.push(this.fb.group({ phoneNumber: [''] }));
  }

  removePhone(index: number): void {
    this.phones.removeAt(index);
  }

  refetchEmployeesAndReturnToHome(): void {
    this.employeeForm.reset();
    this.employeeService.refetchEmployees();
    this.router.navigate(['/employees']);
  }

  onSubmit(): void {
    if (this.employeeForm.invalid) return;

    const formValue = this.employeeForm.value;

    if (this.isEditMode) {
      this.employeeService
        .updateEmployee(this.employee!.id, {
          ...formValue,
          documentNumber: this.employee?.documentNumber,
          phones: formValue.phones.map((item: any) => item.phoneNumber),
        })
        .subscribe(() => {
          alert('Employee updated successfully!');
          this.refetchEmployeesAndReturnToHome();
        });
    } else {
      this.employeeService
        .createEmployee({
          ...formValue,
          phones: formValue.phones.map((item: any) => item.phoneNumber),
        })
        .subscribe(
          () => {
            alert('Employee created successfully!');
            this.refetchEmployeesAndReturnToHome();
          },
          (error) => this.catchErrors(error)
        );
    }
  }

  // this must be a interceptor
  catchErrors(error: any): void {
    if (error.error && typeof error.error === 'string') alert(error.error);
    else if (error.error && error.error.length > 1)
      alert(error.error[0].errorMessage);
  }

  onCancel(): void {
    this.router.navigate(['/employees']);
  }
}
