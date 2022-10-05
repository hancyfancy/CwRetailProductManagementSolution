import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ProductDetailComponent } from './product-detail/product-detail.component';
import { ProductComponent } from './product/product.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AppRoutingModule } from './app-routing.module';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'products', component: ProductComponent },
      { path: 'details', component: ProductDetailComponent },
      { path: '', redirectTo: '/products', pathMatch: 'full' },
      { path: '**', component: NotFoundComponent }
    ]),
    AppRoutingModule,
  ],
  declarations: [
    ProductComponent,
    ProductDetailComponent
  ],
  bootstrap: [ProductComponent]
})
export class AppModule { }
