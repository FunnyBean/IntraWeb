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
 * Created by Ondrej on 01.08.2016.
 */
var core_1 = require('@angular/core');
var reservation_service_1 = require('../../../../../services/reservation.service');
var user_service_1 = require('../../../../../services/user.service');
var reservation_model_1 = require('../../../../../models/reservation.model');
var time_validator_1 = require('../../../../../validators/time.validator');
var moment = require('moment');
var DetailReservationComponent = (function () {
    function DetailReservationComponent(reservationService, userService) {
        this.reservationService = reservationService;
        this.userService = userService;
        this.windowClose = new core_1.EventEmitter();
        this.saving = false;
        this.data = new reservation_model_1.Reservation();
        this.canEdit = false;
        this.emailInvitation = false;
        this.reserveGoToMeeting = false;
    }
    DetailReservationComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.reservationService.getReservation(this.reservationType, this.reservationDetailId[0]).subscribe(function (data) {
            _this.data = data.json();
            _this.dateTime = moment(_this.data.dateTime).format("DD.MM.YYYY HH:mm");
            _this.data.length = _this.data.length / 60;
            _this.authorizeActions();
            _this.updateMaxTime();
            _this.updateEndTime();
        }, function (error) { return console.log(error); });
    };
    DetailReservationComponent.prototype.updateMaxTime = function () {
        this.maxTime = ((18 - moment(this.data.dateTime).hour()) * 60 - moment(this.data.dateTime).minute()) / 60;
    };
    DetailReservationComponent.prototype.updateEndTime = function () {
        if (this.canEdit)
            this.endDateTime = moment(this.data.dateTime).add(this.data.length * 60, 'minutes').format("YYYY-MM-DDTHH:mm");
        else
            this.endDateTime = moment(this.data.dateTime).add(this.data.length * 60, 'minutes').format("DD.MM.YYYY HH:mm");
    };
    DetailReservationComponent.prototype.updateLength = function () {
        this.data.length = (moment(this.endDateTime).unix() - moment(this.data.dateTime).unix()) / 3600;
    };
    DetailReservationComponent.prototype.authorizeActions = function () {
        if (this.data.userId == this.loggedUser.id)
            this.canEdit = true;
        else {
            for (var i = 0; i < this.loggedUser.roles.length; i++) {
                if (this.loggedUser.roles[i].roleId == 1) {
                    this.canEdit = true;
                    break;
                }
            }
        }
    };
    DetailReservationComponent.prototype.editReservation = function () {
        var _this = this;
        this.saving = true;
        var elementId = (this.reservationType == "rooms") ? this.data.roomId : this.data.carId, dayData;
        this.reservationService.getReservations(this.reservationType, elementId, moment(this.data.dateTime).format("DD.MM.YYYY"), moment(this.data.dateTime).add(1, 'days').format("DD.MM.YYYY")).subscribe(function (data) { dayData = data.json(); }, function (error) { return console.log(error); }, function () {
            for (var i = 0; i < dayData.length; i++) {
                if (_this.data.id == dayData[i].id)
                    continue;
                var date = moment(dayData[i].dateTime), reservationTime = moment(_this.data.dateTime).format("HH:mm"), reservationTimeEnd = moment(_this.data.dateTime).add(_this.data.length * 60, 'minutes').format("HH:mm"), time = date.format("HH:mm"), endTime = date.add(dayData[i].length, 'minutes').format("HH:mm");
                console.log(reservationTime + ' ' + time + ' ' + reservationTimeEnd);
                if ((reservationTime >= time && reservationTime < endTime) || (time >= reservationTime && time < reservationTimeEnd)) {
                    _this.error = "Zvolený čas zasahuje do rezervácie iného používateľa.";
                    return false;
                }
            }
            _this.reservationService.editReservation(_this.reservationType, _this.data.id, _this.data.name, elementId, _this.data.userId, _this.data.dateTime, _this.data.length * 60, _this.emailInvitation, _this.reserveGoToMeeting).subscribe(function (data) { }, function (error) {
                _this.error = 'Na daný termín je v GoToMeeting naplánovaná už iná rezervácia';
                console.log("error " + error);
                _this.saving = false;
            }, function () {
                _this.saving = false;
                _this.windowClose.emit(true);
            });
        });
    };
    DetailReservationComponent.prototype.deleteReservation = function () {
        var _this = this;
        this.reservationService.deleteReservation(this.reservationType, this.data.id).subscribe(function () { _this.windowClose.emit(true); }, function (error) { return console.log(error); });
    };
    DetailReservationComponent.prototype.closeWindow = function () {
        this.windowClose.emit(false);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], DetailReservationComponent.prototype, "reservationDetailId", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], DetailReservationComponent.prototype, "reservationType", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], DetailReservationComponent.prototype, "loggedUser", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], DetailReservationComponent.prototype, "usersList", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DetailReservationComponent.prototype, "windowClose", void 0);
    DetailReservationComponent = __decorate([
        core_1.Component({
            selector: 'reservation-detail',
            templateUrl: 'app/components/home/reservations/table/detail/detail.reservation.component.html',
            styleUrls: ['app/components/home/reservations/table/detail/detail.reservation.component.css'],
            directives: [time_validator_1.TimeValidator, time_validator_1.DateValidator],
            providers: [reservation_service_1.ReservationService]
        }), 
        __metadata('design:paramtypes', [reservation_service_1.ReservationService, user_service_1.UserService])
    ], DetailReservationComponent);
    return DetailReservationComponent;
}());
exports.DetailReservationComponent = DetailReservationComponent;
