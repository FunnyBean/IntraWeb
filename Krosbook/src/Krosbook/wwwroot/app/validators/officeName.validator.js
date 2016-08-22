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
var office_service_1 = require('../services/office.service');
var OfficeNameValidator = (function () {
    function OfficeNameValidator(officeService) {
        var _this = this;
        this.officeService = officeService;
        this.initialCValue = null;
        this.officeService.getOffices().subscribe(function (data) { _this.offices = data.json(); }, function (error) { return console.log(error); });
    }
    OfficeNameValidator.prototype.validate = function (c) {
        //nastavenia pre správne fungovanie editovania - nahratie prvej hodnoty do premennej - ak je nová miestnosť tak NULL
        if (this.initialCValue == null && c.value != null)
            this.initialCValue = c.value;
        if (c.value == null)
            this.initialCValue = null;
        if (this.initialCValue == c.value)
            return null;
        if (this.offices) {
            for (var i = 0; i < this.offices.length; i++) {
                if (this.offices[i].name == c.value)
                    return { validateOfficeName: true };
            }
        }
        return null;
    };
    OfficeNameValidator = __decorate([
        core_1.Directive({
            selector: '[validateOfficeName][ngModel],[validateOfficeName][formControl]',
            providers: [
                { provide: forms_1.NG_VALIDATORS, useExisting: core_1.forwardRef(function () { return OfficeNameValidator; }), multi: true }, office_service_1.OfficeService
            ]
        }), 
        __metadata('design:paramtypes', [office_service_1.OfficeService])
    ], OfficeNameValidator);
    return OfficeNameValidator;
}());
exports.OfficeNameValidator = OfficeNameValidator;
