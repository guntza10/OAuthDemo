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

  constructor(private router: Router, private authenticationService: AuthenticationService) {

  }

  logOut() {
    
  }

}
