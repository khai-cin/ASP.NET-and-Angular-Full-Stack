import { Component, inject, input, output } from '@angular/core';
import { RegisterCreds, User } from '../../../types/user';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private accountService = inject(AccountService);
  // input: is a decorator that allows a parent component to pass data to a child component.
  // In this case, it is used to receive a list of users from the Home component.
  usersFromHome = input.required<User[]>();
  // output: is a decorator that allows a child component to emit events to its parent component.
  // In this case, it is used to notify the parent component when the registration process is canceled.
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    this.accountService.register(this.creds).subscribe({
      next: user => {
        console.log('Successfully registered: ', user);
        this.cancel();
      },
      error: error => {
        console.log('Registration error: ', error);
      }
    })
  }

  // Emit false to indicate that registration has been canceled
  cancel() {
    this.cancelRegister.emit(false);
  }

}
