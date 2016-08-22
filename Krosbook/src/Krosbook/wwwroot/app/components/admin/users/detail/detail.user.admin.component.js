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
/**
 * Created by Tibor Poštek on 18.07.2016.
 */
var core_1 = require('@angular/core');
var user_service_1 = require('../../../../services/user.service');
var user_admin_model_1 = require("../../../../models/user.admin.model");
var email_validator_1 = require('../../../../validators/email.validator');
var DetailUserAdminComponent = (function () {
    function DetailUserAdminComponent(userService) {
        this.userService = userService;
        this.formReset = true;
        this.checkedRoles = new Array();
        this.userData = new user_admin_model_1.User();
        this.windowClose = new core_1.EventEmitter();
        this.updateList = new core_1.EventEmitter();
    }
    DetailUserAdminComponent.prototype.ngOnInit = function () {
        if (this.userId) {
            this.getData();
        }
        else {
            this.getRoles();
        }
    };
    DetailUserAdminComponent.prototype.getData = function () {
        var _this = this;
        this.userService.getUser(this.userId).subscribe(function (data) {
            _this.userData = data.json();
        }, function (error) { return console.error(error); }, function () { return _this.getRoles(); });
    };
    DetailUserAdminComponent.prototype.newUser = function () {
        var _this = this;
        var email = this.userData.email;
        var name = this.userData.name;
        var surname = this.userData.surname;
        var roles = JSON.parse("[]");
        for (var i = 0; i < this.allRoles.length; i++) {
            if (this.checkedRoles[i]) {
                roles.push({ "roleId": this.allRoles[i].id });
            }
        }
        this.userService.addUser(JSON.stringify({ email: email, name: name, surname: surname, roles: roles })).subscribe(function (data) { }, function (error) { _this.error = error; }, function () {
            _this.success = 'Užívateľ úspešne vytvorený.';
            _this.userData = new user_admin_model_1.User();
            _this.formReset = false;
            setTimeout(function () { return _this.formReset = true; }, 0);
            for (var i = 0; i < _this.allRoles.length; i++)
                _this.checkedRoles[i] = false;
            _this.updateList.emit(true);
        });
    };
    DetailUserAdminComponent.prototype.editUser = function () {
        var _this = this;
        var id = this.userData.id;
        var email = this.userData.email;
        var name = this.userData.name;
        var surname = this.userData.surname;
        var PhotoBase64 = this.userData.photoBase64;
        var roles = JSON.parse("[]");
        for (var i = 0; i < this.allRoles.length; i++) {
            if (this.checkedRoles[i]) {
                roles.push({ "roleId": this.allRoles[i].id });
            }
        }
        this.userService.editUser(id, JSON.stringify({ id: id, email: email, name: name, surname: surname, roles: roles, PhotoBase64: PhotoBase64 })).subscribe(function (data) {
        }, function (error) {
            _this.error = error;
        }, function () {
            _this.success = 'Užívateľ úspešne upravený.';
            _this.updateList.emit(true);
        });
    };
    DetailUserAdminComponent.prototype.getRoles = function () {
        var _this = this;
        this.userService.getRoles().subscribe(function (data) {
            _this.allRoles = data.json();
        }, function (error) { return console.log(error); }, function () { return _this.setUserRoles(); });
    };
    DetailUserAdminComponent.prototype.setUserRoles = function () {
        if (this.userId) {
            for (var i = 0; i < this.allRoles.length; i++) {
                this.checkedRoles[i] = false;
                for (var j = 0; j < this.userData.roles.length; j++) {
                    if (this.allRoles[i].id == this.userData.roles[j].roleId) {
                        this.checkedRoles[i] = true;
                    }
                }
            }
        }
    };
    DetailUserAdminComponent.prototype.closeWindow = function () {
        this.windowClose.emit(false);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], DetailUserAdminComponent.prototype, "userId", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailUserAdminComponent.prototype, "windowClose", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailUserAdminComponent.prototype, "updateList", void 0);
    DetailUserAdminComponent = __decorate([
        core_1.Component({
            selector: 'user',
            templateUrl: 'app/components/admin/users/detail/detail.user.admin.component.html',
            styleUrls: ['lib/css/modalWindow.css'],
            directives: [email_validator_1.EmailValidator]
        }), 
        __metadata('design:paramtypes', [user_service_1.UserService])
    ], DetailUserAdminComponent);
    return DetailUserAdminComponent;
}());
exports.DetailUserAdminComponent = DetailUserAdminComponent;
