import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import * as CryptoJS from 'crypto-js';

import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductComponent implements OnInit, OnDestroy {
  protected products: Product[] = [];
  protected displayedColumns: string[] = ['name', 'price', 'type', 'active', 'edit', 'delete'];
  protected dataSource: MatTableDataSource<Product> = new MatTableDataSource<Product>();

  @ViewChild(MatPaginator, { static: true }) protected paginator: MatPaginator = new MatPaginator(new MatPaginatorIntl(), ChangeDetectorRef.prototype);
  @ViewChild(MatSort, { static: true }) protected sort: MatSort = new MatSort();

  constructor(private productService: ProductService, private router: Router, private changeDetectorRefs: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.paginator.pageSizeOptions = [5, 10, 20, 50, 100];
    this.paginator.pageSize = 5;

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.getProducts();
  }

  ngOnDestroy(): void {
  }

  getProducts(): void {
    this.productService.getProducts()
      .then((products) => {
        this.products = this.dataSource.data = products!;
        this.changeDetectorRefs.detectChanges();
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      });
  }

  delete(product: Product): void {
    if (confirm("Delete product " + product.name + "?")) {
      this.productService.deleteProduct(product.id)
        .then(() => {
          this.getProducts();
        })
        .catch((error) => {
          console.log("Promise rejected with " + JSON.stringify(error));
        });
    }
  }

  goToDetails(product: Product = new Product()): void {
    const navigationDetails: string[] = ['/details', this.encrypt(product)];
    this.router.navigate(navigationDetails);
  }

  encrypt(data: Product) : string {
    try {
      return CryptoJS.AES.encrypt(JSON.stringify(data, (_, v) => typeof v === 'bigint' ? v.toString() : v), this.productService.secretKey).toString();
    } catch (e) {
      console.log(e);
      throw (e);
    }
  }
}
