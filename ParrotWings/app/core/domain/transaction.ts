export class Transaction {
    Amount: number;
    CorrespondedUser: string;
    Date: Date;
    ResultingBalance: number;
    Outgoing: boolean;
    AmountValid: boolean;
    CorrespondedUserValid: boolean;

    constructor(
    ) 
    {
        this.AmountValid = true;
        this.CorrespondedUserValid = true;      
    }
}