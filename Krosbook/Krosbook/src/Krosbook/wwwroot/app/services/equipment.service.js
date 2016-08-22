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
 * Created by krosaci on 21.7.2016.
 */
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
require('rxjs/add/operator/map');
var EquipmentService = (function () {
    function EquipmentService(http) {
        this.http = http;
        this.hasRoleAdmin = false;
        var _build = http._backend._browserXHR.build;
        http._backend._browserXHR.build = function () {
            var _xhr = _build();
            _xhr.withCredentials = true;
            return _xhr;
        };
    }
    EquipmentService.prototype.getEquipments = function () {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/equipment', { headers: headers });
    };
    EquipmentService.prototype.getEquipment = function (id) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.get('/api/equipment/' + id, { headers: headers });
    };
    EquipmentService.prototype.addEquipment = function (equipment) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.post('/api/equipment', equipment, { headers: headers });
    };
    EquipmentService.prototype.editEquipment = function (id, equipment) {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return this.http.put("/api/equipment/" + id, equipment, { headers: headers });
    };
    EquipmentService.prototype.removeEquipment = function (id) {
        return this.http.delete('/api/equipment/' + id);
    };
    EquipmentService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], EquipmentService);
    return EquipmentService;
}());
exports.EquipmentService = EquipmentService;
