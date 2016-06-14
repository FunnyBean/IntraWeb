import {Component} from 'angular2/core';
import {HTTP_PROVIDERS} from 'angular2/http';
import {Router, ROUTER_PROVIDERS} from 'angular2/router';

import {LoginService} from '../../services/login.service';

@Component({
    selector: 'login',
    templateUrl: 'app/components/login/login.component.html',
    styleUrls: ['app/components/login/login.component.css'],
    providers: [HTTP_PROVIDERS, ROUTER_PROVIDERS]
})
export class LoginComponent {

    constructor(private router: Router, private _loginService: LoginService) {
    }


    public userName: string = "gabo";
    public password: string = "21gabo12";
    public returnUrl: string = "http://google.com";

    onLoginClick() {
        var result = this._loginService.login(this.userName, this.password);
        //window.location.href = "http://azet.sk";
        this.router.navigate(["Dummy"]);
    }

}
