
import { Injectable } from '@angular/core';

@Injectable()
export class ErrorService {

    private _errorBody: string;

    constructor() {
    }    

    public setErrorBody(errorBody: string) {
        this._errorBody = errorBody;
    }

    public getErrorBody() {
        return this._errorBody;
    }
}