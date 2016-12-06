import { Component, OnInit } from '@angular/core';
import { DataService } from '../core/services/data.service';
import { AuthenticationService } from '../core/services/authentication.service';

import { User } from '../core/domain/user';
import { Transaction } from '../core/domain/transaction';

@Component({
    selector: 'transaction',
    templateUrl: './app/components/transaction.component.html'
})
export class TransactionComponent implements OnInit {
    private _transactionsAPI: string = 'api/transactions/';
    private _transactions: Array<Transaction>;
    private verifyAmountResult: boolean;

    constructor(public transactionService: DataService,
        public authService: AuthenticationService) {
       
    }

    ngOnInit() {
        this.transactionService.set(this._transactionsAPI);
        this.getTransactions();
    }

    getTransactions(): void {
        let self = this;
        self.transactionService.get()
            .subscribe(res => {

                    var data: any = res.json();

                    self._transactions = data.Items;
                },
                error => console.error('Error: ' + error));
    }

    verifyAmount(): void {
        let user = this.authService.getLoggedInUser();
        let self = this;
        self.transactionService.post(user)
            .subscribe(res => {

                self.verifyAmountResult = true;
            },
            error => {
                console.error('Error: ' + error);
                self.verifyAmountResult = false;
            });
    }
}