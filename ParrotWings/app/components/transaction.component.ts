import { Component, OnInit } from '@angular/core';
import { DataService } from '../core/services/data.service';
import { AuthenticationService } from '../core/services/authentication.service';
import { OperationResult } from '../core/domain/operationResult';
import { Router } from '@angular/router';
import { User } from '../core/domain/user';
import { Transaction } from '../core/domain/transaction';
import { Pipe, PipeTransform } from "@angular/core";
import { Ng2AutoCompleteModule } from 'ng2-auto-complete';

@Component({
    selector: 'transaction',
    templateUrl: './app/components/transaction.component.html'
    
})
export class TransactionComponent implements OnInit {
    private _transactionsAPI: string = 'api/transaction/alltransactions';
    private _sendMoneyAPI: string = 'api/transaction/sendmoney';
    private _currentUserInfo: string = 'api/transaction/currentuserinfo';
    private _usersAPI: string = 'api/transaction/allusers';

    private _transactions: Array<Transaction> = [];  
    private _usersStr: Array<string> = [];
    private _transaction: Transaction;
    private _currentUser: User;
    private _isUserAuthenticated: boolean;

    constructor(public transactionService: DataService,
        public authService: AuthenticationService) {       
    }

    ngOnInit() {
        //let _isUserAuthenticated = this.authService.isUserAuthenticated();
        this._currentUser = new User('');
        this.getCurrentUserInfo();
        this.getUsers();        
        this.getTransactions();        
		this._transaction = new Transaction();
    }

    onUserModified(value) { 
        if (value === this._currentUser.UserName) {
            this._transaction.CorrespondedUserValid = false;
        }
        else {
            this._transaction.CorrespondedUserValid = true;
        }
    }

    onAmountModified(value) { 
        if (value <= 0) {
            this._transaction.AmountValid = false;
        }
        else {
            if (value > this._currentUser.CurrentBalance) {
                this._transaction.AmountValid = false;
            }
            else {
                this._transaction.AmountValid = true;
            }
        }       
    }

    getCurrentUserInfo(): void {
        this.transactionService.set(this._currentUserInfo);      
        this.transactionService.get()
            .subscribe(res => {
                var data: any = res.json();
                this._currentUser = data;
            },
            error => alert(error.json().Message));
    }
    
    getTransactions(): void {
        this.transactionService.set(this._transactionsAPI);
        let self = this;
        self.transactionService.get()
            .subscribe(res => {
                var data: any = res.json();               
                self._transactions = data;
                self._transactions.sort((a, b) => {
                    var dateA = new Date(a.Date).getTime();
                    var dateB = new Date(b.Date).getTime();
                    return dateA < dateB ? 1 : -1;  
                });
                },
            error => alert(error.json().Message));
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
            error => alert(error.json().Message));
    }   

    send(): void {       
        this.transactionService.set(this._sendMoneyAPI);        
        this._transaction.Date = new Date();

        // send transaction
        this.transactionService.post(this._transaction)
            .subscribe(
            () => {
                //refresh ui
                this.getCurrentUserInfo();
                this.getTransactions();
            },
            error => alert(error.json().Message));
    };
}