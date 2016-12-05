﻿import { Injectable } from '@angular/core';

//declare var alertify: any;

@Injectable()
export class NotificationService {
    private _notifier: any;
    constructor() {
    }

    printSuccessMessage(message: string) {

        alert(message);
        //this._notifier.success(message);
    }

    printErrorMessage(message: string) {
        alert(message);
        //this._notifier.error(message);
    }

    printConfirmationDialog(message: string, okCallback: () => any) {
        
        this._notifier.confirm(message, function (e) {
            if (e) {
                okCallback();
            } else {
            }
        });
    }
}