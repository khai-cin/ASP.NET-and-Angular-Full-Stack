import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected creds: any = {};

  login() {
    // this is an observable, so we need to subscribe to it to get the result
    this.accountService.login(this.creds).subscribe({
      next: result => {
        this.creds = {}; 
      },
      error: error => alert(error.message)
    });
  }

  logout() {
    this.accountService.logout();
  }

}
