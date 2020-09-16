import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterUser } from '../_models/RegisterUser';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly endpointUrl = "https://localhost:4000/api/user/";

  constructor(private http: HttpClient) { }

  AuthenticateUser() {
    return this.http.get<User>(this.endpointUrl + "Authenticate")
  }

  RegisterUser(user: RegisterUser) {
    return this.http.post(this.endpointUrl + "CreateUser", user)
  }
}
