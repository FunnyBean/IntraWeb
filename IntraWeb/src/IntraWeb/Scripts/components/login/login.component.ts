import {Component} from 'angular2/core';
import {HTTP_PROVIDERS} from 'angular2/http';

import {LoginService} from '../../services/login.service';

@Component({
    selector: 'login',
    templateUrl: 'app/components/login/login.component.html',
    styleUrls: ['app/components/login/login.component.css'],
    providers: [HTTP_PROVIDERS]
})
export class LoginComponent {

    constructor(private _loginService: LoginService) {
    }


    public userName: string;
    public password: string;


    onLoginClick() {
        var result = this._loginService.login(this.userName, this.password);
    }

}
