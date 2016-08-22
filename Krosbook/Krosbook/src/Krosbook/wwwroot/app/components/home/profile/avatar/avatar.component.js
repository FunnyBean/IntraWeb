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
var user_admin_model_1 = require("../../../../models/user.admin.model");
var user_service_1 = require("../../../../services/user.service");
var AvatarComponent = (function () {
    function AvatarComponent(userService) {
        this.userService = userService;
        this.userData = new user_admin_model_1.User();
        this.newImage = '';
        this.progress = 0;
    }
    AvatarComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.userService.myProfile().subscribe(function (data) {
            _this.userData = data.json();
        }, function (error) { return console.log(error); });
    };
    AvatarComponent.prototype.onChange = function (event) {
        var fileToLoad = event.srcElement.files[0];
        if (fileToLoad) {
            if (fileToLoad.size > 1048576) {
                this.error = "Zvolený obrázok musí byť menší alebo rovný 1MB. Zvoľte obrázok, ktorý túto podmienku spĺňa.";
                return false;
            }
            var fileReader = new FileReader();
            var thisDocument = this;
            fileReader.onload = function (file) {
                thisDocument.newImage = fileReader.result;
                $("#result").attr("src", thisDocument.newImage);
                thisDocument.newImage = thisDocument.newImage.split(',')[1];
            };
            fileReader.onloadstart = function () { thisDocument.progress = 1; };
            fileReader.onloadend = function () { thisDocument.progress = 0; };
            fileReader.readAsDataURL(fileToLoad);
        }
    };
    AvatarComponent.prototype.editAvatar = function () {
        var _this = this;
        this.userService.updateImage(this.newImage).subscribe(function (data) {
            $("#profilePicture").attr("src", 'data:image/png;base64,' + _this.newImage);
            $("#result").removeAttr("src");
            $("#avatarImg").attr("src", "data:image/jpeg;base64," + _this.newImage);
        }, function (error) { return console.log(error); });
    };
    AvatarComponent.prototype.dismiss = function () {
        this.error = "";
    };
    AvatarComponent = __decorate([
        core_1.Component({
            selector: 'avatar',
            templateUrl: 'app/components/home/profile/avatar/avatar.component.html',
            styles: ['.avatarImg{width: 150px; height: auto; display: block; margin: 0 auto;margin-bottom: 15px; margin-top: 15px}'],
        }), 
        __metadata('design:paramtypes', [user_service_1.UserService])
    ], AvatarComponent);
    return AvatarComponent;
}());
exports.AvatarComponent = AvatarComponent;
