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

        let x = this._http.post('/api/authentication/login', JSON.stringify({ userName: userName, password: password }), options)
            .subscribe(this.extractData);
        console.info(x);
    }

    private extractData(res: Response) {
        //if (res.status < 200 || res.status >= 300) {
        //    throw new Error('Bad response status: ' + res.status);
        //}
        //let body = res.json();
        //return body.data || {};
    }


}
