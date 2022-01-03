import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }
  // Here error indicates that the whole Http error response
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error.errors) {
                const modalStateErrors = [];
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key])
                  }
                }
                //flat is the built-in javascript method (in ES2019) to flatten the nested array.
                //So we need to add this ES2019 version in the lib property of the tsconfig.json file.
                //If the flat function is not recognized still, then we need to restart our application.
                //Note: we can use all the modern features of JavaScript inside typescript,
                //because it compiles it into something our browsers can understand.
                throw modalStateErrors.flat();
              }
              else {
                this.toastr.error(error.statusText, error.status)
              }
              break;
            case 401:
              this.toastr.error(error.statusText, error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = { state: { error: error.error } };
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        return throwError(error);
      })
    )
  }
}
