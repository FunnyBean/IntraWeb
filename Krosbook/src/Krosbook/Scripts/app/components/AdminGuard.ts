/**
 * Created by Tibor Poštek on 15.07.2016.
 */
import {UserService} from '../services/user.service';
import {Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate} from '@angular/router';
import {Injectable} from "@angular/core";
import {Observable} from 'rxjs/Observable';

@Injectable()
export class AdminGuard implements CanActivate {
  private userRoles:any;

  constructor(public userService:UserService, public router:Router) {
    this.loadAuth();
  }

  canActivate(next: ActivatedRouteSnapshot,state: RouterStateSnapshot) {
    var result = this.loadAuth();
    return result;
  }

  loadAuth(){
    return Observable.create(observer => {
      this.userService.myProfile().map(res => res.json().roles).subscribe((result) => { 
        this.userRoles = result;
        var hasAdminRole:boolean = false;
        for(var i = 0; i < this.userRoles.length; i++)
        {
          if(this.userRoles[i].roleId == 1){
            hasAdminRole = true;
          }
        }
        if(hasAdminRole)
          observer.next(true);
        else {
          observer.next(false);
          this.router.navigate(['/home']);
        } 
        observer.complete();
      },
      error => { 
        observer.next(false);
        observer.complete();
      }
    )
    });
  }
}
