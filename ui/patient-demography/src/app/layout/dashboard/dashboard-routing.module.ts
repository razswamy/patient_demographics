import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { PatientAddComponent } from "./patient-add/patient-add.component";
import { PatientEditComponent } from "./patient-edit/patient-edit.component";
const routes: Routes = [
    {
        path: '', component: DashboardComponent,
    }, {
        path: 'patient-add', component: PatientAddComponent
    }, {
        path: 'patient-edit/:patientid', component: PatientEditComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DashboardRoutingModule {
}
