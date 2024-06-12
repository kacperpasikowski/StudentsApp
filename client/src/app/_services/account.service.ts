import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, throwError } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }
  loggedIn = false;

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'auth/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user)
        }
      })
    )
  }
  register(model: any): Observable<User>{
    return this.http.post<User>(this.baseUrl + 'auth/register', model).pipe(
      map((response: User) => {
        const user = response;
        if (user){
          this.setCurrentUser(user)
          return response;
        }
        throw new Error('Brak danych użytkownika w odpowiedzi HTTP.');
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role; // Pobranie ról z dekodowanego tokenu
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user' , JSON.stringify(user));
    this.currentUserSource.next(user); 
    this.getUserPhoto(user.id);
    console.log(roles);
}
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/')
    
  }

  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]));
  }
  getUserPhoto(userEmail: string): Observable<any> {
    return this.http.get(this.baseUrl + `users/${userEmail}/photo`, { responseType: 'blob' });
  }
  isAdmin(): Observable<boolean> {
    return this.currentUser$.pipe(
      map(user => user?.roles.includes('Admin') ?? false)
    );
  }

}
