import {Component} from '@angular/core';
import {CourseService} from './../services/course.service';
import {Person} from './../customDataClasses/Person'

//@Injectable()
@Component({
    selector: 'person-list',
    templateUrl: `Home/PersonList`,
    providers: [CourseService], //Dipendency Injection
})
export class PersonList {
    public persons: Person[];

    constructor(private courseService: CourseService) { }

    public getPersons() {
        //Unseren Service aufrufen und die Daten abrufen
        this.courseService.getPersons("test").subscribe( data => this.persons = data );
    }
}