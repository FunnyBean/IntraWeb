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
var car_service_1 = require('../../../services/car.service');
var detail_car_admin_component_1 = require('./detail/detail.car.admin.component');
var CarsAdminComponent = (function () {
    function CarsAdminComponent(carService) {
        this.carService = carService;
        this.showCarWindow = false;
    }
    CarsAdminComponent.prototype.ngOnInit = function () {
        this.getCars();
    };
    CarsAdminComponent.prototype.getCars = function () {
        var _this = this;
        this.carService.getCars()
            .subscribe(function (data) { _this.cars = data.json(); }, function (error) { return console.error(error); });
    };
    CarsAdminComponent.prototype.deleteCar = function (id) {
        var _this = this;
        if (confirm("Naozaj chcete vymazať vozidlo?")) {
            this.carService.removeCar(id).subscribe(function (data) { }, function (error) { alert(error); }, function () { _this.getCars(); });
        }
    };
    CarsAdminComponent.prototype.newCar = function () {
        this.carId = null;
        this.windowOpen();
    };
    CarsAdminComponent.prototype.editCar = function (id) {
        for (var i = 0; i < this.cars.length; i++) {
            if (this.cars[i].id == id) {
                this.carId = this.cars[i].id;
                break;
            }
        }
        this.windowOpen();
    };
    //opens detail.office.admin.component window
    CarsAdminComponent.prototype.windowOpen = function () {
        this.showCarWindow = true;
    };
    //closes detail.office.admin.component window
    CarsAdminComponent.prototype.windowClose = function (action) {
        this.showCarWindow = action;
    };
    CarsAdminComponent = __decorate([
        core_1.Component({
            selector: 'cars-admin',
            templateUrl: 'app/components/admin/cars/cars.admin.component.html',
            directives: [detail_car_admin_component_1.DetailCarAdminComponent]
        }), 
        __metadata('design:paramtypes', [car_service_1.CarService])
    ], CarsAdminComponent);
    return CarsAdminComponent;
}());
exports.CarsAdminComponent = CarsAdminComponent;
