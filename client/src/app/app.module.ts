import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//Added http client module to make http request from angular to API
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { ToastrModule } from 'ngx-toastr';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';

//Here we have NgModule decorator with meta data.
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TooltipModule.forRoot(),
    BrowserAnimationsModule,
    FormsModule,
    //So now everything in the shared module is going to be available via our app module to our components in our application.
    SharedModule
  ],
  // forRoot() means that it's got some services or components that it needs to initialize along with the root module.
  // Now we're inside our root module (@NgModule).
  // We've got our bootstrap dropdown module and we need to add this for root to ensure that it loads up all of
  // the services that it needs with our root module.

  // In this app module file, this meta data providers array is empty.
  //Now, in previous versions of angular, we would have needed to add our service inside here,
  // but in current versions of angular, we use the provided in root (present in service file) instead.
  //Now on, angular service is a singleton.
  //Adding the custom interceptor in the provider array.
  // We need to specify our interceptor class with Multi as true, because we want to add this to the interceptors.
  // We don't want to replace the ones that come with ANGULAR (built-in interceptors) with our own interceptor.
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
