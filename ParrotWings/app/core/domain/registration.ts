export class Registration {
    Username: string;
    Password: string;
    Email: string;
    PasswordConfirmation: string;    
    PasswordConfirmed: boolean;

    constructor(username: string,
        password: string,
        email: string) {
        this.Username = username;
        this.Password = password;
        this.Email = email;
        this.PasswordConfirmed = true;
    }
}