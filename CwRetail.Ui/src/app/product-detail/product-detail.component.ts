import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';

import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';
import { Settings } from '../../settings';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  protected product: Product = new Product();
  protected actionText: string = '';

  constructor(private productService: ProductService, private settings: Settings, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.product = this.decrypt(this.route.snapshot.paramMap.get('product')!);
    this.actionText = this.product.productId > 0 ? 'Edit' : 'Add';
  }

  addOrEdit(name: string, price: number, typeAsString: string, activeAsString: string): void {
    name = name.trim();
    typeAsString = typeAsString.trim();
    activeAsString = activeAsString.trim();

    if ((!name) || (!price) || (!typeAsString) || (!activeAsString)) { return; }

    var active: boolean = (/true/i).test(activeAsString.toLowerCase());
    var type: number = typeAsString == 'books' ? 1 : typeAsString == 'electronics' ? 2 : typeAsString == 'food' ? 3 : typeAsString == 'furniture' ? 4 : typeAsString == 'toys' ? 5 : 0;

    var originalProduct: Product = this.decrypt(this.route.snapshot.paramMap.get('product')!);

    if (originalProduct.productId > 0) {

      var dynObj: any = {};

      if (originalProduct.name != name) {
        dynObj.name = name;
      }

      if (originalProduct.price != price) {
        dynObj.price = price;
      }

      if (originalProduct.type.toString().toUpperCase() != typeAsString.toUpperCase()) {
        dynObj.type = type;
      }

      if (originalProduct.active != active) {
        dynObj.active = active;
      }

      this.productService.updateProduct(originalProduct.productId, dynObj)
        .then(() => {
          this.goToProducts();
        })
        .catch((error) => {
          console.log("Promise rejected with " + JSON.stringify(error));
        });
    }
    else
    {
      this.productService.addProduct(new Product(0n, name, price, type, active))
        .then(() => {
          this.goToProducts();
        })
        .catch((error) => {
          console.log("Promise rejected with " + JSON.stringify(error));
        });
    }
  }

  goToProducts(): void {
    const navigationDetails: string[] = ['/products'];
    this.router.navigate(navigationDetails);
  }

  decrypt(data : string) : Product {
    try {
      const bytes = CryptoJS.AES.decrypt(data, this.settings.secretKey);
      return JSON.parse(bytes.toString(CryptoJS.enc.Utf8)) as Product;
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
