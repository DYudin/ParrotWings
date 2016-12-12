import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Registration } from '../../core/domain/registration';
import { OperationResult } from '../../core/domain/operationResult';
import { AuthenticationService } from '../../core/services/authentication.service';

@Component({
    selector: 'register',
    providers: [AuthenticationService], 
    templateUrl: './app/components/account/register.component.html'
})
export class RegisterComponent implements OnInit {

    private _newUser: Registration;

    constructor(public authService: AuthenticationService,
        public router: Router) { }

    ngOnInit() {
        this._newUser = new Registration('', '', '');
    }
    
    onPassModified(value) { 
        if (value != this._newUser.PasswordConfirmation) {
            this._newUser.PasswordConfirmed = false;
        }
        else {
            this._newUser.PasswordConfirmed = true;
        }
    }

    onPassConfModified(value) {
        if (value != this._newUser.Password) {
            this._newUser.PasswordConfirmed = false;
        }
        else {
            this._newUser.PasswordConfirmed = true;
        }
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