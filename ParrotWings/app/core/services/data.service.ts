import { Http, Response, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataService {

    public _baseUri: string;
    private _authHeaderValue: string;

    constructor(public http: Http) {
    }

    set(baseUri: string): void {
        this._baseUri = baseUri;		
    }

     setAuthHeaderValue(authHeaderValue: string): void {
        this._authHeaderValue = authHeaderValue;
    }

    private createAuthorizationHeader(headers: Headers) {
        if (this._authHeaderValue != undefined) {
            headers.append('Authorization', 'Basic ' +
                this._authHeaderValue);
        }
    }

    get() {
        let headers = new Headers();
        this.createAuthorizationHeader(headers);
        var uri = this._baseUri;

        return this.http.get(uri, {
            headers: headers})
            .map(response => (<Response>response));
    }

    post(data?: any, mapJson: boolean = true) {
        let headers = new Headers();
        this.createAuthorizationHeader(headers);

        if (mapJson)
            return this.http.post(this._baseUri, data, {
                headers: headers})
                .map(response => <any>(<Response>response).json());
        else
            return this.http.post(this._baseUri, data);
    }
}