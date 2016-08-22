import {Component, OnInit} from  '@angular/core';
import {RolesService} from '../../../services/roles.service';
import {Role} from '../../../models/role.admin.model';
import {DetailRoleAdminComponent} from './detail/detail.role.admin.component';

@Component({
  templateUrl: 'app/components/admin/roles/roles.admin.component.html',
  directives: [DetailRoleAdminComponent]
})

export class RolesAdminComponent implements OnInit{
  public roles:Array<Role>;
  public roleId:number;
  private showRoleWindow:boolean = false;

  constructor(private rolesService:RolesService){ }

  ngOnInit(){
    this.getRoles();
  }

  getRoles() {
    this.rolesService.getRoles()
      .subscribe(
        data => {this.roles = data.json()},
        error => console.error(error)
      );
  }

  deleteRole(id:string){
    if(confirm("Naozaj chcete vymazať túto rolu?")) {
      this.rolesService.removeRole(id).subscribe(
        data => { },
        error => { alert(error) },
        () => { this.getRoles() }
      )
    }
  }

  newRole() {
    this.roleId = null;
    this.windowOpen();
  }

  editRole(id:number){
    for(var i = 0; i < this.roles.length; i++){
      if(this.roles[i].id == id){
        this.roleId = this.roles[i].id;
        break;
      }
    }
    this.windowOpen();
  }

  windowOpen(){
    this.showRoleWindow = true;
  }

  windowClose(action: boolean){
    this.showRoleWindow = action;
  }
}
