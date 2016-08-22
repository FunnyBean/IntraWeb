import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {UserService} from '../../services/user.service';
import {Cookie} from 'ng2-cookies/ng2-cookies';

@Component({
    selector: 'login',
    templateUrl: 'app/components/login/login.component.html',
    styleUrls: ['app/components/login/login.component.css']

})
export class LoginComponent {
  private email: string;
  private password: string;
  private error: string;
  private year = '2016';
  private rememberActive:boolean = false;

  constructor(private router: Router, private userService: UserService) {
    if(Cookie.get("RememberMe") != null){
      var loginString = Cookie.get("RememberMe").split(":");
      this.userService.loginWithCookie(loginString[0], loginString[1]).subscribe(
        response => {
          this.router.navigate(['/']);
        },
        error => {
          Cookie.delete('RememberMe');
          this.error = 'Nesprávne údaje na automatické prihlásenie';
          console.log(error.text());
        }
      );
    }
  }

  onSubmit() {
    this.userService.login(this.email, this.password, this.rememberActive).subscribe(
      response => {
        this.router.navigate(['/']);
      },
      error => {
        this.error = 'Nesprávny email/heslo';
        console.log(error.text());
      }
    );
  }
}
