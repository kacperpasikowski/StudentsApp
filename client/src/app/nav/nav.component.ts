import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { Observable, of } from 'rxjs';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  loggedInUser: User | null = null;
  currentUser$: Observable<User | null> = of(null)
  userPhotoUrl: SafeUrl | null = null;
  isAdmin$: Observable<boolean> = of(false);

  constructor(private accountService: AccountService, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.isAdmin$ = this.accountService.isAdmin();
    this.currentUser$.subscribe(user => {
      if (user) {
        console.log(user.id);
        this.loggedInUser = user;
        this.loadUserPhoto(user.email);
        
      }
    });
  }




  logout() {
    this.accountService.logout();
  }


  loadUserPhoto(userEmail: string): void {
    this.accountService.getUserPhoto(userEmail).subscribe({
      next: photoBlob => {
        const imageUrl = this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(photoBlob));
        this.userPhotoUrl = imageUrl;
        if(this.loggedInUser){
          this.loggedInUser.avatarUrl = imageUrl;
        }

      },
      error: error => console.log(error)
    })
      
  }
}
