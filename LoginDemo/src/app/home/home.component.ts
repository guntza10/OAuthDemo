import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { User } from '@app/_models/User';
import { AuthenticationService } from '@app/_services/authentication.service';
import { UserService } from '@app/_services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  users: User[];
  userLogin: User;

  constructor(private userService: UserService, private authService: AuthenticationService, private route: ActivatedRoute) {
    this.userLogin = this.authService.currentUserValue;
    // this.authService.currentUser.subscribe(user => {
    //   this.userLogin = user;
    // });
  }

  ngOnInit(): void {
    this.userService.GetAll()
      .subscribe(users => {
        this.users = users;
      });
  }
}
