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
var car_model_1 = require('../../../../models/car.model');
var car_service_1 = require('../../../../services/car.service');
var plate_validator_1 = require('../../../../validators/plate.validator');
var DetailCarAdminComponent = (function () {
    function DetailCarAdminComponent(carService) {
        this.carService = carService;
        this.formReset = true;
        this.carData = new car_model_1.Car();
        this.windowClose = new core_1.EventEmitter();
        this.updateList = new core_1.EventEmitter();
    }
    DetailCarAdminComponent.prototype.ngOnInit = function () {
        if (this.carId)
            this.getData();
    };
    DetailCarAdminComponent.prototype.getData = function () {
        var _this = this;
        this.carService.getCar(this.carId).subscribe(function (data) { _this.carData = data.json(); }, function (error) { return console.error(error); });
    };
    DetailCarAdminComponent.prototype.newCar = function () {
        var _this = this;
        var plate = this.carData.plate.toUpperCase();
        var name = this.carData.name;
        this.carService.addCar(JSON.stringify({ plate: plate, name: name })).subscribe(function (data) {
        }, function (error) {
            _this.error = error;
        }, function () {
            _this.success = 'Vozidlo úspešne vytvorené.';
            _this.carData = new car_model_1.Car();
            _this.formReset = false;
            setTimeout(function () { return _this.formReset = true; }, 0);
            _this.updateList.emit(true);
        });
    };
    DetailCarAdminComponent.prototype.editCar = function () {
        var _this = this;
        var id = this.carData.id;
        var plate = this.carData.plate.toUpperCase();
        var name = this.carData.name;
        this.carService.editCar(id, JSON.stringify({ id: id, name: name, plate: plate })).subscribe(function (data) {
        }, function (error) {
            _this.error = error;
        }, function () {
            _this.success = 'Vozidlo úspešne upravené.';
            _this.updateList.emit(true);
        });
    };
    DetailCarAdminComponent.prototype.closeWindow = function () {
        this.windowClose.emit(false);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], DetailCarAdminComponent.prototype, "carId", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailCarAdminComponent.prototype, "windowClose", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailCarAdminComponent.prototype, "updateList", void 0);
    DetailCarAdminComponent = __decorate([
        core_1.Component({
            selector: "car",
            templateUrl: 'app/components/admin/cars/detail/detail.car.admin.component.html',
            styleUrls: ['lib/css/modalWindow.css'],
            directives: [plate_validator_1.PlateUniquenessValidator, plate_validator_1.PlateValidator]
        }), 
        __metadata('design:paramtypes', [car_service_1.CarService])
    ], DetailCarAdminComponent);
    return DetailCarAdminComponent;
}());
exports.DetailCarAdminComponent = DetailCarAdminComponent;
