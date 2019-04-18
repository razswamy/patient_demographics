import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbCarouselModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';
import { DataTableModule } from "angular5-data-table";

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { MomentModule } from 'ngx-moment';
import { StatModule } from '../../shared';
import { PatientAddComponent } from './patient-add/patient-add.component';
import { PatientEditComponent } from './patient-edit/patient-edit.component';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        NgbCarouselModule,
        NgbAlertModule,
        DashboardRoutingModule,
        StatModule,
        MomentModule, FormsModule,
        DataTableModule.forRoot(),
    ],
    declarations: [
        DashboardComponent,
        PatientAddComponent,
        PatientEditComponent
    ]
})
export class DashboardModule { }
