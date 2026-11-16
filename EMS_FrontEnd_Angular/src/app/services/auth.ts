import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private apiUrl = 'http://localhost:7004/api/auth';

  private tokenKey = 'token';

  constructor(private http: HttpClient, private router: Router) {}

  // 🔹 Login request
  login(data: { username: string; password: string }) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, data);
  }

 // 🔹 Get token
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  // 🔹 Save token to localStorage
  saveToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

 

  // 🔹 Decode token and get role
  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      // decode JWT payload safely
      const payloadBase64 = token.split('.')[1];
      const payloadJson = atob(payloadBase64);
      const payload = JSON.parse(payloadJson);
      return payload['role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null|| null; // backend me role claim check
    } catch (e) {
      console.error('Invalid token', e);
      return null;
    }
  }

  // 🔹 Check if logged in
  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const role = this.getUserRole();
    return !!role; // true if role exists
  }

  // 🔹 Logout
  logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }
}