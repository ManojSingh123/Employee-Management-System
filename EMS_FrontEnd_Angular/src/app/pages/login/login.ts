import { Component } from '@angular/core';
import { Auth } from '../../services/auth';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class Login {

  loginData = {
    username: '',
    password: ''
  };

  errorMessage = '';

  constructor(private authService: Auth, private router: Router) {}

  onLogin(form: NgForm) {

    // ✅ Check if form is valid
    if (!form.valid) {
      // Mark all fields as touched to show validation messages
      form.control.markAllAsTouched();
      return;
    }

    // Call API if form is valid
    this.authService.login(this.loginData).subscribe({
      next: (res: any) => {

        this.authService.saveToken(res.token);

        const role = res.role;

        if (role === 'Admin') {
          this.router.navigate(['/admin']);
        } else if (role === 'Employee') {
          this.router.navigate(['/employee']);
        }
      },
      error: () => {
        this.errorMessage = 'Invalid Username or Password';
      }
    });
  }

}