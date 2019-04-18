import { Injectable } from "@angular/core";
import { NotificationsService } from "angular2-notifications";
@Injectable()
export class NotificationFactory {
  constructor(private _service: NotificationsService) {}
  success(message: string) {
    this._service.success("Success", message);
  }
  error(message: string[] | string) {
    if (typeof message === "string") {
      this._service.error("Error", message);
    } else {
      this._service.error("Error", message.join(" <BR> "));
    }
  }
  info(message: string) {
    this._service.info("Information", message);
  }
  warning(message: string) {
    this._service.warn("Warning", message);
  }
}
