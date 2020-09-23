import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '@app/_models/User';
import { UserLogin } from '@app/_models/userLogin';
import { AuthenticationService } from '@app/_services/authentication.service';
import { UserService } from '@app/_services/user.service';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  returnUrl: string;
  userLogin: UserLogin;
  error = '';
  userData: User;

  constructor(private router: Router, private route: ActivatedRoute, private fb: FormBuilder, private authService: AuthenticationService) {
    if (authService.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      'username': [''],
      'password': ['']
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get formUserLogin() {
    return this.loginForm.controls;
  }

  onSubmit() {

    // this.userLogin = new UserLogin;
    // this.userLogin.username = this.formUserLogin.username.value;
    // this.userLogin.password = this.formUserLogin.password.value;

    this.authService.login(this.loginForm.value)
      .subscribe(res => {
        this.userData = res;
        console.log(`username : ${this.userData.username}`);
        console.log(`returnUrl : ${this.returnUrl}`);
        this.router.navigate([this.returnUrl]);
        // this.router.navigate(['home', { userName: this.userData.username }]);
      }, err => {
        this.error = err;
      });
  }
}
