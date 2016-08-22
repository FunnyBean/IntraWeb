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
var office_admin_model_1 = require('../../../../models/office.admin.model');
var office_service_1 = require('../../../../services/office.service');
var equipment_component_1 = require('./equipment/equipment.component');
var officeName_validator_1 = require('../../../../validators/officeName.validator');
var DetailOfficeAdminComponent = (function () {
    function DetailOfficeAdminComponent(officeService) {
        this.officeService = officeService;
        this.formReset = true;
        this.officeData = new office_admin_model_1.Office();
        this.windowClose = new core_1.EventEmitter();
        this.updateList = new core_1.EventEmitter();
    }
    DetailOfficeAdminComponent.prototype.ngOnInit = function () {
        if (this.officeId)
            this.getData();
        else
            this.equipment = JSON.parse('[]');
    };
    DetailOfficeAdminComponent.prototype.getData = function () {
        var _this = this;
        this.officeService.getOffice(this.officeId).subscribe(function (data) { _this.officeData = data.json(), _this.equipment = data.json().equipment; }, function (error) { return console.error(error); });
    };
    DetailOfficeAdminComponent.prototype.newOffice = function () {
        var _this = this;
        var name = this.officeData.name;
        var type = this.officeData.type;
        var description = this.officeData.description;
        var color = this.officeData.color;
        var equipment = this.equipment;
        this.officeService.addOffice(JSON.stringify({ name: name, type: type, description: description, color: color, equipment: equipment })).subscribe(function (data) {
        }, function (error) {
            _this.error = error;
        }, function () {
            _this.success = 'Miestnosť úspešne vytvorená.';
            _this.officeData = new office_admin_model_1.Office();
            _this.formReset = false;
            setTimeout(function () { return _this.formReset = true; }, 0);
            _this.updateList.emit(true);
            //this.closeWindow();
        });
    };
    DetailOfficeAdminComponent.prototype.editOffice = function () {
        var _this = this;
        var id = this.officeData.id;
        var name = this.officeData.name;
        var type = this.officeData.type;
        var description = this.officeData.description;
        var color = this.officeData.color;
        var equipment = this.equipment;
        this.officeService.editOffice(id, JSON.stringify({ id: id, name: name, type: type, description: description, color: color, equipment: equipment })).subscribe(function (data) {
        }, function (error) {
            _this.error = error;
        }, function () {
            _this.success = 'Miestnosť úspešne upravená.';
            _this.updateList.emit(true);
            //this.closeWindow();
        });
    };
    DetailOfficeAdminComponent.prototype.closeWindow = function () {
        this.windowClose.emit(true);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], DetailOfficeAdminComponent.prototype, "officeId", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailOfficeAdminComponent.prototype, "windowClose", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailOfficeAdminComponent.prototype, "updateList", void 0);
    DetailOfficeAdminComponent = __decorate([
        core_1.Component({
            selector: "office",
            templateUrl: 'app/components/admin/offices/detail/detail.office.admin.component.html',
            styleUrls: ['lib/css/modalWindow.css'],
            directives: [equipment_component_1.EquipmentComponent, officeName_validator_1.OfficeNameValidator]
        }), 
        __metadata('design:paramtypes', [office_service_1.OfficeService])
    ], DetailOfficeAdminComponent);
    return DetailOfficeAdminComponent;
}());
exports.DetailOfficeAdminComponent = DetailOfficeAdminComponent;
