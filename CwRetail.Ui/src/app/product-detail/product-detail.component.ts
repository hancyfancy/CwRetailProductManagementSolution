import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Product } from '../../models/product';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  protected productId: bigint = 0n;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.productId = BigInt(this.route.snapshot.paramMap.get('id')!);
  }

}
