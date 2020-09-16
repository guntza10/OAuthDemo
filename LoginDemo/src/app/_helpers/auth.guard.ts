import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../_services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authenticationSurvice: AuthenticationService) {

  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // const currentUser = this.authenticationSurvice.currentUserValue;
    // if (currentUser) {
    //   return true;
    // }

    // this.router.navigate(['login'], { queryParams: { returnUlr: state.url } });
    // return false;
    return true;
  }

}
