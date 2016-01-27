import {Component} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES} from 'angular2/router';

import {LoginComponent} from './login/login.component';

@Component({
    selector: 'app',
    templateUrl: 'app/components/app.component.html',
    styleUrls: ['app/components/app.component.css'],
    directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([
    {path: '/login', name: 'Login', component: LoginComponent, useAsDefault: true}
])
export class AppComponent { }

