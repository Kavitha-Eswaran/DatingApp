import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
 
  //when we are injecting the service, if the automatic importing the service file 
  //at the top is not working, then we need to restart the Visual Studio code.
  //We need to define the variable accountService as public to access it in this template file.
  //we cannot use it in html file when it is private, but we can access it within this component file.
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login()
  {
    //An observable is lazy. It doesn't do anything until we subscribe to the observable.
    //So we have to subscribe and then we're going to get a response back from our server.
    //Now we know that this service method response from our server when we log in, 
    //we're going to get that user DTO returns to us.
    //For time being, we are just logging the response (user DTO object) in the console window
    // and just setting a bool variable as true. 
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
    }, error => {
    console.log(error);
    });
  }

  logout(){
    this.accountService.logout();
  }

}
