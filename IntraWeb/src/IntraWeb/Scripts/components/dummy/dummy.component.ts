import {Component} from 'angular2/core';
import {Router, ROUTER_PROVIDERS} from 'angular2/router';

@Component({
    selector: 'dummy',
    templateUrl: 'app/components/dummy/dummy.component.html',
    styleUrls: ['app/components/dummy/dummy.component.css'],
    providers: [ROUTER_PROVIDERS]
})
export class DummyComponent {

    constructor() {
    }

}
