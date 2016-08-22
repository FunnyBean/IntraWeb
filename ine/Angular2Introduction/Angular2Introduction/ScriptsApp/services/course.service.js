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
var http_1 = require('@angular/http');
require('rxjs/add/operator/map'); //Notwendig damit wird weiter unten auf den map Operator zugreifen können
var CourseService = (function () {
    function CourseService(_http) {
        this._http = _http;
    }
    CourseService.prototype.getCourses = function () {
        var courses = ["Course 1", "Course 2", "Course 3"];
        return courses;
    };
    CourseService.prototype.getPersons = function (name) {
        return this._http.get("Home/GetPersons?name=" + name).map(function (res) { return res.json(); });
    };
    CourseService = __decorate([
        //Notwendig damit wird weiter unten auf den map Operator zugreifen können
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], CourseService);
    return CourseService;
}());
exports.CourseService = CourseService;
//# sourceMappingURL=course.service.js.map