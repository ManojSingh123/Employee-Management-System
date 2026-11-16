import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { Employee } from '../../models/employee';
import { EmployeeService } from '../../services/employee';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './employee-dashboard.html',
  styleUrls: ['./employee-dashboard.css'], // fixed typo
})
export class EmployeeDashboard implements OnInit {

  employees: Employee[] = [];
  employee: Employee = { name: '', email: '', salary: 0 };
  totalEmployees: number = 0;
  currentPage: number = 1;
  pageSize: number = 2; // records per page
  isEditMode = false;
  role:string | null=null;
  private router = inject(Router);

  constructor(private employeeService: EmployeeService, private cd: ChangeDetectorRef, private authService:Auth) {}



  ngOnInit(): void {
    this.currentPage = 1;
    this.loadEmployees(this.currentPage);
    this.role = this.authService.getUserRole();
  }




  // Load employees with pagination
  loadEmployees(page: number = 1) {
    this.employeeService.getAll().subscribe({
      next: (res: Employee[]) => {
        this.totalEmployees = res.length;

        if (this.totalEmployees === 0) {
          this.employees = [];
          this.cd.detectChanges();
          return;
        }

        const totalPages = Math.ceil(this.totalEmployees / this.pageSize);
        if (page < 1) page = 1;
        if (page > totalPages) page = totalPages;

        this.currentPage = page;

        const startIndex = (this.currentPage - 1) * this.pageSize;
        const endIndex = startIndex + this.pageSize;

        // Slice employees for current page
        this.employees = res.slice(startIndex, endIndex);

        this.cd.detectChanges();
      },
      error: (err) => console.error(err)
    });
  }

  saveEmployee() {
    if (this.isEditMode && this.employee.id) {
      this.employeeService.update(this.employee).subscribe(() => {
        this.loadEmployees(this.currentPage);
        this.resetForm();
      });
    } else {
      this.employeeService.create(this.employee).subscribe(() => {
        this.loadEmployees(this.currentPage);
        this.resetForm();
      });
    }
  }

  editEmployee(emp: Employee) {
    this.employee = { ...emp };
    this.isEditMode = true;
    this.cd.detectChanges();
  }

  deleteEmployee(id?: number) {
    
    if (!id) return;
    if(this.role==='Admin')
    {
    if (confirm('Delete this employee?')) {
      this.employeeService.delete(id).subscribe(() => {
        const remainingItems = this.totalEmployees - 1;
        const totalPagesAfterDelete = Math.ceil(remainingItems / this.pageSize);
        if (this.currentPage > totalPagesAfterDelete) this.currentPage = totalPagesAfterDelete || 1;
        this.loadEmployees(this.currentPage);
      });
    }
  }
  else
  {
    alert('You dont have permission to delete');
  }
  }

  resetForm() {
    this.employee = { name: '', email: '', salary: 0 };
    this.isEditMode = false;
    this.cd.detectChanges();
  }

  LOGOUT() {
    this.authService.logout();
    // localStorage.removeItem('token');
    // sessionStorage.clear();
    // this.router.navigate(['/login']);
  }

  // Pagination helpers
  goToPage(page: number) {
    if (page < 1 || page > this.totalPages()) return;
    this.currentPage = page;
    this.loadEmployees(this.currentPage);
  }

  nextPage() {
    if (this.currentPage < this.totalPages()) this.goToPage(this.currentPage + 1);
  }

  prevPage() {
    if (this.currentPage > 1) this.goToPage(this.currentPage - 1);
  }

  totalPages(): number {
    return this.totalEmployees > 0 ? Math.ceil(this.totalEmployees / this.pageSize) : 1;
  }

  showPagination(): boolean {
    return this.totalEmployees > this.pageSize;
  }

  // Add/Update button disabled check (for template)
  isAddDisabled(): boolean {
    return !this.employee.name || !this.employee.email || this.employee.salary <= 0;
  }

  get pages(): number[] {
  return Array.from({ length: this.totalPages() }, (_, i) => i + 1);
}
}