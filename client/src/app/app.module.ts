import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//Added http client module to make http request from angular to API
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

//Here we have NgModule decorator with meta data.
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    TooltipModule.forRoot(),
    BrowserAnimationsModule,
    FormsModule,
    BsDropdownModule.forRoot()
  ],
// forRoot() means that it's got some services or components that it needs to initialize along with the root module.
// Now we're inside our root module (@NgModule).
// We've got our bootstrap dropdown module and we need to add this for root to ensure that it loads up all of
// the services that it needs with our root module.
  
  // In this app module file, this meta data providers array is empty.
  //Now, in previous versions of angular, we would have needed to add our service inside here,
  // but in current versions of angular, we use the provided in root (present in service file) instead.
  //Now on, angular service is a singleton.
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
