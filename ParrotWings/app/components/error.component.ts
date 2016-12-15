import { Component } from '@angular/core';
import { ErrorService } from '../core/services/error.service';

@Component({
    selector: 'home',
    templateUrl: './app/components/error.component.html'
})
export class ErrorComponent {
    constructor(public errorService: ErrorService) {
        this._errorBody = errorService.getErrorBody();
    }

    private _errorBody: string;
}