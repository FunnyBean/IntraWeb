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
var roles_service_1 = require('../../../services/roles.service');
var detail_role_admin_component_1 = require('./detail/detail.role.admin.component');
var RolesAdminComponent = (function () {
    function RolesAdminComponent(rolesService) {
        this.rolesService = rolesService;
        this.showRoleWindow = false;
    }
    RolesAdminComponent.prototype.ngOnInit = function () {
        this.getRoles();
    };
    RolesAdminComponent.prototype.getRoles = function () {
        var _this = this;
        this.rolesService.getRoles()
            .subscribe(function (data) { _this.roles = data.json(); }, function (error) { return console.error(error); });
    };
    RolesAdminComponent.prototype.deleteRole = function (id) {
        var _this = this;
        if (confirm("Naozaj chcete vymazať túto rolu?")) {
            this.rolesService.removeRole(id).subscribe(function (data) { }, function (error) { alert(error); }, function () { _this.getRoles(); });
        }
    };
    RolesAdminComponent.prototype.newRole = function () {
        this.roleId = null;
        this.windowOpen();
    };
    RolesAdminComponent.prototype.editRole = function (id) {
        for (var i = 0; i < this.roles.length; i++) {
            if (this.roles[i].id == id) {
                this.roleId = this.roles[i].id;
                break;
            }
        }
        this.windowOpen();
    };
    RolesAdminComponent.prototype.windowOpen = function () {
        this.showRoleWindow = true;
    };
    RolesAdminComponent.prototype.windowClose = function (action) {
        this.showRoleWindow = action;
    };
    RolesAdminComponent = __decorate([
        core_1.Component({
            templateUrl: 'app/components/admin/roles/roles.admin.component.html',
            directives: [detail_role_admin_component_1.DetailRoleAdminComponent]
        }), 
        __metadata('design:paramtypes', [roles_service_1.RolesService])
    ], RolesAdminComponent);
    return RolesAdminComponent;
}());
exports.RolesAdminComponent = RolesAdminComponent;
