import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as CryptoJS from 'crypto-js';

import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  protected product: Product = new Product();
  protected actionText: string = '';

  constructor(private productService: ProductService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.product = this.decrypt(this.route.snapshot.paramMap.get('product')!);
    this.actionText = this.product.id > 0 ? 'Edit' : 'Add';
  }

  addOrEdit(name: string, priceAsString: string, type: string, activeAsString: string): void {
    name = name.trim();
    priceAsString = priceAsString.trim();
    type = type.trim();
    activeAsString = activeAsString.trim();

    if ((!name) || (!priceAsString) || (!type) || (!activeAsString)) { return; }

    var price: number = +priceAsString;
    var active: boolean = (/true/i).test(activeAsString.toLowerCase());

    if (this.product.id > 0) {

      var dynObj: any = {};

      if (this.product.name != name) {
        dynObj.name = name;
      }

      if (this.product.price != price) {
        dynObj.price = price;
      }

      if (this.product.type != type) {
        dynObj.type = type;
      }

      if (this.product.active != active) {
        dynObj.active = active;
      }

      this.productService.updateProduct(dynObj)
        .then(() => {

        })
        .catch((error) => {
          console.log("Promise rejected with " + JSON.stringify(error));
        });
    }
    else
    {
      this.productService.addProduct({ name, price, type, active } as Product)
        .then(() => {
          
        })
        .catch((error) => {
          console.log("Promise rejected with " + JSON.stringify(error));
        });
    }
  }

  decrypt(data : string) : Product {
    try {
      const bytes = CryptoJS.AES.decrypt(data, this.productService.secretKey);
      return JSON.parse(bytes.toString(CryptoJS.enc.Utf8)) as Product;
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
