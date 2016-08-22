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
var reservation_service_1 = require('../../../../services/reservation.service');
var holiday_service_1 = require('../../../../services/holiday.service');
var detail_reservation_component_1 = require('./detail/detail.reservation.component');
var moment = require('moment');
var TableReservationComponent = (function () {
    function TableReservationComponent(reservationService, holidayService) {
        this.reservationService = reservationService;
        this.holidayService = holidayService;
        this.tableData = [];
        this.detailReset = false;
        this.reservationDetailId = [0, 0, 0, 0];
        this.reservationInProgress = false;
    }
    TableReservationComponent.prototype.ngOnInit = function () {
        this.updateData();
        this.data.color = (this.data.color === undefined) ? '#337ab7' : this.data.color;
        var thisDocument = this;
        $(window).on("focus", function () {
            thisDocument.updateData();
        });
    };
    TableReservationComponent.prototype.ngOnDestroy = function () {
        $(window).unbind("focus");
    };
    TableReservationComponent.prototype.fillTable = function () {
        var table = '<tr><td colspan="6" class="officeName" style="background-color: ' + this.data.color + '"><h4>' + this.data.name + '</h4></td></tr>', fromRow, fromCol, length = 1, isMouseDown = false, thisDocument = this, col, row, beforeRow = 0;
        for (var i = 0; i < this.times.length; i++) {
            table += '<tr>';
            table += '<td class="col-md-1">' + this.times[i].time + '</td>';
            for (var j = 0; j < this.tableData[this.times[i].time].length; j++) {
                var cell = this.tableData[this.times[i].time][j];
                var holiday = this.holidayService.isHoliday(moment().add(this.week, 'weeks').weekday(j + 1).format("DD/MM"), moment().add(this.week, 'weeks').format("YYYY"));
                if (cell.long == null) {
                    var filter = 'empty';
                    if (this.filterActive && moment(this.filterDateTime).format("DD.MM.YYYY") == moment().add(this.week, 'weeks').weekday(j + 1).format("DD.MM.YYYY") && moment(this.filterDateTime).format("HH:mm") <= this.times[i].time && this.times[i].time < moment(this.filterDateTime).add(this.filterTimeLength * 60, 'minutes').format("HH:mm"))
                        filter += 'filterSelected';
                    if (holiday) {
                        if (i == 0)
                            table += '<td class="col-md-2 holiday">' + holiday + '</td>';
                        else
                            table += '<td class="col-md-2 holiday"></td>';
                    }
                    else
                        table += '<td class="col-md-2 empty ' + filter + '"></td>';
                }
                else {
                    if (cell.reservationName == null) {
                        table += '<td reservationId="' + cell.reservationId + '" class="col-md-2 bg-primary full"></td>';
                    }
                    else {
                        table += '<td reservationId="' + cell.reservationId + '" class="col-md-2 bg-primary full text-center"><strong>' + cell.reservationName + '</strong> <small>' + cell.userName + '</small></td>';
                    }
                }
            }
            table += '</tr>';
        }
        //replacing old data with new table data
        $(".records" + this.data.id + " > tr").remove();
        $(".records" + this.data.id).prepend(table);
        $(".records" + this.data.id + " td.full").on("click", function (event) {
            setTimeout(function () { return thisDocument.detailReset = true; }, 0);
            var element = $(this);
            var id = element.attr("reservationId");
            if (thisDocument.reservationDetailId[0] == 0 || thisDocument.reservationDetailId[0] != id) {
                thisDocument.detailReset = false;
                var horizontalPosition = (element.index() !== 5) ? (element.position().left).toString() + 'px' : (element.position().left + element.width() - 294).toString() + 'px';
                thisDocument.reservationDetailId = [id, horizontalPosition, (element.position().top + $("#content").scrollTop() + 25).toString() + 'px', 1];
            }
            else {
                thisDocument.detailReset = false;
                thisDocument.reservationDetailId = [0, 0, 0, 0];
            }
        });
        $(".records" + this.data.id + " td.empty").on("mouseenter", function (event) {
            col = $(this).parent().children().index($(this));
            row = $(this).parent().parent().children().index($(this).parent()) - 1;
        });
        $(".records" + this.data.id + " td.empty")
            .on("mousedown", function (event) {
            if (event.which != 1 || thisDocument.reservationDetailId[0] != 0 || thisDocument.reservationInProgress)
                return false; //does not work for other than left button
            var element = $(this);
            isMouseDown = true;
            $(this).addClass("selected");
            fromRow = row;
            fromCol = col;
            beforeRow = row;
            var horizontalPosition = (element.index() !== 5) ? (element.position().left).toString() + 'px' : (element.position().left + element.width() - 294).toString() + 'px';
            thisDocument.reservationDetailId = [0, horizontalPosition, (element.position().top + $("#content").scrollTop() + 25).toString() + 'px'];
            return false;
        })
            .on("mouseover", function () {
            if (isMouseDown && col == fromCol && (row - 1) == beforeRow && !($(this).hasClass("selected"))) {
                $(this).addClass("selected");
                beforeRow = row;
                length++;
            }
        });
        $(document).on("mouseup", function () {
            if (isMouseDown) {
                thisDocument.makeReservation(fromRow, fromCol, length);
                isMouseDown = false;
                length = 1;
            }
        });
    };
    TableReservationComponent.prototype.makeReservation = function (fromRow, fromCol, length) {
        var _this = this;
        this.reservationInProgress = true;
        var date, hours = 7, minutes = 0;
        if (fromRow % 2 != 0) {
            fromRow -= 1;
            minutes = 30;
        }
        for (var i = 0; i < fromRow / 2; i++)
            hours++;
        date = moment().add(this.week, 'weeks').weekday(fromCol).hour(hours).minute(minutes).format("DD.MM.YYYY HH:mm");
        //checks, if selected time is not already reserved
        var time = moment().hour(hours).minute(minutes).format("HH:mm"), endTime = moment(time, 'HH:mm').add(length * 30, 'minutes').format("HH:mm");
        while (time < endTime) {
            if (this.tableData[time] && this.tableData[time][fromCol - 1]) {
                alert('Vaša rezervácia zasahuje do inej rezervácie. Zvoľte svoju rezerváciu inak.');
                this.fillTable();
                return false;
            }
            time = moment(time, 'HH:mm').add(30, 'minutes').format('HH:mm');
        }
        //checks if selected reservation is not in the past
        /*if(date < moment().format("DD.MM.YYYY HH:mm")){
          alert('Zvolená rezervácia je v minulosti. Nie je možné ju vytroviť.');
          this.fillTable();
          return false;
        }*/
        //saves the data  
        this.reservationService.addReservation(this.reservationType, this.data.id, 1, 'Rezervácia', date, length * 30).subscribe(function (data) {
            _this.detailReset = true;
            _this.reservationDetailId = [data.json().id, _this.reservationDetailId[1], _this.reservationDetailId[2], 0]; //okno na potvrdenie rezervacie      
        }, function (error) { alert(error); }, function () {
            _this.updateData();
            _this.reservationInProgress = false;
            $(".filterSelected").removeClass("filterSelected");
        });
    };
    TableReservationComponent.prototype.updateData = function (weeks) {
        var _this = this;
        if (weeks === void 0) { weeks = this.week; }
        this.reservationService.getReservations(this.reservationType, this.data.id, moment().add(weeks, 'weeks').weekday(1).format("DD.MM.YYYY"), moment().add(weeks, 'weeks').weekday(6).format("DD.MM.YYYY")).subscribe(function (data) {
            _this.reservationsData = data.json();
        }, function (error) { return console.log(error); }, function () {
            //setting default data to 0
            for (var i = 0; i < _this.times.length; i++) {
                var pTime = _this.times[i].time;
                _this.tableData[pTime] = new Array();
                for (var j = 0; j < 5; j++)
                    _this.tableData[pTime][j] = 0;
            }
            //for each record, set table data
            for (var i = 0; i < _this.reservationsData.length; i++) {
                var record = _this.reservationsData[i];
                var time = moment(record.dateTime, "YYYY-MM-DD HH:mm:ss").format("HH:mm"), endTime = moment(record.dateTime, "YYYY-MM-DD HH:mm:ss").add(record.length, 'minutes').format('HH:mm'), day = moment(record.dateTime, "YYYY-MM-DD HH:mm:ss").weekday() - 1;
                while (time < endTime && time <= '17:30') {
                    if (time == moment(record.dateTime, "YYYY-MM-DD HH:mm:ss").format("HH:mm"))
                        _this.tableData[time][day] = JSON.parse('{"userName": "' + _this.usersList[record.userId] + '", "long": 0, "reservationName":"' + record.name + '", "reservationId": "' + record.id + '"}');
                    else
                        _this.tableData[time][day] = JSON.parse('{"userName": "", "long": 1, "reservationId": "' + record.id + '"}');
                    time = moment(time, 'HH:mm').add(30, 'minutes').format('HH:mm');
                }
            }
            _this.fillTable();
        });
    };
    TableReservationComponent.prototype.closeWindow = function ($event) {
        this.reservationDetailId = [0, 0, 0];
        this.detailReset = false;
        if ($event)
            this.updateData();
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "data", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "times", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "reservationType", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "usersList", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "loggedUser", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "week", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "filterActive", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "filterDateTime", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], TableReservationComponent.prototype, "filterTimeLength", void 0);
    TableReservationComponent = __decorate([
        core_1.Component({
            selector: 'tbody',
            templateUrl: 'app/components/home/reservations/table/table.reservations.component.html',
            directives: [detail_reservation_component_1.DetailReservationComponent],
            providers: [reservation_service_1.ReservationService, holiday_service_1.HolidayService]
        }), 
        __metadata('design:paramtypes', [reservation_service_1.ReservationService, holiday_service_1.HolidayService])
    ], TableReservationComponent);
    return TableReservationComponent;
}());
exports.TableReservationComponent = TableReservationComponent;
