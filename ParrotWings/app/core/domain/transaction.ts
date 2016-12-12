export class Transaction {
    Amount: number;
    CorrespondedUser: string;
    Date: Date;
    ResultingBalance: number;
    AmountValid: boolean;
    CorrespondedUserValid: boolean;

    constructor(        
		//amount: number,
		//recepientName: string,
		//date: Date,
		//resultingBalance: number
		) 
    {
        this.AmountValid = true;
        this.CorrespondedUserValid = true;
        //this.Amount = amount;
        //this.RecepientName = recepientName;
		//this.Date = date;
        //this.ResultingBalance = resultingBalance;
    }
}