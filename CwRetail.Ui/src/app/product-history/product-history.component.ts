import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../../models/product';
import { ProductAuditService } from '../../services/product-audit.service';
import { Settings } from '../../settings';

@Component({
  selector: 'app-product-history',
  templateUrl: './product-history.component.html',
  styleUrls: ['./product-history.component.css']
})
export class ProductHistoryComponent implements OnInit {

  constructor(private productAuditService: ProductAuditService, private settings: Settings, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
  }

  getHistory(): void {
    var originalProduct: Product = this.decrypt(this.route.snapshot.paramMap.get('product')!);

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
