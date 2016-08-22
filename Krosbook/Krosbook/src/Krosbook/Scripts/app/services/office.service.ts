import {Injectable} from '@angular/core';
import {Http, Headers} from '@angular/http';
import 'rxjs/add/operator/map'
import {Observable} from "rxjs/Rx";


@Injectable()
export class OfficeService {
    hasRoleAdmin: boolean = false;

    constructor(private http: Http) {
        let _build = (<any>http)._backend._browserXHR.build;
        (<any>http)._backend._browserXHR.build = () => {
            let _xhr = _build();
            _xhr.withCredentials = true;
            return _xhr;
        };
    }

    public getOffices() {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/rooms', { headers });
    }

    public getOffice(id: number) { //with equipment
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/rooms/' + id, { headers });
    }

    public addOffice(office: string) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.post('/api/rooms', office, { headers });
    }

    public editOffice(id: string, office: string) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.put("/api/rooms/" + id, office, { headers });
    }

    public removeOffice(id: string) {
        return this.http.delete('/api/rooms/' + id);
    }

    public getEquipment() {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/equipment/', { headers });
    }

    public filterOffices(date: string, length: number, filterType: string) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.post('/api/rooms/filter', JSON.stringify({ date, length, filterType }), { headers });
    }
}
