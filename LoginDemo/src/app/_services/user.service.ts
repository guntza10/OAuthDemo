import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { RegisterUser } from '../_models/RegisterUser';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  AuthenticateUser() {
    return this.http.get<User>(`${environment.endpointUrl}/Authenticate`);
  }

  RegisterUser(user: RegisterUser) {
    return this.http.post(`${environment.endpointUrl}/CreateUser`, user);
  }
}
