import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input - input decorator which is auto import from angular/core. 
  //This will be assigned in parent component template file.
  //@Input() usersFromHomeComponent: any;

  //@output - putput decorator which should be auto import from angular/core.
  //And what we need to do for output components is we're emitting events. So 
  //we need to use an event emitter.
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      //this.cancel() method is to close the signup form and navigate back to parent component.
      //We haven't got any routing set up at the moment, so we're just going to use conditionals to display and hide components.
      this.cancel();
    }, error => {
      console.log(error);
    })
  }

  cancel() {
    //when we click on the cancel button, we want to emit a value using this event emitter (assigning data to the veriable).
    //we're just going to emit, false. we want our cancel button to emit
    //false as we want to turn off the register mode in the home components.
    this.cancelRegister.emit(false);
  }
}
