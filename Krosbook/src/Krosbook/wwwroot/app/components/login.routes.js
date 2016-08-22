"use strict";
/**
 * Created by Tibor Poštek on 14.07.2016.
 */
var user_service_1 = require('../services/user.service');
var AdminGuard_1 = require("./AdminGuard");
var AuthGuard_1 = require("./AuthGuard");
exports.authProviders = [AuthGuard_1.AuthGuard, AdminGuard_1.AdminGuard, user_service_1.UserService];
