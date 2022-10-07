import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  private productId: bigint = 0n;
  protected path: string = '';

  constructor(private route: Route) { }

  ngOnInit(): void {
    this.path = this.route.path!;
  }

}
