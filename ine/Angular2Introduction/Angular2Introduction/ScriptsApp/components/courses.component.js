"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var course_service_1 = require('./../services/course.service');
var autoGrow_directive_1 = require('./../directives/autoGrow.directive');
var personList_component_1 = require('./personList.component');
//@Injectable()
var CoursesComponent = (function () {
    function CoursesComponent(courseService) {
        this.courseService = courseService;
        this.title = "Das ist der Title der Courses Page";
        this.courses = [];
        this.eingabeText = "TEST";
        this.courses = courseService.getCourses();
    }
    CoursesComponent = __decorate([
        core_1.Component({
            selector: 'courses',
            templateUrl: "Home/CoursesComponent",
            providers: [course_service_1.CourseService],
            directives: [autoGrow_directive_1.AutoGrowDirective, personList_component_1.PersonList],
            inputs: ['name']
        }), 
        __metadata('design:paramtypes', [course_service_1.CourseService])
    ], CoursesComponent);
    return CoursesComponent;
}());
exports.CoursesComponent = CoursesComponent;
//# sourceMappingURL=courses.component.js.map