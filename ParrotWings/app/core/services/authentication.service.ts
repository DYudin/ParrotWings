import { Http, Response, Request } from '@angular/http';
import { Injectable } from '@angular/core';
import { DataService } from './data.service';
import { Registration } from '../domain/registration';
import { User } from '../domain/user';
import { Credentials } from '../domain/credentials';

@Injectable()
export class AuthenticationService {

    private _accountRegisterAPI: string = 'api/account/register/';
    private _accountLoginAPI: string = 'api/account/login/';
    private _accountLogoutAPI: string = 'api/account/logout/';

    constructor(public accountService: DataService) { }

    register(newUser: Registration) {

        this.accountService.set(this._accountRegisterAPI);

        return this.accountService.post(JSON.stringify(newUser), false);
    }

    login(creds: Credentials) {
        this.accountService.set(this._accountLoginAPI);
        return this.accountService.post(JSON.stringify(creds), false);
    }

    logout() {
        this.accountService.set(this._accountLogoutAPI);
        return this.accountService.post(null, false);
    }

    isUserAuthenticated(): boolean {
        var _user: any  = localStorage.getItem('user');
        if (_user != null)
            return true;
        else
            return false;
    }

    getAuthValue(): string {
        var _authValue: string;

        if (this.isUserAuthenticated()) {
            var _authValueData = JSON.parse(localStorage.getItem('user'));
            _authValue = _authValueData.UserName;
        }

        return _authValue;
    }

    //getLoggedInUser(): User {
    //    var _user: User;

    //    if (this.isUserAuthenticated()) {
    //        var _userData = JSON.parse(localStorage.getItem('user'));
    //        _user = new User(_userData.Username);
    //    }

    //    return _user;
    //}
}