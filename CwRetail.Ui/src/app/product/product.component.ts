import { Component, OnInit, NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Product } from '../product';

@NgModule({
  imports: [
    HttpClientModule,
  ],
})

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  products: Product[] = [];

  constructor() { }

  ngOnInit(): void {
  }


}
