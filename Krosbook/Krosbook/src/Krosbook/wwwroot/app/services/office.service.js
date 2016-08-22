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
var http_1 = require('@angular/http');
require('rxjs/add/operator/map');
var OfficeService = (function () {
    function OfficeService(http) {
        this.http = http;
        this.hasRoleAdmin = false;
        var _build = http._backend._browserXHR.build;
        http._backend._browserXHR.build = function () {
            var _xhr = _build();
            _xhr.withCredentials = true;
            return _xhr;
        };
    }
    OfficeService.prototype.getOffices = function () {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/rooms', { headers: headers });
    };
    OfficeService.prototype.getOffice = function (id) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/rooms/' + id, { headers: headers });
    };
    OfficeService.prototype.addOffice = function (office) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.post('/api/rooms', office, { headers: headers });
    };
    OfficeService.prototype.editOffice = function (id, office) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.put("/api/rooms/" + id, office, { headers: headers });
    };
    OfficeService.prototype.removeOffice = function (id) {
        return this.http.delete('/api/rooms/' + id);
    };
    OfficeService.prototype.getEquipment = function () {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/equipment/', { headers: headers });
    };
    OfficeService.prototype.filterOffices = function (date, length, filterType) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.post('/api/rooms/filter', JSON.stringify({ date: date, length: length, filterType: filterType }), { headers: headers });
    };
    OfficeService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], OfficeService);
    return OfficeService;
}());
exports.OfficeService = OfficeService;
