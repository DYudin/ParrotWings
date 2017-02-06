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
        this.authService.register(this._newUser)
            .subscribe(
            () => {
                alert("Registration succeeded");
                this.router.navigate(['account/login']);
            },
                error => alert(error.json().Message));
    }
}