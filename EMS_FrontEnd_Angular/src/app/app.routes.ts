import { Routes } from '@angular/router';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { authGuard } from './guards/auth-guard';
import { Login } from './pages/login/login';
import { EmployeeDashboard } from './pages/employee-dashboard/employee-dashboard';

export const routes: Routes = 
[
     { path: 'login', component: Login },

     { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'admin',
    component: AdminDashboard,
    canActivate: [authGuard],
    data: { role: 'Admin' }
  },

  {
    path: 'employee',
    component: EmployeeDashboard,
    canActivate: [authGuard],
    data: { role: 'Employee' }
  },
];
