import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent  implements OnInit {
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient,private accountService: AccountService) {

  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user')!);
    this.accountService.setCurrentUser(user);
  }

  getUsers() {
    this.http.get('https://localhost:5045/api/users').subscribe({
      next: response => this.users = response,
      error: err => console.error(err),
      complete: () => console.log('The observable is completed!')
    });
  }
  
  ngOnInit(): void {
    this.setCurrentUser();
    this.getUsers();
  }
}
