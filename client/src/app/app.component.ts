import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  //defined users property with any datatype. By default typescript provides type safety. 
  //Here we are truning off type safety by mentioning any datatype.
  users: any;

  //HttpClient is auto import from the module @angular/common/http.
  //& it performs Http Requests. This service is available as an injectable class with methods
  //to perform Http Requests.
  //Our http request to an API naturally is an asynchronous request.
  //Angular comes with the lifecycle events.
  //The lifecycle event takes place after the constructor is known as initialization.
  //So modify our appcomponent class implements the OnInit lifecycle hook 
  //and define the OnInit hook after the constructor using quickfix as shown below.
  constructor(private http:HttpClient){}
  
  //Here void is the return type of the OnInit method. it is a default return type.
  //Need to use this keyword to access the properties/variables defined within the class.
  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    //On mouse overing the get method, url is required input param and return type is observable.
    //Observables are lazy and they dont do anything unless somebody subscribes to them.
    //so we need to subscribe to get/use this observable data.
    //on mouse overing the subscribe method, it has three params as next, error, complete.
    //All these next,error & complete are functions. these are optional. 
    //We tell our subscriber that what we need to do next/in case of error/on completion of the http request.
    //We do want to do something validated when it comes back to us. So we need to get the response.
    //Here response is a parameter and contains our users data.
    this.http.get('https://localhost:5001/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    });
  }
}
