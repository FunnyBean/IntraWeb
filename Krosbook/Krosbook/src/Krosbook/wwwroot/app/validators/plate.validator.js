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
var forms_1 = require('@angular/forms');
var car_service_1 = require('../services/car.service');
var PlateUniquenessValidator = (function () {
    function PlateUniquenessValidator(carService) {
        var _this = this;
        this.carService = carService;
        this.initialCValue = null;
        this.carService.getCars().subscribe(function (data) { _this.cars = data.json(); }, function (error) { return console.log(error); });
    }
    PlateUniquenessValidator.prototype.validate = function (c) {
        var plate;
        if (c.value != null)
            plate = c.value.toUpperCase();
        //nastavenia pre správne fungovanie editovania - nahratie prvej hodnoty do premennej - ak je nová miestnosť tak NULL
        if (this.initialCValue == null && plate != null)
            this.initialCValue = plate;
        if (plate == null)
            this.initialCValue = null;
        if (this.initialCValue == plate)
            return null;
        if (this.cars) {
            for (var i = 0; i < this.cars.length; i++) {
                if (this.cars[i].plate == plate)
                    return { validatePlateUniqueness: true };
            }
        }
        return null;
    };
    PlateUniquenessValidator = __decorate([
        core_1.Directive({
            selector: '[validatePlateUniqueness][ngModel],[validatePlateUniqueness][formControl]',
            providers: [
                { provide: forms_1.NG_VALIDATORS, useExisting: core_1.forwardRef(function () { return PlateUniquenessValidator; }), multi: true }, car_service_1.CarService
            ]
        }), 
        __metadata('design:paramtypes', [car_service_1.CarService])
    ], PlateUniquenessValidator);
    return PlateUniquenessValidator;
}());
exports.PlateUniquenessValidator = PlateUniquenessValidator;
var PlateValidator = (function () {
    function PlateValidator() {
    }
    PlateValidator.prototype.validate = function (c) {
        var plate;
        if (c.value != null)
            plate = c.value.toUpperCase();
        if (/^[A-Z]{2}?-?\d{3}[A-Z]{2}$/.test(plate))
            return null;
        else
            return { validatePlate: true };
    };
    PlateValidator = __decorate([
        core_1.Directive({
            selector: '[validatePlate][ngModel],[validatePlate][formControl]',
            providers: [
                { provide: forms_1.NG_VALIDATORS, useExisting: core_1.forwardRef(function () { return PlateValidator; }), multi: true }, car_service_1.CarService
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], PlateValidator);
    return PlateValidator;
}());
exports.PlateValidator = PlateValidator;
