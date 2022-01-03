import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  // If we want what's inside the imports to be available everywhere else, then we also need to export the modules inside here as well. 
  //So what we also need to do is add another array in here for the exports, open up the array, 
  //and then we need to export the BSdropdown module and we also need to export the toaster module. 
  //We do not need to add the configuration when we export, we only need to add this configuration when we're importing. 
  //So it doesn't make much difference at the moment. But as we use more and more angular bootstrap components, we add other third party modules,
  // then our app module can get a bit out of control. And this is just a little bit of housekeeping to avoid that.
  exports: [
    BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }
