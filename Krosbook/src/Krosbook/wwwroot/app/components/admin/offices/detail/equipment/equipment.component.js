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
var office_service_1 = require('../../../../../services/office.service');
var EquipmentComponent = (function () {
    function EquipmentComponent(officeService) {
        this.officeService = officeService;
        this.amount = 0;
        this.itemId = 1;
    }
    EquipmentComponent.prototype.ngOnInit = function () {
        this.getEquipmentList();
    };
    EquipmentComponent.prototype.getEquipmentList = function () {
        var _this = this;
        this.officeService.getEquipment().subscribe(function (data) { _this.list = data.json(); }, function (error) { return console.log(error); });
    };
    EquipmentComponent.prototype.addEquipment = function () {
        var desc;
        for (var i = 0; i < this.list.length; i++) {
            if (this.list[i].id == this.itemId)
                desc = this.list[i].description;
        }
        if (this.equipment)
            this.equipment.push({ "equipmentId": this.itemId, "description": desc, "amount": this.amount });
        else
            this.equipment = JSON.parse('[{"equipmentId": ' + this.itemId + ', "description": "' + desc + '", "amount": ' + this.amount + '}]');
        this.amount = 0;
    };
    EquipmentComponent.prototype.removeEquipment = function (item) {
        var lastId = this.equipment.length - 1;
        for (var i = 0; i < this.equipment.length; i++) {
            if (this.equipment[i] == item) {
                var helper = this.equipment[lastId];
                this.equipment[lastId] = this.equipment[i];
                this.equipment[i] = helper;
                delete this.equipment[lastId];
                this.equipment.length -= 1;
            }
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], EquipmentComponent.prototype, "equipment", void 0);
    EquipmentComponent = __decorate([
        core_1.Component({
            selector: 'equipment',
            templateUrl: 'app/components/admin/offices/detail/equipment/equipment.component.html',
            styles: ['.control-label { padding-top: 7px; }']
        }), 
        __metadata('design:paramtypes', [office_service_1.OfficeService])
    ], EquipmentComponent);
    return EquipmentComponent;
}());
exports.EquipmentComponent = EquipmentComponent;
