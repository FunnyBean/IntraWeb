import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/observable';
import {Person} from './../customDataClasses/Person'
import 'rxjs/add/operator/map'; //Notwendig damit wird weiter unten auf den map Operator zugreifen können

@Injectable()
export class CourseService {
    constructor(private _http: Http) { }

    public getCourses(): string[] {
        let courses: string[] = ["Course 1", "Course 2", "Course 3"];
        return courses;
    }

    getPersons(name: string) : Observable<Person[]>  {
        return this._http.get(`Home/GetPersons?name=${name}`).map((res : Response) => res.json());
    }
}