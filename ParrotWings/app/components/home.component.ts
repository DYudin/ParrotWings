import { Component, OnInit } from '@angular/core';
import { DataService } from '../core/services/data.service';

@Component({
    selector: 'home',
    templateUrl: './app/components/home.component.html'
})
export class HomeComponent implements OnInit {
    //private _welcomeStringAPI: string = 'api/home/welcomestring';
    //private welcomeString: string;

    constructor(public dataService: DataService) {
    }

    ngOnInit() {
        //this.dataService.set(this._welcomeStringAPI);

        //this.dataService.get()
        //    .subscribe(res => {

        //        var data: any = res.json();

        //        this.welcomeString = data;
        //    },
        //    error => console.error('Error: ' + error));
    }
}