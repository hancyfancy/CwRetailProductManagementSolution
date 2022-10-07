import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';

import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit, OnDestroy {
  protected products: Product[] = [];
  protected displayedColumns: string[] = ['name', 'price', 'type', 'active', 'edit', 'delete'];
  protected dataSource: MatTableDataSource<Product> = new MatTableDataSource<Product>();

  @ViewChild(MatPaginator, { static: true }) protected paginator: MatPaginator = new MatPaginator(new MatPaginatorIntl(), ChangeDetectorRef.prototype);
  @ViewChild(MatSort, { static: true }) protected sort: MatSort = new MatSort();

  constructor(private productService: ProductService, private router: Router) { }

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
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      });
  }

  add(name: string, priceAsString: string, type: string, activeAsString: string): void {
    name = name.trim();
    if (!name) { return; }
    var price: number = +priceAsString;
    var active: boolean = (/true/i).test(activeAsString.toLowerCase());
    this.productService.addProduct({ name, price, type, active } as Product)
      .subscribe(product => {
        this.products.push(product);
      });
  }

  delete(product: Product): void {
    this.products = this.products.filter(p => p !== product);
    this.productService.deleteProduct(product.id)
      .then(() => {
        this.getProducts();
      })
      .catch((error) => {
        console.log("Promise rejected with " + JSON.stringify(error));
      });
  }

  goToDetails(): void {
    const navigationDetails: string[] = ['/details'];
    this.router.navigate(navigationDetails);
  }
}
