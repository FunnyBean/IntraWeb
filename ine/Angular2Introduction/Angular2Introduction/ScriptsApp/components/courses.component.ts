import {Component} from '@angular/core';
import {CourseService} from './../services/course.service';
import {AutoGrowDirective} from './../directives/autoGrow.directive';
import {PersonList} from './personList.component';

//@Injectable()
@Component({
    selector: 'courses',
    templateUrl: `Home/CoursesComponent`,
    providers: [CourseService], //Dipendency Injection
    directives: [AutoGrowDirective, PersonList ],
    inputs: ['name']
})
export class CoursesComponent {
    public title: string = "Das ist der Title der Courses Page";
    public courses: string[] = [];
    public eingabeText: string = "TEST";

    constructor(private courseService: CourseService) {
        this.courses = courseService.getCourses();
    }
}