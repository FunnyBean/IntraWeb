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
 * Created by krosaci on 26.7.2016.
 */
var core_1 = require('@angular/core');
var user_service_1 = require('../../../../services/user.service');
var equal_validator_1 = require('../../../../validators/equal.validator');
var PasswordComponent = (function () {
    function PasswordComponent(userService) {
        this.userService = userService;
        this.passwordData = ['', '', ''];
    }
    PasswordComponent.prototype.ngAfterContentInit = function () {
        $(".active").removeClass("active");
        $("#password").addClass("active");
    };
    PasswordComponent.prototype.updatePasswordData = function (col, value) {
        this.passwordData[col] = value;
        console.log(this.passwordData);
    };
    PasswordComponent.prototype.savePassword = function () {
        var _this = this;
        this.userService.updatePassword(this.passwordData[0], this.passwordData[1]).subscribe(function (data) { _this.success = "Heslo bolo úspešne zmenené."; $("input").val(''); }, function (error) { _this.error = "Staré heslo sa nezhoduje so záznamom v databáze."; });
    };
    PasswordComponent = __decorate([
        core_1.Component({
            selector: 'password',
            templateUrl: 'app/components/home/profile/password/password.component.html',
            styleUrls: ['../../../../lib/css/modalWindow.css'],
            directives: [equal_validator_1.EqualValidator]
        }), 
        __metadata('design:paramtypes', [user_service_1.UserService])
    ], PasswordComponent);
    return PasswordComponent;
}());
exports.PasswordComponent = PasswordComponent;
