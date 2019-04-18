import { Component, OnInit, ViewChild } from '@angular/core';
import { DataTable } from "angular5-data-table";
import { GridData } from 'src/app/models/griddata';
import { response } from 'src/app/models/response';
import { Httpmethod } from 'src/app/models/enums/httpmethod.enum';
import { NotificationFactory } from 'src/app/services/notificationfactory.service';
import { RestFactoryService } from 'src/app/services/rest-factory.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
    public alerts: Array<any> = [];
    public sliders: Array<any> = [];
    @ViewChild(DataTable) patientstable: DataTable;
    config = {
        limit: 10,
        offset: 0,
        count: null,
        searchkey: "",
        pageLimits: [10, 20, 50, 100, 500]
    };
    columns = [{ key: "forenames", title: "Forenames", sortable: false },
    { key: "surname", title: "Sur Name", sortable: false }];
    data = [];
    constructor(private nf: NotificationFactory, private rf: RestFactoryService) {

    }
    reloaddata($event) {
        this.cleanupsearchkey();
        let tempdata = new GridData();
        tempdata.searchkey = this.config.searchkey;
        tempdata.limit = $event.limit;
        tempdata.offset = $event.offset;
        this.getdata(tempdata);
    }
    cleanupsearchkey() {
        if (this.config.searchkey === undefined || this.config.searchkey === null) {
            this.config.searchkey = "";
        }
        this.config.searchkey = this.config.searchkey.trim();
    }
    searchtable() {
        this.cleanupsearchkey();
        if (this.config.searchkey.length === 0) {
            let tempdata = new GridData();
            tempdata.searchkey = "";
            tempdata.limit = this.patientstable.limit;
            tempdata.offset = 0;

            this.getdata(tempdata);
        } else if (this.config.searchkey.length <= 2) {
            this.nf.info("Minimum 3 characters to search");
        } else {
            let tempdata = new GridData();
            tempdata.searchkey = this.config.searchkey;
            tempdata.limit = this.patientstable.limit;
            tempdata.offset = 0;
            this.getdata(tempdata);
        }
    }

    getdata(griddata: GridData) {
        const params =
            `limit=${griddata.limit}&offset=${griddata.offset}` +
            (griddata.searchkey === undefined ||
                griddata.searchkey === null ||
                griddata.searchkey.length <= 0
                ? ``
                : `&searchkey=${griddata.searchkey}`);
        this.rf
            .REST<response>(Httpmethod.GET, "/patients?" + params, {})
            .subscribe(data => {
                if (!data.error) {
                    this.data = data.data.rows;
                    this.config.count = data.data.total;
                }
            });
    }
    deletepatient($patientid) {
        this.rf
            .REST<response>(Httpmethod.POST, "/patients/delete/" + $patientid, {})
            .subscribe(data => {
                if (!data.error) {
                    this.nf.success("patient deleted successfully");
                    this.reloaddata(this.config);
                } else {
                    this.nf.error(data.errormessage);
                }
            });
    }
    gendertag($gender: number) {
        switch ($gender) {
            case 1:
                return "Female";
            case 2:
                return "Male";
            case 3:
                return "Other";
            default:
                return "None";
        }
    }

    phonenumbertag($phonenumber: number) {
        switch ($phonenumber) {
            case 1:
                return "Home";
            case 2:
                return "Work";
            case 3:
                return "Mobile";
            case 4:
                return "Other";
            default:
                return "None";
        }
    }

    ngOnInit() {
        let tempdata = new GridData();
        tempdata.limit = 10;
        tempdata.offset = 0;
    }

    public closeAlert(alert: any) {
        const index: number = this.alerts.indexOf(alert);
        this.alerts.splice(index, 1);
    }
}
