import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";
import { User } from '../types/user';
import { Register } from '../features/account/register/register';

@Component({
  selector: 'app-root',
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  // dependency injection - dependency injection is a 
  // design pattern used to implement IoC (Inversion of Control), 
  // allowing the creation of dependent objects outside of a class and providing those objects to a class in different ways.



  // mew way of doing dependency injection
  private accountService = inject(AccountService);
  private http = inject(HttpClient);
  protected readonly title = 'Dating App';
  // signal - a signal is a reactive primitive that holds a value and notifies subscribers when the value changes.
  protected users = signal(<User[]>[]);

  // contructor injection - the dependencies are provided through a class constructor.
  // old way of doing dependency injection
  // constructor(private http: HttpClient) {}


  // lifecycle hook - ngOnInit is a lifecycle hook that is called after Angular has initialized all data-bound properties of a directive.
  // It is a good place to put initialization logic for the component.
    async ngOnInit() {
      // an observable is a data stream that can emit multiple values over time.
      // subscribe is a method that is used to subscribe to an observable.
      // when the observable emits a value, the callback function is called with the emitted value.
      // this.http.get('https://localhost:5001/api/members').subscribe({
      //   next: response => this.members.set(response),
      //   error: error => console.log(error),
      //   complete: () => console.log('Request completed') // automatically unsubscribes after completion
      // })

      this.users.set(await this.getMembers());
      this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;

    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  async getMembers() {
    try {
      return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));
    }
    catch (error) {
      console.log(error);
      throw error;
    }
  }
}
