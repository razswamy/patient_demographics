import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LanguageTranslationModule } from './shared/modules/language-translation/language-translation.module'
import { SimpleNotificationsModule } from "angular2-notifications";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthGuard } from './shared';
import { RestFactoryService } from './services/rest-factory.service';
import { NotificationFactory } from './services/notificationfactory.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { MomentModule } from 'ngx-moment';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule, MomentModule,
        LanguageTranslationModule, NgxSpinnerModule,
        AppRoutingModule, SimpleNotificationsModule.forRoot({
            timeOut: 5000,
            position: ["bottom", "center"]
        }),
    ],
    declarations: [AppComponent],
    providers: [AuthGuard, RestFactoryService, NotificationFactory,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true
        }],
    bootstrap: [AppComponent]
})
export class AppModule { }
