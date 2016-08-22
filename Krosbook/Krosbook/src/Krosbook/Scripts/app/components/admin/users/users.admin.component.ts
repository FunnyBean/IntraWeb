import {Component, OnInit} from  '@angular/core';
import {ROUTER_DIRECTIVES, Router} from '@angular/router';
import {UserService} from '../../../services/user.service';
import {DetailUserAdminComponent} from './detail/detail.user.admin.component';
import {User} from '../../../models/user.admin.model';


@Component({
  templateUrl: 'app/components/admin/users/users.admin.component.html',
  directives: [DetailUserAdminComponent]
})

export class UsersAdminComponent implements OnInit {
  public users:Array<User>;
  public userId:number;
  private showOfficeWindow:boolean  = false;

  constructor(private router:Router, private userService:UserService) {
    this.GetUsers;
  }

  ngOnInit() {
    this.GetUsers();
  }

  GetUsers() {
    this.userService.getUsers()
      .subscribe(
        data => {
          this.users = data.json()
        },
        error => console.error(error)
      );
  }

  newUser(){
    this.userId = null;
    this.windowOpen();
  }

  editUser(id:number){
    for(var i = 0; i < this.users.length; i++){
      if(this.users[i].id == id){
        this.userId = this.users[i].id;
        break;
      }
    }
    this.windowOpen();
  }

  removeUser(id:string) {
    if(confirm("Naozaj chcete vymazať používateľa?")) {
      this.userService.removeUser(id).subscribe(
        data => {
        },
        error => {
          alert(error)
        },
        () => {
          this.GetUsers();
        }
      )
    }
  }

  //opens detail.office.admin.component window
  windowOpen(){
    this.showOfficeWindow = true;
  }

  //closes detail.office.admin.component window
  windowClose(action: boolean){
    this.showOfficeWindow = action;
  }


}
