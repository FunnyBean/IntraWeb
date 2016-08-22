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
var equipment_admin_model_1 = require('../../../../models/equipment.admin.model');
var equipment_service_1 = require('../../../../services/equipment.service');
var DetailEquipmentAdminComponent = (function () {
    function DetailEquipmentAdminComponent(equipmentService) {
        this.equipmentService = equipmentService;
        this.formReset = true;
        this.equipmentData = new equipment_admin_model_1.Equipment();
        this.windowClose = new core_1.EventEmitter();
        this.updateList = new core_1.EventEmitter();
    }
    DetailEquipmentAdminComponent.prototype.ngOnInit = function () {
        if (this.equipmentId)
            this.getEquipment();
    };
    DetailEquipmentAdminComponent.prototype.getEquipment = function () {
        var _this = this;
        this.equipmentService.getEquipment(this.equipmentId).subscribe(function (data) { _this.equipmentData = data.json(); }, function (error) { return console.log(error); });
    };
    DetailEquipmentAdminComponent.prototype.newEquipment = function () {
        var _this = this;
        var description = this.equipmentData.description;
        var amount = this.equipmentData.amount;
        this.equipmentService.addEquipment(JSON.stringify({ description: description, amount: amount })).subscribe(function (data) { }, function (error) { _this.error = error; }, function () {
            _this.success = 'Vybavenie úspešne pridané.';
            _this.equipmentData = new equipment_admin_model_1.Equipment();
            _this.formReset = false;
            setTimeout(function () { return _this.formReset = true; }, 0);
            _this.updateList.emit(true);
        });
    };
    DetailEquipmentAdminComponent.prototype.editEquipment = function () {
        var _this = this;
        var id = this.equipmentData.id;
        var description = this.equipmentData.description;
        var amount = this.equipmentData.amount;
        this.equipmentService.editEquipment(id, JSON.stringify({ id: id, description: description, amount: amount })).subscribe(function (data) { }, function (error) { _this.error = error; }, function () {
            _this.success = 'Vybavenie úspešne upravené.';
            _this.updateList.emit(true);
        });
    };
    DetailEquipmentAdminComponent.prototype.closeWindow = function () {
        this.windowClose.emit(false);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], DetailEquipmentAdminComponent.prototype, "equipmentId", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailEquipmentAdminComponent.prototype, "windowClose", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailEquipmentAdminComponent.prototype, "updateList", void 0);
    DetailEquipmentAdminComponent = __decorate([
        core_1.Component({
            selector: "equipment",
            templateUrl: 'app/components/admin/equipment/detail/detail.equipment.admin.component.html',
            styleUrls: ['lib/css/modalWindow.css'],
        }), 
        __metadata('design:paramtypes', [equipment_service_1.EquipmentService])
    ], DetailEquipmentAdminComponent);
    return DetailEquipmentAdminComponent;
}());
exports.DetailEquipmentAdminComponent = DetailEquipmentAdminComponent;
