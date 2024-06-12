import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagionation.model';
import { Observable, map } from 'rxjs';
import { UserParams } from '../_models/user-params.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = 'https://localhost:5001/api/';


  constructor(private http: HttpClient) { }


  getUsersWithRoles(userParams: UserParams) {
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    return this.getPaginatedResults<User[]>(this.baseUrl + 'admin/users-with-roles', params)
  }

  updateUserRoles(username: string, roles: string | any[]){
    return this.http.post<string[]>(this.baseUrl +'admin/edit-roles/' + username + '?roles=' + roles, {})
  }

  private getPaginatedResults<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        console.log('Headers:', response.headers.keys()); 
        response.headers.keys().forEach(key => {
          console.log(`${key}: ${response.headers.get(key)}`); 
        });
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        console.log('Pagination:', pagination); 
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      })
    );
  }
  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }
  
}
