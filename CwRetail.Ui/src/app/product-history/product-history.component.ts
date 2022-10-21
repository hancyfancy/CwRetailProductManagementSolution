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
  protected history: ProductHistory[] = [];

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
    var firstAuditJsonString: string = firstAudit.json;
    var firstAuditProducts: Product[] = JSON.parse(firstAuditJsonString) as Product[];
    this.history.push(new ProductHistory(firstAuditProducts[0], 'Original'));
    this.history.push(new ProductHistory(firstAuditProducts[1], firstAudit.dateTime.toUTCString()));

    if (this.productAuditService.productAudits.length > 1) {
      for (let i = 1; i < this.productAuditService.productAudits.length; i++) {
        var element: ProductAudit = this.productAuditService.productAudits[i];
        var jsonString: string = element.json;
        var products: Product[] = JSON.parse(jsonString) as Product[];
        this.history.push(new ProductHistory(products[1], element.dateTime.toUTCString()));
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
