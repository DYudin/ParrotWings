import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Registration } from '../../core/domain/registration';
import { OperationResult } from '../../core/domain/operationResult';
import { AuthenticationService } from '../../core/services/authentication.service';
//import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'register',
    providers: [AuthenticationService], //, NotificationService
    templateUrl: './app/components/account/register.component.html'
})
export class RegisterComponent implements OnInit {

    private _newUser: Registration;

    constructor(public authService: AuthenticationService,
        //public notificationService: NotificationService,
        public router: Router) { }

    ngOnInit() {
        this._newUser = new Registration('', '', '');
    }

    register(): void {
        var _registrationResult: OperationResult = new OperationResult(false, '');
        this.authService.register(this._newUser)
            .subscribe(res => {
                _registrationResult.Succeeded = res.Succeeded;
                _registrationResult.Message = res.Message;

            },
            error => console.error('Error: ' + error),
            () => {
                if (_registrationResult.Succeeded) {
                    alert(_registrationResult.Message);
                    //this.notificationService.printSuccessMessage('Dear ' + this._newUser.Username + ', please login with your credentials');
                    this.router.navigate(['account/login']);
                }
                else {
                    alert(_registrationResult.Message);
                    //this.notificationService.printErrorMessage(_registrationResult.Message);
                }
            });
    };
}