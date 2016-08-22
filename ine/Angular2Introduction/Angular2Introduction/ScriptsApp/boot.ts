/// <reference path="../typings/browser.d.ts" />
import {bootstrap} from '@angular/platform-browser-dynamic'
import {AppComponent} from './mainApp'
import {HTTP_PROVIDERS} from '@angular/http'

bootstrap(AppComponent, [HTTP_PROVIDERS]);