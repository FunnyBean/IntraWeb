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
var office_service_1 = require('../../../services/office.service');
var detail_office_admin_component_1 = require('./detail/detail.office.admin.component');
var OfficesAdminComponent = (function () {
    function OfficesAdminComponent(officeService) {
        this.officeService = officeService;
        this.showOfficeWindow = false;
    }
    OfficesAdminComponent.prototype.ngOnInit = function () {
        this.getOffices();
    };
    OfficesAdminComponent.prototype.getOffices = function () {
        var _this = this;
        this.officeService.getOffices()
            .subscribe(function (data) { _this.offices = data.json(); }, function (error) { return console.error(error); });
    };
    OfficesAdminComponent.prototype.deleteOffice = function (id) {
        var _this = this;
        if (confirm("Naozaj chcete vymazať miestnosť?")) {
            this.officeService.removeOffice(id).subscribe(function (data) { }, function (error) { alert(error); }, function () { _this.getOffices(); });
        }
    };
    OfficesAdminComponent.prototype.newOffice = function () {
        this.officeId = null;
        this.windowOpen();
    };
    OfficesAdminComponent.prototype.editOffice = function (id) {
        for (var i = 0; i < this.offices.length; i++) {
            if (this.offices[i].id == id) {
                this.officeId = this.offices[i].id;
                break;
            }
        }
        this.windowOpen();
    };
    //opens detail.office.admin.component window
    OfficesAdminComponent.prototype.windowOpen = function () {
        this.showOfficeWindow = true;
    };
    //closes detail.office.admin.component window
    OfficesAdminComponent.prototype.windowClose = function () {
        this.showOfficeWindow = false;
    };
    OfficesAdminComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/components/admin/offices/offices.admin.component.html',
            directives: [detail_office_admin_component_1.DetailOfficeAdminComponent]
        }), 
        __metadata('design:paramtypes', [office_service_1.OfficeService])
    ], OfficesAdminComponent);
    return OfficesAdminComponent;
}());
exports.OfficesAdminComponent = OfficesAdminComponent;
