/**
 * Created by krosaci on 26.7.2016.
 */
import {Component, AfterContentInit} from '@angular/core';

import {User} from "../../../../models/user.admin.model";
import {UserService} from '../../../../services/user.service';
import {EqualValidator} from '../../../../validators/equal.validator';

declare var $:any;

@Component({
  selector: 'password',
  templateUrl: 'app/components/home/profile/password/password.component.html',
  styleUrls: ['../../../../lib/css/modalWindow.css'],
  directives: [EqualValidator]
})

export class PasswordComponent implements AfterContentInit{
  private passwordData:Array<string> = ['', '', ''];
  public error:string;
  public success:string;

  constructor(private userService:UserService) { }

  ngAfterContentInit() {
    $(".active").removeClass("active");
    $("#password").addClass("active");
  }

  updatePasswordData(col, value){
    this.passwordData[col] = value;
    console.log(this.passwordData);
  }

  savePassword(){
      this.userService.updatePassword(this.passwordData[0], this.passwordData[1]).subscribe(
          data => { this.success = "Heslo bolo úspešne zmenené."; $("input").val(''); },
          error => { this.error = "Staré heslo sa nezhoduje so záznamom v databáze."; }
      );
  }

}


