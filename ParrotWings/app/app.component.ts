import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import { Router } from '@angular/router';
import { enableProdMode, HostListener } from '@angular/core';
import { DataService } from './core/services/data.service';
import { ErrorService } from './core/services/error.service';
import { ErrorComponent } from './components/error.component';

enableProdMode();
import { AuthenticationService } from './core/services/authentication.service';
import { User } from './core/domain/user';

@Component({
    selector: 'parrotwings-app',
  templateUrl: './app/app.component.html'
})

export class AppComponent implements OnInit {
    private _startAPI: string = 'api/warmup/start';
    private servicesInitialized: boolean = false;
    private servicesInitFailed: boolean = false;
    private response: string;

    @HostListener('window:unload', ['$event'])
    unloadHandler(event) {
        this.logout();
        localStorage.removeItem('user');
    }

    constructor(public authService: AuthenticationService,
        public location: Location,
        public warmUpService: DataService,
        public router: Router,
        public errorService: ErrorService) { }

    ngOnInit() {
        this.location.go('/');

        this.warmUp();
    }

    warmUp(): void {
        // warm up
        this.warmUpService.set(this._startAPI);

        this.warmUpService.get()
            .subscribe(res => {
                var data: any = res.json();
                this.response = data;

                if (data == "Warm up completed") {
                    this.servicesInitialized = true;
                }

            },
            error => {
                this.servicesInitFailed = true;
                if (error._body != undefined && error._body != null) {
                    var errorBody = error._body;
                    this.errorService.setErrorBody(errorBody);
                    this.router.navigate(['error']);
                }
                else {
                    this.errorService.setErrorBody(error);
                    this.router.navigate(['error']);
                }
               
                console.error('Error: ' + error);
            });        
    }

    isUserLoggedIn(): boolean {
        return this.authService.isUserAuthenticated();
    }

    //getUserName(): string {
    //    if (this.isUserLoggedIn()) {
    //        var _user = this.authService.getLoggedInUser();
    //        return _user.UserName;
    //    }
    //    else
    //        return 'No user';
    //}

    logout(): void {
        this.authService.logout()
            .subscribe(
            () => {
                localStorage.removeItem('user');
            },
            error => console.error('Error: ' + error),
            () => { });
    }
}
