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
var role_admin_model_1 = require('../../../../models/role.admin.model');
var roles_service_1 = require('../../../../services/roles.service');
var DetailRoleAdminComponent = (function () {
    function DetailRoleAdminComponent(rolesService) {
        this.rolesService = rolesService;
        this.formReset = true;
        this.roleData = new role_admin_model_1.Role();
        this.windowClose = new core_1.EventEmitter();
        this.updateList = new core_1.EventEmitter();
    }
    DetailRoleAdminComponent.prototype.ngOnInit = function () {
        if (this.roleId)
            this.getRole();
    };
    DetailRoleAdminComponent.prototype.getRole = function () {
        var _this = this;
        this.rolesService.getRole(this.roleId).subscribe(function (data) { _this.roleData = data.json(); }, function (error) { return console.log(error); });
    };
    DetailRoleAdminComponent.prototype.newRole = function () {
        var _this = this;
        var name = this.roleData.name;
        var id = this.roleData.id;
        this.rolesService.addRole(JSON.stringify({ name: name, id: id })).subscribe(function (data) { }, function (error) { _this.error = error; }, function () {
            _this.success = 'Rola úspešné pridaná.';
            _this.roleData = new role_admin_model_1.Role();
            _this.formReset = false;
            setTimeout(function () { return _this.formReset = true; }, 0);
            _this.updateList.emit(true);
        });
    };
    DetailRoleAdminComponent.prototype.editRole = function () {
        var _this = this;
        var name = this.roleData.name;
        var id = this.roleData.id;
        this.rolesService.editRole(id, JSON.stringify({ id: id, name: name })).subscribe(function (data) { }, function (error) { _this.error = error; }, function () {
            _this.success = 'Rola úspešne upravená.';
            _this.updateList.emit(true);
        });
    };
    DetailRoleAdminComponent.prototype.closeWindow = function () {
        this.windowClose.emit(false);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], DetailRoleAdminComponent.prototype, "roleId", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailRoleAdminComponent.prototype, "windowClose", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailRoleAdminComponent.prototype, "updateList", void 0);
    DetailRoleAdminComponent = __decorate([
        core_1.Component({
            selector: "equipment",
            templateUrl: 'app/components/admin/roles/detail/detail.role.admin.component.html',
            styleUrls: ['lib/css/modalWindow.css'],
        }), 
        __metadata('design:paramtypes', [roles_service_1.RolesService])
    ], DetailRoleAdminComponent);
    return DetailRoleAdminComponent;
}());
exports.DetailRoleAdminComponent = DetailRoleAdminComponent;
