import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../models/product';
import { ProductAudit } from '../../models/product-audit';
import { ProductHistory } from '../../models/product-history';
import { ProductAuditService } from '../../services/product-audit.service';
import { Settings } from '../../settings';

import * as CryptoJS from 'crypto-js';

@Component({
  selector: 'app-product-history',
  templateUrl: './product-history.component.html',
  styleUrls: ['./product-history.component.css']
})
export class ProductHistoryComponent implements OnInit {
  protected history: Product[] = [];

  constructor(private productAuditService: ProductAuditService, private settings: Settings, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getHistory();    
  }

  getHistory(): void {
    var originalProduct: Product = this.decrypt(this.route.snapshot.paramMap.get('product')!);
    this.productAuditService.getProductAuditUpdates(originalProduct.id)
      .then((productAudits) => {
        this.productAuditService.productAudits = productAudits!;
        this.loadHistory();
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      })
  }

  loadHistory(): void {
    if (this.productAuditService.productAudits.length == 0) {
      return;
    }

    var firstAudit: ProductAudit = this.productAuditService.productAudits[0];
    var firstAuditJsonString: string = firstAudit.json.toLowerCase();
    var firstAuditProducts: Product[] = JSON.parse(firstAuditJsonString) as Product[];

    var originalProduct: Product = new Product(firstAuditProducts[0].id, firstAuditProducts[0].name, firstAuditProducts[0].price, firstAuditProducts[0].type, firstAuditProducts[0].active);
    originalProduct.lastUpdated = 'Original';

    var modifiedProduct: Product = new Product(firstAuditProducts[1].id, firstAuditProducts[1].name, firstAuditProducts[1].price, firstAuditProducts[1].type, firstAuditProducts[1].active);
    modifiedProduct.lastUpdated = firstAudit.dateTime.toString();

    this.history.push(originalProduct);
    this.history.push(modifiedProduct);

    if (this.productAuditService.productAudits.length > 1) {
      for (let i = 1; i < this.productAuditService.productAudits.length; i++) {
        var element: ProductAudit = this.productAuditService.productAudits[i];
        var jsonString: string = element.json;
        var products: Product[] = JSON.parse(jsonString) as Product[];
        var currentModifiedProduct: Product = new Product(products[1].id, products[1].name, products[1].price, products[1].type, products[1].active);
        currentModifiedProduct.lastUpdated = element.dateTime.toString();
        this.history.push(currentModifiedProduct);
      }
    }
  }

  goToProducts(): void {
    const navigationDetails: string[] = ['/products'];
    this.router.navigate(navigationDetails);
  }

  decrypt(data: string): Product {
    try {
      const bytes = CryptoJS.AES.decrypt(data, this.settings.secretKey);
      return JSON.parse(bytes.toString(CryptoJS.enc.Utf8)) as Product;
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
