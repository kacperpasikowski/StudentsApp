import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  model: any = {};
  loggedIn = false;
  currentUser$: Observable<User | null> = of(null)


  constructor(private accountService: AccountService,
      private router: Router, private toastr: ToastrService){}



  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next: user => this.loggedIn = !!user,
      
    })
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        this.router.navigateByUrl('/');
      },
      error: error => {
        console.log(error);
      }
      
    })
  }
  logout(){
    this.accountService.logout();
  }

}

