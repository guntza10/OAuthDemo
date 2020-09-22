import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './_models/User';
import { AuthenticationService } from './_services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  currentUser: User;
  constructor(private router: Router, private authService: AuthenticationService) {
    this.authService.currentUser.subscribe(user => {
      this.currentUser = user;
    });
  }

  logOut() {
    this.authService.logout();
    this.router.navigate(['login']);
  }

}
