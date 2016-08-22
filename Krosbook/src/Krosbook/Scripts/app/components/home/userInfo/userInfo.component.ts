import {Component, Input} from '@angular/core';
import {Router, ROUTER_DIRECTIVES} from '@angular/router';
import {UserService} from '../../../services/user.service';
import {User} from '../../../models/user.admin.model';
import {Cookie} from 'ng2-cookies/ng2-cookies';

//import {ProfileComponent} from "../profile/profile.component";

@Component({
  selector: 'user_info',
  templateUrl: 'app/components/home/userInfo/userInfo.component.html',
  styleUrls: ['app/components/home/userInfo/userInfo.component.css'],
  directives: [ROUTER_DIRECTIVES]

})
export class UserInfoComponent {
  @Input() userData;
  @Input() isAdmin;

  constructor(private router:Router, private userService:UserService) { }


  ngOnInit(){
    this.hideUserInfo();
  }

  logout() {
    this.userService.logout()
      .subscribe(
        data => {
          this.router.navigate(['/login']);
        },
        error => {
          console.log('Nepodarilo sa odhlásiť, pravdepodobne nastala chyba servera!');
          Cookie.delete("KrosbookAuth");
          this.router.navigate(['/login']);
        }
      );
  }

  hideUserInfo(){   
    document.getElementById('userInfo').style.display='none';
  }


}
