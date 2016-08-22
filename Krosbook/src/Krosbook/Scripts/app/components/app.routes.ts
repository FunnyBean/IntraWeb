import {ProfileComponent} from './home/profile/profile.component';
import {provideRouter, RouterConfig}  from '@angular/router';
import {LoginComponent} from './login/login.component';
import {HomeComponent} from './home/home.component';
import {AdminComponent} from './admin/admin.component';
import {authProviders }      from './login.routes';
import {AuthGuard} from "./AuthGuard";
import {AdminGuard} from "./AdminGuard";

import {UsersAdminComponent} from './admin/users/users.admin.component';
import {OfficesAdminComponent} from './admin/offices/offices.admin.component';
import {CarsAdminComponent} from "./admin/cars/cars.admin.component";
import {EquipmentAdminComponent} from "./admin/equipment/equipment.admin.component";
import {RolesAdminComponent} from './admin/roles/roles.admin.component';

import {ReservationsComponent} from './home/reservations/reservations.component';
import {AvatarComponent} from "./home/profile/avatar/avatar.component";
import {PasswordComponent} from "./home/profile/password/password.component";


const routes: RouterConfig = [
  { path: '', redirectTo: 'home', terminal: true},
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      {
        path:'profile',component:ProfileComponent,

        children:[
          {
            path:'',
            component:AvatarComponent
          },
          {
            path:'avatar',
            component:AvatarComponent
          },
          {
            path:'password',
            component:PasswordComponent
          }
        ]
      },
      {
        path: '',
        component: ReservationsComponent
      },
      {
        path: 'reservations/:type',
        component: ReservationsComponent
      }
    ]
  },
  { path: 'login', component: LoginComponent },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard, AdminGuard],
    children: [
      {
        path: '',
        component: UsersAdminComponent
      },
      {
        path: 'offices',
        component: OfficesAdminComponent
      },
      {
        path: 'cars',
        component: CarsAdminComponent
      },
      {
        path: 'users',
        component: UsersAdminComponent
      },
      {
        path: 'equipment',
        component: EquipmentAdminComponent
      },
      {
        path: 'roles',
        component: RolesAdminComponent
      }
    ]
  }
];

export const APP_ROUTER_PROVIDERS = [
  provideRouter(routes), authProviders
];
