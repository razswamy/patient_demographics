import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

    collapedSideBar: boolean;
    patient: any;

    constructor() { }

    ngOnInit() {
    }

    receiveCollapsed($event) {
        this.collapedSideBar = $event;
    }

}
