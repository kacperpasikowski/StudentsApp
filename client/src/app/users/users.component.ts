import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../modals/roles-modal/roles-modal.component';
import { Title } from '@angular/platform-browser';
import { Pagination } from '../_models/pagionation.model';
import { UserParams } from '../_models/user-params.model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit{

  constructor(private userService: UserService, private modalService: BsModalService){}
  users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  availableRoles = [
    'Admin',
    'Secretary',
    'Viewer',
    'Student'
  ];
  pagination: Pagination | undefined;
  userParams: UserParams ={
    pageNumber: 1,
    pageSize: 5
  };

  ngOnInit(): void {
    this.getUsersWithRoles();
  }


  getUsersWithRoles(){
    this.userService.getUsersWithRoles(this.userParams).subscribe({
      next: response => {
        if(response.result && response.pagination){
          this.users = response.result;
          this.pagination = response.pagination;
          
        }
      }
    })
  }

  openRolesModal(user: User){
    const config = {
      class: 'modal-dialog-centered',
      initialState:{
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.bsModalRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles!, user.roles)){
          this.userService.updateUserRoles(user.username, selectedRoles!).subscribe({
            next: roles => user.roles = roles,
            error: error => console.log(error)
          })
        }

      }
    })
  }

  private arrayEqual(arr1: any[], arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort())
  }
  pageChanged(event: any){
    if(this.userParams && this.userParams.pageNumber !== event.page){
      this.userParams.pageNumber = event.page;
      this.getUsersWithRoles();
    }
  }

}
