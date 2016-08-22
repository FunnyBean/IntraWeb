"use strict";
/**
 * Created by Tibor Poštek on 19.07.2016.
 */
var User = (function () {
    function User() {
        this.roles = JSON.parse("[]"); //= JSON.parse("[]"); //:
    }
    return User;
}());
exports.User = User;
