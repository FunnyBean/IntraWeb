import {Component} from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router';

import {UserService} from '../services/user.service';
import {OfficeService} from '../services/office.service';
import {CarService} from '../services/car.service';
import {EquipmentService} from '../services/equipment.service';
import {RolesService} from '../services/roles.service';

@Component({
    selector: 'app-root',
    template: '<router-outlet></router-outlet>',
    directives: [ROUTER_DIRECTIVES],
    providers: [
        UserService, OfficeService, CarService, EquipmentService, RolesService
    ]
})
export class AppComponent { }
