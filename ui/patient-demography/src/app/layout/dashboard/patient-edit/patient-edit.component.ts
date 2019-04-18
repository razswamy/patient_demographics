import { Component, OnInit } from '@angular/core';
import { patient } from 'src/app/models/patient';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationFactory } from 'src/app/services/notificationfactory.service';
import { RestFactoryService } from 'src/app/services/rest-factory.service';
import { response } from 'src/app/models/response';
import { Httpmethod } from 'src/app/models/enums/httpmethod.enum';
import { patientphonenumber } from 'src/app/models/patientphonenumber';

@Component({
  selector: 'app-patient-edit',
  templateUrl: './patient-edit.component.html',
  styleUrls: ['./patient-edit.component.css']
})
export class PatientEditComponent implements OnInit {

  patient: patient = new patient();
  gender: Array<any> = [{ name: "None", value: 0 }, { name: "Female", value: 1 },
  { name: "Male", value: 2 },
  { name: "Other", value: 3 }
  ];
  phonenumbertypes: Array<any> = [{ name: "None", value: 0 }, { name: "Home", value: 1 },
  { name: "Work", value: 2 },
  { name: "Mobile", value: 3 }, { name: "Other", value: 4 }];
  patientid: any;
  params: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private nf: NotificationFactory,
    private rf: RestFactoryService
  ) { }

  ngOnInit() {
    this.patient = new patient();
    this.patient.gender = 0;
    this.patient.patientphonenumbers = [];
    this.params = this.route.params.subscribe(params => {
      this.patientid = params["patientid"] || 0;
      this.getpatient(this.patientid);

    });
  }
  addpatient() {
    this.rf
      .REST<response>(Httpmethod.POST, "/patients/add", this.patient)
      .subscribe(data => {
        if (!data.error) {
          this.nf.success("patient added successfully");
          this.router.navigate(["/dashboard"]);
        } else {
          //this.nf.error(data.errormessage);
        }
      });
  }
  getpatient(patientid: number) {
    this.rf
      .REST<response>(Httpmethod.GET, "/patients/" + patientid, {})
      .subscribe(data => {
        if (!data.error) {
          this.patient = data.data;
          this.patient.dateofbirth = new Date(this.patient.dateofbirth);
        } else {
          this.nf.error(data.errormessage);
        }
      });
  }
  addphonenumber() {
    this.patient.patientphonenumbers.push(new patientphonenumber());
  }

  removephonenumber($index) {
    this.patient.patientphonenumbers.splice($index, 1);
  }

}
