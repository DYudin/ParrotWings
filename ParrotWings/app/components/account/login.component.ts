import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Credentials } from '../../core/domain/credentials';
import { OperationResult } from '../../core/domain/operationResult';
import { AuthenticationService } from '../../core/services/authentication.service';
import { User } from '../../core/domain/user';

@Component({
    selector: 'login',
    templateUrl: './app/components/account/login.component.html'
})
export class LoginComponent implements OnInit {
    private _credentials: Credentials;
    private _currentUser: User;

    constructor(public authService: AuthenticationService,
        public router: Router) { }

    ngOnInit() {
        this._credentials = new Credentials('', '');        
    }

    login(): void {
        this.authService.login(this._credentials)
            .subscribe(
            () => {
                this._currentUser = new User(this._credentials.Email);
                localStorage.setItem('user', JSON.stringify(this._currentUser));
                this.router.navigate(['home']);
            },
            error => alert(error.json().Message));
    };
}