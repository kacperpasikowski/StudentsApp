import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {


  user: User | null = null;

  constructor(private accountService: AccountService, private router: Router){}
  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(user => {
      this.user = user;
    });
  }

}
