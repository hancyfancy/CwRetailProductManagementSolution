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

  addOrEdit(name: string, price: number, typeAsString: string, activeAsString: string): void {
    name = name.trim();
    typeAsString = typeAsString.trim();
    activeAsString = activeAsString.trim();

    if ((!name) || (!price) || (!typeAsString) || (!activeAsString)) { return; }

    var active: boolean = (/true/i).test(activeAsString.toLowerCase());
    var type: number = typeAsString == 'books' ? 1 : typeAsString == 'electronics' ? 2 : typeAsString == 'food' ? 3 : typeAsString == 'furniture' ? 4 : typeAsString == 'toys' ? 5 : 0;

    if (this.product.id > 0) {

      var dynObj: any = {};

      console.log(this.product.name);
      console.log(name);

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
      console.log(name);
      console.log(price);
      console.log(type);
      console.log(active);

      this.productService.addProduct(new Product(0n, name, price, type, active))
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
