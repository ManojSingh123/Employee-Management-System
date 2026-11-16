import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'http://localhost:7004/api/employee';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }



  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  getById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  create(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee, { headers: this.getHeaders() });
  }

  // update(employee: Employee): Observable<Employee> {
  //   return this.http.put<Employee>(
  //     `${this.apiUrl}/${employee.id}`,
  //     employee,
  //     { headers: this.getHeaders() }
  //   );
  // }

  update(employee: Employee): Observable<Employee> {
  return this.http.put<Employee>(`${this.apiUrl}/${employee.id}`, employee, { headers: this.getHeaders() });
}

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}