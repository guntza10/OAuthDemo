import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AlertComponent } from './_components/alert.component';
import { AccountComponent } from './_services/account.component';
import { Account2Component } from './_services/account2.component';
import { AccountThaiComponent } from './_services/account-thai.component';
import { Accountthai2Component } from './_services/accountthai2.component';
import { AccountThai3Component } from './_services/account-thai3.component';

@NgModule({
  declarations: [
    AppComponent,
    AlertComponent,
    AccountComponent,
    Account2Component,
    AccountThaiComponent,
    Accountthai2Component,
    AccountThai3Component
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
