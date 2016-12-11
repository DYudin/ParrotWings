import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import { enableProdMode, HostListener } from '@angular/core';
import { DataService } from './core/services/data.service';

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
    private response: string;

    @HostListener('window:unload', ['$event'])
    unloadHandler(event) {
        this.logout();
        localStorage.removeItem('user')
    }

    constructor(public authService: AuthenticationService,
        public location: Location, public warmUpService: DataService) { }

    ngOnInit() {
        this.location.go('/');

        // warm up
        this.warmUpService.set(this._startAPI);

        this.warmUpService.get()
            .subscribe(res => {
                var data: any = res.json();
                this.response = data;

                if (data == "Done") {
                    this.servicesInitialized = true;
                }

            },
            error => console.error('Error: ' + error));
    }

    isUserLoggedIn(): boolean {
        return this.authService.isUserAuthenticated();
    }

    getUserName(): string {
        if (this.isUserLoggedIn()) {
            var _user = this.authService.getLoggedInUser();
            return _user.Username;
        }
        else
            return 'No user';
    }

    logout(): void {
        this.authService.logout()
            .subscribe(res => {
                localStorage.removeItem('user');
            },
            error => console.error('Error: ' + error),
            () => { });
    }
}
