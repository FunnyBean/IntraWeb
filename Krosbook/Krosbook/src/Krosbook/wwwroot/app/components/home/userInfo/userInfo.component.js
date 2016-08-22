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
var router_1 = require('@angular/router');
var user_service_1 = require('../../../services/user.service');
var ng2_cookies_1 = require('ng2-cookies/ng2-cookies');
//import {ProfileComponent} from "../profile/profile.component";
var UserInfoComponent = (function () {
    function UserInfoComponent(router, userService) {
        this.router = router;
        this.userService = userService;
    }
    UserInfoComponent.prototype.ngOnInit = function () {
        this.hideUserInfo();
    };
    UserInfoComponent.prototype.logout = function () {
        var _this = this;
        this.userService.logout()
            .subscribe(function (data) {
            _this.router.navigate(['/login']);
        }, function (error) {
            console.log('Nepodarilo sa odhlásiť, pravdepodobne nastala chyba servera!');
            ng2_cookies_1.Cookie.delete("KrosbookAuth");
            _this.router.navigate(['/login']);
        });
    };
    UserInfoComponent.prototype.hideUserInfo = function () {
        document.getElementById('userInfo').style.display = 'none';
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], UserInfoComponent.prototype, "userData", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], UserInfoComponent.prototype, "isAdmin", void 0);
    UserInfoComponent = __decorate([
        core_1.Component({
            selector: 'user_info',
            templateUrl: 'app/components/home/userInfo/userInfo.component.html',
            styleUrls: ['app/components/home/userInfo/userInfo.component.css'],
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [router_1.Router, user_service_1.UserService])
    ], UserInfoComponent);
    return UserInfoComponent;
}());
exports.UserInfoComponent = UserInfoComponent;
