import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { Location, LocationStrategy, HashLocationStrategy } from '@angular/common';
import { Headers, RequestOptions, BaseRequestOptions } from '@angular/http';

import { AccountModule } from './components/account/account.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home.component';

import { routing } from './routes';

import { DataService } from './core/services/data.service';
import { AuthenticationService } from './core/services/authentication.service';
//import { UtilityService } from './core/services/utility.service';
import { NotificationService } from './core/services/notification.service';

class AppBaseRequestOptions extends BaseRequestOptions {
    headers: Headers = new Headers();

    constructor() {
        super();
        this.headers.append('Content-Type', 'application/json');
        this.body = '';
    }
}

@NgModule({
    imports: [BrowserModule,
        FormsModule,
        HttpModule,
        routing,
        AccountModule ],
    declarations: [AppComponent, HomeComponent],
    providers: [DataService, AuthenticationService, NotificationService,
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: RequestOptions, useClass: AppBaseRequestOptions }],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
