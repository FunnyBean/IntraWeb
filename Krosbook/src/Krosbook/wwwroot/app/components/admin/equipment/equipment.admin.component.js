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
var equipment_service_1 = require('../../../services/equipment.service');
var detail_equipment_admin_component_1 = require('./detail/detail.equipment.admin.component');
var EquipmentAdminComponent = (function () {
    function EquipmentAdminComponent(equipmentService) {
        this.equipmentService = equipmentService;
        this.showEquipmentWindow = false;
    }
    EquipmentAdminComponent.prototype.ngOnInit = function () {
        this.getEquipments();
    };
    EquipmentAdminComponent.prototype.getEquipments = function () {
        var _this = this;
        this.equipmentService.getEquipments()
            .subscribe(function (data) { _this.equipments = data.json(); }, function (error) { return console.error(error); });
    };
    EquipmentAdminComponent.prototype.deleteEquimpment = function (id) {
        var _this = this;
        if (confirm("Naozaj chcete vymazať vybavenie?")) {
            this.equipmentService.removeEquipment(id).subscribe(function (data) { }, function (error) { alert(error); }, function () { _this.getEquipments(); });
        }
    };
    EquipmentAdminComponent.prototype.newEquimpent = function () {
        this.equipmentId = null;
        this.windowOpen();
    };
    EquipmentAdminComponent.prototype.editEquimpent = function (id) {
        for (var i = 0; i < this.equipments.length; i++) {
            if (this.equipments[i].id == id) {
                this.equipmentId = this.equipments[i].id;
                break;
            }
        }
        this.windowOpen();
    };
    EquipmentAdminComponent.prototype.windowOpen = function () {
        this.showEquipmentWindow = true;
    };
    EquipmentAdminComponent.prototype.windowClose = function (action) {
        this.showEquipmentWindow = action;
    };
    EquipmentAdminComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/components/admin/equipment/equipment.admin.component.html',
            directives: [detail_equipment_admin_component_1.DetailEquipmentAdminComponent]
        }), 
        __metadata('design:paramtypes', [equipment_service_1.EquipmentService])
    ], EquipmentAdminComponent);
    return EquipmentAdminComponent;
}());
exports.EquipmentAdminComponent = EquipmentAdminComponent;
