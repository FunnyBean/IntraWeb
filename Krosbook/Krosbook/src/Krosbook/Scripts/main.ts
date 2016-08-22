import { bootstrap } from '@angular/platform-browser-dynamic';
import { enableProdMode } from '@angular/core';
import { AppComponent } from './app/components/app.component';
import {APP_ROUTER_PROVIDERS} from './app/components/app.routes';
import {HTTP_PROVIDERS} from '@angular/http';
import { disableDeprecatedForms, provideForms } from '@angular/forms';

enableProdMode();
bootstrap(AppComponent, [disableDeprecatedForms(), provideForms(), APP_ROUTER_PROVIDERS, HTTP_PROVIDERS]);

