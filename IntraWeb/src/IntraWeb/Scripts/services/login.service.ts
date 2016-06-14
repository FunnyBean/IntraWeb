import {Http, Headers, Response, Request, RequestOptions} from 'angular2/http';
import {Injectable} from 'angular2/core';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class LoginService {

    constructor(public _http: Http) {
    }


    login(userName: string, password: string) {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        let x = this._http.post('/api/authentication/login', JSON.stringify({ userName: userName, password: password }), options);
    }

}
