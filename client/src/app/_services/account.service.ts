import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';

// We have an injectable decorator here. This means that this service can be injected 
//into other components or other services in our application.
@Injectable({
  //This providedIn: 'root' is a meta data. In current versions of angular, 
  //we use this provided in root instead.
  //Now on, angular service is a singleton. When we inject it into a component and it's initialized, 
  //it will stay initialized until our app is disposed of.
  //The user closes the browser for instance, or they move away from our application, at that point, 
  //our service is destroyed, but if they stay on our application, then this account service
  //will stay initiated through the lifetime that the application is around.
  providedIn: 'root'
})

//This service is going to be used to make requests to our API. so defining the baseurl property as below.
export class AccountService {

  //when we want to set this property to something we use equals.
  //If we wanted to make it a type (data type) of something, we would use a colon.
  baseUrl = 'https://localhost:5001/api/';

  //ReplaySubject is a kind of buffer object is going to store the values inside here.
  //And any time a subscriber subscribes to this observable, it's going to omit the last value inside it,
  //or however many values inside it that we want to admit.
  //Here we're going to store User inside this replay subject and How many versions of the current user are we going to store
  // it's just a user object for the current user, so we only need one of them. so bufferzise is set as 1.
  //typescript became stricter. so we have to mention it explicitly as below " | null" when we need our observable accepting nulls.
  private currentUserSource = new ReplaySubject<User | null>(1)

  //As this is going to be an observable we give it the dollar sign at the end.
  currentUser$ = this.currentUserSource.asObservable();

  //we're going to inject the HTTP client into our account service.
  constructor(private http: HttpClient) { }

  //The login is going to receive our credentials from the login form from our nav bar. So we'll just call it as 'model'
  // and give it a type of 'any' for the time being.
  //Anything what we writing here within pipe() is going to be RxJS operator.
  //JSON.stringify - converts a JavaScript value to a JavaScript Object Notation (JSON) string.
  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          //Storing the user object in JSON format
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
