import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  protected productId: bigint = 0n;
  protected actionText: string = '';

  constructor(private productService: ProductService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.productId = BigInt(this.route.snapshot.paramMap.get('id')!);
    this.actionText = this.productId > 0 ? 'Edit' : 'Add';
  }

  add(name: string, priceAsString: string, type: string, activeAsString: string): void {
    name = name.trim();
    if (!name) { return; }
    var price: number = +priceAsString;
    var active: boolean = (/true/i).test(activeAsString.toLowerCase());
    this.productService.addProduct({ name, price, type, active } as Product)
      .subscribe(product => {
        //Need to convert addProduct method into promise
        //this.products.push(product);
      });
  }
}
