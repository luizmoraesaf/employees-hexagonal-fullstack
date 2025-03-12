import { EmployeeRole } from './employee-role.enum';

export interface EmployeeDTO {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  documentNumber: string;
  birthDate: Date | string;
  password: string;
  phones: string[];
  managerId: string;
  role: EmployeeRole;
}
