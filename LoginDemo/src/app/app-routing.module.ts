import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './_helpers/auth.guard';


const routes: Routes = [
  {
    path: '', redirectTo: 'Home', pathMatch: 'full'
  },
  {
    path: 'Home', component: HomeComponent, 
    // canActivate: [AuthGuard]
  },
  {
    path: 'Login', component: LoginComponent
  },
  {
    path: '**', redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
