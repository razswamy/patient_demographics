import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable, OnDestroy } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { environment } from "./../../environments/environment";
import { Httpmethod } from "./../models/enums/httpmethod.enum";
import { NgxSpinnerService } from "ngx-spinner";
@Injectable()
export class RestFactoryService implements OnDestroy {
  constructor(
    private http: HttpClient,
    private spinnerService: NgxSpinnerService
  ) { }

  REST<T>(method: Httpmethod, url: string, data: any): Observable<T> {
    let body = JSON.stringify(data);
    const httpOptions = {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    };
    url = environment.API_ENDPOINT + url;
    let returndata;
    this.spinnerService.show();
    switch (method) {
      case Httpmethod.POST:
        returndata = this.http.post<T>(url, body, httpOptions);
        break;
      case Httpmethod.PUT:
        returndata = this.http.put<T>(url, body);
        break;
      case Httpmethod.PATCH:
        returndata = this.http.patch<T>(url, body);
        break;
      case Httpmethod.DELETE:
        returndata = this.http.delete<T>(url);
        break;
      case Httpmethod.GET:
      default:
        returndata = this.http.get<T>(url);
        break;
    }
    /* returndata.subscribe(
      data => {
        if (( < response > data).error) {
          this.nf.error(data.errormessage);
        }
        this.blockUI.stop();
      },
      err => {
        this.nf.error(err.error.errormessage);
        this.blockUI.stop();
      }
    );*/
    return returndata;
  }



  ngOnDestroy() { }
}
