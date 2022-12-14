import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';

import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';

import { CurrencyValidatorDirective } from '../validators/currency-validator.directive';
import { LengthValidatorDirective } from '../validators/length-validator.directive';
import { TokenLengthValidatorDirective } from '../validators/token-length-validator.directive';

import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductComponent } from './product/product.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AppComponent } from './app/app.component';
import { ProductHistoryComponent } from './product-history/product-history.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ValidateComponent } from './validate/validate.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    MatFormFieldModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    RouterModule,
    MatInputModule,
    MatSelectModule,
    MatSlideToggleModule
  ],
  declarations: [
    ProductComponent,
    ProductDetailComponent,
    NotFoundComponent,
    AppComponent,
    CurrencyValidatorDirective,
    LengthValidatorDirective,
    ProductHistoryComponent,
    LoginComponent,
    RegisterComponent,
    ValidateComponent,
    TokenLengthValidatorDirective
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
