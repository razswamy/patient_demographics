import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { tap } from "rxjs/operators";
import { NgxSpinnerService } from "ngx-spinner";
import { NotificationFactory } from "../services/notificationfactory.service";
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private nf: NotificationFactory,
    private spinnerService: NgxSpinnerService
  ) { }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const dupReq = req.clone();

    const started = Date.now();
    return next.handle(dupReq).pipe(
      tap(
        event => {
          if (event instanceof HttpResponse) {
            this.spinnerService.hide();
            if (event.body["error"]) {
              this.nf.error(event.body.errormessage);
            }
          }
        },
        error => {
          this.spinnerService.hide();
          if (error instanceof HttpErrorResponse) {
            switch (error.status) {

              case 500:
                this.nf.error(error.error.errormessage);
                break;
            }
          }
        }
      )
    );
  }
}
