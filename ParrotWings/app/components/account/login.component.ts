import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Credentials } from '../../core/domain/credentials';
import { OperationResult } from '../../core/domain/operationResult';
import { AuthenticationService } from '../../core/services/authentication.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'login',
    templateUrl: './app/components/account/login.component.html'
})
export class LoginComponent implements OnInit {
    private _credentials: Credentials;

    constructor(public authService: AuthenticationService,
        //public notificationService: NotificationService,
        public router: Router) { }

    ngOnInit() {
        this._credentials = new Credentials('', '');
    }

    login(): void {
        var _authenticationResult: OperationResult = new OperationResult(false, '');

        this.authService.login(this._credentials)
            .subscribe(res => {
                _authenticationResult.Succeeded = res.Succeeded;
                _authenticationResult.Message = res.Message;
            },
            error => console.error('Error: ' + error),
            () => {
                if (_authenticationResult.Succeeded) {
                    alert(_authenticationResult.Message);
                    //this.notificationService.printSuccessMessage('Welcome back ' + this._user.Username + '!');
                    localStorage.setItem('user', JSON.stringify(this._credentials));
                    this.router.navigate(['home']);
                }
                else {
                    alert(_authenticationResult.Message);
                    //this.notificationService.printErrorMessage(_authenticationResult.Message);
                }
            });
    };
}