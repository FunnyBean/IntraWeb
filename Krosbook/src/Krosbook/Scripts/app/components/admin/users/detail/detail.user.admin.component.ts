/**
 * Created by Tibor Poštek on 18.07.2016.
 */
import {Component, OnInit, Input, Output, EventEmitter} from  '@angular/core';
import {UserService} from '../../../../services/user.service';
import {User} from "../../../../models/user.admin.model";
import {EmailValidator} from '../../../../validators/email.validator';
import {Role} from "../../../../models/role.admin.model";

@Component({
  selector: 'user',
  templateUrl: 'app/components/admin/users/detail/detail.user.admin.component.html',
  styleUrls: ['lib/css/modalWindow.css'],
  directives: [EmailValidator]
})

export class DetailUserAdminComponent implements OnInit {
  public error:string;
  public success:string;
  public formReset:boolean = true;
  public allRoles:Array<Role>;
  public checkedRoles:Array<boolean> = new Array();
  public userData:User = new User();

  @Input() userId:number;
  @Output() windowClose = new EventEmitter<boolean>();
  @Output() updateList = new EventEmitter<boolean>();

  constructor(private userService:UserService) { }


  ngOnInit() {
    if (this.userId) {
      this.getData();
    } else {
      this.getRoles();
    }
  }

  getData() {
    this.userService.getUser(this.userId).subscribe(
      data => {
        this.userData = data.json()
      },
      error => console.error(error),
      () => this.getRoles());
  }


  newUser() {
    let email = this.userData.email;
    let name = this.userData.name;
    let surname = this.userData.surname;
    let roles = JSON.parse("[]");
    for (var i = 0; i < this.allRoles.length; i++) {
      if (this.checkedRoles[i]) {
        roles.push({"roleId": this.allRoles[i].id});
      }
    }
    this.userService.addUser(JSON.stringify({email, name, surname, roles})).subscribe(
      data => { },
      error => { this.error = error; },
      () => {
        this.success = 'Užívateľ úspešne vytvorený.';
        this.userData = new User();
        this.formReset = false;
        setTimeout(() => this.formReset = true, 0);
        for (var i = 0; i < this.allRoles.length; i++)
          this.checkedRoles[i] = false;
        this.updateList.emit(true);
      }
    );
  }

  editUser() {
    let id = this.userData.id;
    let email = this.userData.email;
    let name = this.userData.name;
    let surname = this.userData.surname;
    let PhotoBase64 = this.userData.photoBase64;
    let roles = JSON.parse("[]");
    for (var i = 0; i < this.allRoles.length; i++) {
      if (this.checkedRoles[i]) {
        roles.push({"roleId": this.allRoles[i].id});
      }
    }
    this.userService.editUser(id, JSON.stringify({id, email, name, surname, roles, PhotoBase64})).subscribe(
      data => {
      },
      error => {
        this.error = error;
      },
      () => {
        this.success = 'Užívateľ úspešne upravený.';
        this.updateList.emit(true);
      }
    );

  }

  getRoles() {
    this.userService.getRoles().subscribe(
      data => {
        this.allRoles = data.json()
      },
      error => console.log(error),
      () => this.setUserRoles()
    );
  }

  setUserRoles() {
    if(this.userId) {
      for (var i = 0; i < this.allRoles.length; i++) {
        this.checkedRoles[i] = false;
        for (var j = 0; j < this.userData.roles.length; j++) {
          if (this.allRoles[i].id == this.userData.roles[j].roleId) {
            this.checkedRoles[i] = true;
          }
        }
      }
    }
  }

  closeWindow() {
    this.windowClose.emit(false);
  }
}
