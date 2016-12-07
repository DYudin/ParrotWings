import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import 'rxjs/add/operator/map';
import { enableProdMode } from '@angular/core';

enableProdMode();
import { AuthenticationService } from './core/services/authentication.service';
import { User } from './core/domain/user';

@Component({
    selector: 'parrotwings-app',
  templateUrl: './app/app.component.html'
})

export class AppComponent implements OnInit {

    constructor(public authService: AuthenticationService,
        public location: Location) { }

    ngOnInit() {
        this.location.go('/');
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
