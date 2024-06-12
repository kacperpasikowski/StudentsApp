import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { User } from '../_models/user';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent implements OnInit {

  user: User | null = null;

  constructor(private accountService: AccountService, private router: Router){}
  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(user => {
      this.user = user;
    });
  }


  

}
