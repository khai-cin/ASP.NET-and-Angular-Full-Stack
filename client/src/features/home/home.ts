import { Component, Input, signal } from '@angular/core';
import { Register } from "../account/register/register";
import { User } from '../../types/user';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [FormsModule, Register],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  @Input({required: true}) usersFromApp: User[] = [];
  protected registerMode = signal(false);

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }

}
