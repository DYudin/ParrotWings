import { Component, OnInit } from '@angular/core';
import { DataService } from '../core/services/data.service';
import { AuthenticationService } from '../core/services/authentication.service';
import { OperationResult } from '../core/domain/operationResult';
import { Router } from '@angular/router';
import { User } from '../core/domain/user';
import { Transaction } from '../core/domain/transaction';

@Component({
    selector: 'transaction',
    templateUrl: './app/components/transaction.component.html'
})
export class TransactionComponent implements OnInit {
    private _transactionsAPI: string = 'api/transaction/alltransactions';
	private _verifyAmountAPI: string = 'api/transaction/verifyamount';
	private _verifyUserAPI: string = 'api/transaction/verifyuser';

    private _transactions: Array<Transaction>;
    private verifyAmountResult: boolean;
    private _transaction: Transaction;
    private _isUserAuthenticated: boolean;

    constructor(public transactionService: DataService,
        public authService: AuthenticationService) {
       
    }

    ngOnInit() {
        let _isUserAuthenticated = this.authService.isUserAuthenticated();

        //this.transactionService.set('api/transaction/alltransactions');
        //this.getTransactions();

		this._transaction = new Transaction();
    }

    getTransactions(): void {
		 this.transactionService.set(this._transactionsAPI);
        let self = this;
        self.transactionService.get()
            .subscribe(res => {

                    var data: any = res.json();

                    self._transactions = data.Items;
                },
                error => console.error('Error: ' + error));
    }

    verifyAmount(): void {
	     this.transactionService.set(this._verifyAmountAPI);
        //let user = this.authService.getLoggedInUser();
        let self = this;
        self.transactionService.post(this._transaction.Amount)
            .subscribe(res => {

                self.verifyAmountResult = true;
            },
            error => {
                console.error('Error: ' + error);
                self.verifyAmountResult = false;
            });
    }

	 send(): void {       
         var _sendResult: OperationResult = new OperationResult(false, '');
        this.transactionService.post(this._transaction)
            .subscribe(res => {
                _sendResult.Succeeded = res.Succeeded;
                _sendResult.Message = res.Message;
            },
            error => console.error('Error: ' + error),
            () => {
                if (_sendResult.Succeeded) {
                    alert(_sendResult.Message);
                    //this.notificationService.printSuccessMessage('Welcome back ' + this._user.Username + '!');
                    localStorage.setItem('transaction', JSON.stringify(this._transaction));
                    //this.router.navigate(['home']);
                }
                else {
                    alert(_sendResult.Message);
                    //this.notificationService.printErrorMessage(_authenticationResult.Message);
                }
            });
    };
}