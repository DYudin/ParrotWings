import { Component, OnInit } from '@angular/core';
import { DataService } from '../core/services/data.service';
import { AuthenticationService } from '../core/services/authentication.service';
import { OperationResult } from '../core/domain/operationResult';
import { Router } from '@angular/router';
import { User } from '../core/domain/user';
import { Transaction } from '../core/domain/transaction';
import { Pipe, PipeTransform } from "@angular/core";
import { Ng2AutoCompleteModule } from 'ng2-auto-complete';
import { OrderByPipe } from '../core/services/OrderByPipe';

@Component({
    selector: 'transaction',
    templateUrl: './app/components/transaction.component.html'
    
})
export class TransactionComponent implements OnInit {
    private _transactionsAPI: string = 'api/transaction/alltransactions';
	private _verifyAmountAPI: string = 'api/transaction/verifyamount';
    private _verifyUserAPI: string = 'api/transaction/verifyuser';
    private _sendMoneyAPI: string = 'api/transaction/sendmoney';
    private _currentUserInfo: string = 'api/transaction/currentuserinfo';
    private _usersAPI: string = 'api/transaction/allusers';

    private _transactions: Array<Transaction> = [];  
    private _usersStr: Array<string> = [];
    private _verifyAmountResult: boolean;
    private _transaction: Transaction;
    private _currentUser: User;
    private _isUserAuthenticated: boolean;

    constructor(public transactionService: DataService,
        public authService: AuthenticationService) {
       
    }

    ngOnInit() {
        let _isUserAuthenticated = this.authService.isUserAuthenticated();
        this._currentUser = new User('');
        this.getCurrentUserInfo();
        this.getUsers();        
        this.getTransactions();        
		this._transaction = new Transaction();
    }

    onUserModified(value) { // without type info
        if (value == this._currentUser.UserName) {
            this._transaction.CorrespondedUserValid = false;
        }
        else {
            this._transaction.CorrespondedUserValid = true;
        }
    }

    onAmountModified(value) { // without type info   
        if (value > this._currentUser.CurrentBalance) {
            this._transaction.AmountValid = false;
        }
        else {
            this._transaction.AmountValid = true;
        }
    }

    getCurrentUserInfo(): void {
        this.transactionService.set(this._currentUserInfo);      
        this.transactionService.get()
            .subscribe(res => {
                var data: any = res.json();
                this._currentUser = data;
            },
            error => console.error('Error: ' + error));
    }
    
    getTransactions(): void {
        this.transactionService.set(this._transactionsAPI);
        let self = this;
        self.transactionService.get()
            .subscribe(res => {
                var data: any = res.json();
                self._transactions = data;
                },
                error => console.error('Error: ' + error));
    }

    getUsers(): void {
        this.transactionService.set(this._usersAPI);
        let self = this;
        self.transactionService.get()
            .subscribe(res => {
                var data: any = res.json();              
                for (let us of data) {
                    if (us.UserName != this._currentUser.UserName) {
                        this._usersStr.push(us.UserName);
                    }
                }
            },
            error => console.error('Error: ' + error));
    }

    verifyAmount(): void {
	     this.transactionService.set(this._verifyAmountAPI);
        //let user = this.authService.getLoggedInUser();
        let self = this;
        self.transactionService.post(this._transaction.Amount)
            .subscribe(res => {

                self._verifyAmountResult = true;
            },
            error => {
                console.error('Error: ' + error);
                self._verifyAmountResult = false;
            });
    }

    send(): void {       
        this.transactionService.set(this._sendMoneyAPI);        
        var _sendResult: OperationResult = new OperationResult(false, '');
        this._transaction.Date = new Date(); // toDO now

        // send transaction
        this.transactionService.post(this._transaction)
            .subscribe(res => {
                _sendResult.Succeeded = res.Succeeded;
                _sendResult.Message = res.Message;
            },
            error => console.error('Error: ' + error),
            () => {
                if (_sendResult.Succeeded) {
                    alert(_sendResult.Message);

                    //refresh ui
                    this.getCurrentUserInfo();
                    this.getTransactions();
                }
                else {
                    alert(_sendResult.Message);
                    //this.notificationService.printErrorMessage(_authenticationResult.Message);
                }
            });      
    };
}