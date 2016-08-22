import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map'
import {Observable} from "rxjs/Rx";

@Injectable()
export class RolesService {

  constructor(private http: Http) {
    let _build = (<any>http)._backend._browserXHR.build;
    (<any>http)._backend._browserXHR.build = () => {
      let _xhr = _build();
      _xhr.withCredentials = true;
      return _xhr;
    };
  }

  public getRoles(){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.get('/api/roles', { headers  } );
  }

  public getRole(id:number){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.get('/api/roles/'+id, { headers  } );
  }

  public addRole(role:string) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post('/api/roles',role, {headers});
  }

  public editRole(id:number, role:string){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.put("/api/roles/"+id, role, {headers});
  }

  public removeRole(id: string) {
    return this.http.delete('/api/roles/'+id);
  }


}



