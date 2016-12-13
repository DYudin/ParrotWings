import { Http, Response } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class DataService {

    public _baseUri: string;

    constructor(public http: Http) {

    }

    set(baseUri: string): void {
        this._baseUri = baseUri;		
    }

    get() {
        var uri = this._baseUri;

        return this.http.get(uri)
            .map(response => (<Response>response));
    }

    post(data?: any, mapJson: boolean = true) {
        if (mapJson)
            return this.http.post(this._baseUri, data)
                .map(response => <any>(<Response>response).json());
        else
            return this.http.post(this._baseUri, data);
    }
}