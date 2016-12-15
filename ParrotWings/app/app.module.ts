import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';
import { Location, LocationStrategy, HashLocationStrategy } from '@angular/common';
import { Headers, RequestOptions, BaseRequestOptions } from '@angular/http';
import { Ng2AutoCompleteModule } from 'ng2-auto-complete';

import { AccountModule } from './components/account/account.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home.component';
import { TransactionComponent } from './components/transaction.component';
import { ErrorComponent } from './components/error.component';

import { routing } from './routes';
import { DataService } from './core/services/data.service';
import { ErrorService } from './core/services/error.service';
import { AuthenticationService } from './core/services/authentication.service';

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
        AccountModule,
        Ng2AutoCompleteModule],
    declarations: [AppComponent, HomeComponent, TransactionComponent, ErrorComponent],
    providers: [DataService, AuthenticationService, ErrorService,
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: RequestOptions, useClass: AppBaseRequestOptions }],
  bootstrap:    [ AppComponent ]
})
export class AppModule { }
