/**
 * Created by Tibor Poštek on 14.07.2016.
 */
import {UserService} from '../services/user.service';
import {Router, CanActivate} from '@angular/router';
import {Injectable} from "@angular/core";
import {Observable} from 'rxjs/Observable';


@Injectable()
export class AuthGuard implements CanActivate {

    constructor(public userService: UserService, public router: Router) { }

    canActivate() {
        if (this.userService.isLoggedIn()) {
            return true;
        } else {
            this.router.navigate(['/login']);
            return false;
        }
        /*return Observable.create(observer => {
          this.userService.isLoggedIn().map(res => res).subscribe((result) => { 
            if(result)
              observer.next(true);
            else {
              observer.next(false);
              this.router.navigate(['/login']);
            } 
            observer.complete();
          },
          error => { 
            observer.next(false);
            observer.complete();
            this.router.navigate(['/login']);
          }
        )
        });;  */
    }

}
