import {Component} from '@angular/core';
// mit "./" sucht er im aktuellen Verzeichnis nach der Komponente
import {CoursesComponent} from './components/courses.component';
import {CourseService} from './services/course.service';

@Component({
    selector: 'my-app',
    templateUrl: 'Home/MainComponent',
    providers: [CourseService], //Dipendency Injection
    directives: [CoursesComponent]
})
export class AppComponent {
    constructor(courseService: CourseService) {

    }



} 

