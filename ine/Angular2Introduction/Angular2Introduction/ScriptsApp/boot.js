"use strict";
/// <reference path="../typings/browser.d.ts" />
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var mainApp_1 = require('./mainApp');
var http_1 = require('@angular/http');
platform_browser_dynamic_1.bootstrap(mainApp_1.AppComponent, [http_1.HTTP_PROVIDERS]);
//# sourceMappingURL=boot.js.map