import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

//This has been decorated with injectable but we are not going to inject this into any other component.
@Injectable({
  providedIn: 'root'
})

//We selected the interface CanActivate during this template creation, which this auth class should be inherited from. 
//CanActivate is an interface that a class can implement to be a guard deciding if a route can be activated.
//If all guards return true, navigation continues. 
//If any guard returns false, navigation is cancelled. 
//If any guard returns a UrlTree, the current navigation is cancelled 
//and a new navigation begins to the UrlTree returned from the guard.
export class AuthGuard implements CanActivate {

  //we're going to add a constructor so that we can inject our required services.
  constructor(private accountService: AccountService, private toastr: ToastrService) {}
  
  //Implementing the canActivate method of the above interface CanActivate.
  canActivate() : Observable<boolean> {
    //when we use AuthGuard, it automatically subscribes to any observables. So we need to use the pipe and then the map in this case.
    return this.accountService.currentUser$.pipe(
      map(user => {
          if(user) {
            return true;
          }
          return false;
          this.toastr.error('You shall not pass!');          
        })
    )
  }  
}
