/**
 * Created by Ondrej on 01.08.2016.
 */
import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map'
import {Observable} from "rxjs/Rx";

@Injectable()
export class ReservationService {

  constructor(private http: Http) {
    let _build = (<any>http)._backend._browserXHR.build;
    (<any>http)._backend._browserXHR.build = () => {
      let _xhr = _build();
      _xhr.withCredentials = true;
      return _xhr;
    };
  }

  public getReservation(type:string, id:number){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.get('/api/reservations/'+type+'/'+id, {headers});
  }

  public getReservations(type:string, id:number, from:any, to:any){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    var by = (type == "rooms") ? "byroom" : "bycar";
    return this.http.post('/api/reservations/'+type+'/'+by+'/'+id, JSON.stringify({from, to}), {headers});
  }

  public addReservation(type:string, elementId: number, userId:number, name:string, date:string, length:number){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    if(type == "rooms"){
      var roomId = elementId;
      return this.http.post('/api/reservations/'+type+'/', JSON.stringify({roomId, name, date, length}), {headers});
    } else {
      var carId = elementId;
      return this.http.post('/api/reservations/'+type+'/', JSON.stringify({carId, name, date, length}), {headers});
    }
  }

  public editReservation(type: string, id: number, name: string, elementId: number, userId: number, dateTime:any, length:number, emailInvitation:boolean, goToMeeting:boolean){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    if(type == "rooms"){
        var roomId = elementId;
        return this.http.put('/api/reservations/' + type + '/' + id, JSON.stringify({ id, name, roomId, dateTime, userId, length, emailInvitation, goToMeeting}), {headers});
    } else {
      var carId = elementId;
      return this.http.put('/api/reservations/' + type + '/' + id, JSON.stringify({ id, name, carId, dateTime, userId, length}), {headers});
    }
  }

  public deleteReservation(type:string, id:number){
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.delete('/api/reservations/'+type+'/'+id, {headers});
  }
}
