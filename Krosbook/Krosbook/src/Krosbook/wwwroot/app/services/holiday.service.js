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
var moment = require('moment');
var HolidayService = (function () {
    function HolidayService() {
        this.holidays = ['01/01', '06/01', '', '', '01/05', '08/05', '05/07', '29/08', '01/09', '15/09', '01/11', '17/11', '24/12', '25/12', '26/12'];
        this.holidayNames = ['Deň vzniku Slovenskej republiky',
            'Zjavenie Pána (Traja králi)',
            'Veľký piatok',
            'Veľkonočný pondelok',
            'Sviatok práce',
            'Deň víťazstva nad fašizmom',
            'Sviatok svätého Cyrila a Metoda',
            'Výročie SNP',
            'Deň Ústavy Slovenskej republiky',
            'Sedembolestná Panna Mária',
            'Sviatok všetkých svätých ',
            'Deň boja za slobodu a demokraciu',
            'Štedrý deň',
            'Prvý sviatok vianočný',
            'Druhý sviatok vianočný'];
        this.year = 2016;
        this.buildEaster();
    }
    HolidayService.prototype.buildEaster = function () {
        var a = this.year % 19;
        var b = this.year % 4;
        var c = this.year % 7;
        var k = Math.floor(this.year / 100);
        var p = Math.floor((13 + 8 * k) / 25);
        var q = Math.floor(k / 4);
        var M = (15 - p + k - q) % 30;
        var N = (4 + k - q) % 7;
        var d = (19 * a + M) % 30;
        var e = (2 * b + 4 * c + 6 * d + N) % 7;
        if (d == 29 && e == 6) {
            this.holidays[2] = "24/04";
            this.holidays[3] = "27/04";
        }
        else if (d == 28 && e == 6 && (11 * M + 11) % 30 < 19) {
            this.holidays[2] = "16/04";
            this.holidays[3] = "19/04";
        }
        else if (22 + d + e <= 31) {
            var sunday = moment((22 + d + e) + ".03." + this.year, "DD.MM.YYYY");
            this.holidays[2] = sunday.add(-2, 'days').format("DD/MM");
            this.holidays[3] = sunday.add(3, 'days').format("DD/MM");
        }
        else if (d + e - 9 > 0) {
            var sunday = moment((d + e - 9) + ".04." + this.year, "DD.MM.YYYY");
            this.holidays[2] = sunday.add(-2, 'days').format("DD/MM");
            this.holidays[3] = sunday.add(3, 'days').format("DD/MM");
        }
    };
    HolidayService.prototype.isHoliday = function (date, year) {
        if (this.year != year) {
            this.year = year;
            this.buildEaster();
        }
        if (this.holidays.indexOf(date) == -1) {
            return null;
        }
        else {
            return this.holidayNames[this.holidays.indexOf(date)];
        }
    };
    HolidayService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], HolidayService);
    return HolidayService;
}());
exports.HolidayService = HolidayService;
