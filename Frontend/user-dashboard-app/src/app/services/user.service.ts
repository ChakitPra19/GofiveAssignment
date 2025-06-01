import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';
import { map, tap } from 'rxjs/operators';

interface DeleteUserResponse {
  status: {
    code: string;
    description: string;
  };
  data: {
    result: boolean;
    message: string;
  };
}

export interface Role {
  roleId: string;
  roleName: string;
}

interface RoleResponse {
  $id: string;
  code: string;
  description: string;
  data: {
    $id: string;
    $values: Role[];
  };
}

export interface AddUserDto {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  username: string;
  password: string;
  roleId: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/api`;
  private apiPaginatedUrl = 'http://localhost:5002/api/users/DataTable'; // New API endpoint for pagination
  private rolesSubject = new BehaviorSubject<Role[]>([]);
  roles$ = this.rolesSubject.asObservable();

  constructor(private http: HttpClient) {
    // Load roles when service is initialized
    this.loadRoles();
  }

  private loadRoles() {
    this.http.get<RoleResponse>(`${this.apiUrl}/roles`).pipe(
      map(response => response.data.$values)
    ).subscribe({
      next: (roles) => {
        this.rolesSubject.next(roles);
      },
      error: (error) => {
        console.error('Error loading roles:', error);
      }
    });
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Users`);
  }

  // New method for paginated users
  getPaginatedUsers(
    page: number,
    pageSize: number,
    orderBy?: string,
    orderDirection?: string,
    search?: string
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', page.toString())
      .set('pageSize', pageSize.toString());

    if (orderBy) {
      params = params.set('orderBy', orderBy);
    }
    if (orderDirection) {
      params = params.set('orderDirection', orderDirection);
    }
    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<any>(this.apiPaginatedUrl, { params });
  }

  getRoles(): Observable<Role[]> {
    return this.roles$;
  }

  addUser(user: AddUserDto): Observable<any> {
    return this.http.post(`${this.apiUrl}/Users`, user);
  }

  updateUser(id: string, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/Users/${id}`, user);
  }

  deleteUser(id: string): Observable<DeleteUserResponse> {
    return this.http.delete<DeleteUserResponse>(`${this.apiUrl}/Users/${id}`);
  }
}
