import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../_models/User';
import { catchError, map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { UserLogin } from '@app/_models/userLogin';
import { RefreshToken } from '@app/_models/RefreshToken';
import { JwtInterceptor } from '@app/_helpers/jwt.interceptor';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
    // this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUserSubject = new BehaviorSubject<User>(null);
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(user: UserLogin) {
    return this.http.post<User>(`${environment.endpointUrl}/Authenticate`, user, { withCredentials: true }).pipe(
      map(user => {
        // localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        this.startRefreshTokenTimer();
        return user;
      })
    );
  }

  logout() {
    // localStorage.removeItem('currentUser');
    this.http.post<any>(`${environment.endpointUrl}/RevokeToken`, {}, { withCredentials: true }).subscribe();
    this.stopRefreshTokenTimer();
    this.currentUserSubject.next(null);
  }

  refreshToken() {
    return this.http.post<User>(`${environment.endpointUrl}/RefreshToken`, {}, { withCredentials: true })
      .pipe(
        map(user => {
          this.currentUserSubject.next(user);
          this.startRefreshTokenTimer();
          return user;
        }));
  }

  private refreshTokenTimeout;

  private startRefreshTokenTimer() {
    console.log(`jwtToken : ${this.currentUserValue.token}`);
    const jwtTokenPayload = JSON.parse(atob(this.currentUserValue.token.split('.')[1]));
    
    console.log(`jwtTokenPayload : ${JSON.stringify(jwtTokenPayload)}`)
    console.log(`jwtToken expires : ${jwtTokenPayload.exp}`);

    const expires = new Date(jwtTokenPayload.exp * 1000);
    console.log(`expires : ${expires}`);
    console.log(`expires.getTime() : ${expires.getTime()}`);
    console.log(`Date.now() : ${Date.now()}`);

    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    console.log(`timeout : ${timeout}`);
    this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
