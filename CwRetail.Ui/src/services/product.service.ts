import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductService {

  private domain: string = 'http://localhost:5138';
  private urlPrefix: string = this.domain + '/api/Product';
  public products: Product[] = [];

  constructor(private http: HttpClient) { }

  /** GET **/
  getProducts() : Observable<Product[]> {
    return this.http.get<Product[]>(this.urlPrefix + '/Get')
      .pipe(
        tap(_ => this.log('fetched products')),
        catchError(this.handleError<Product[]>('getProducts', []))
      );
  }


  /** POST **/
  addProduct(product: Product) {
    var addHttpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post<Product>(this.urlPrefix + '/Create', JSON.stringify(product, (_, v) => typeof v === 'bigint' ? v.toString() : v), addHttpOptions).pipe(
      tap((newProduct: Product) => this.log(`added product w/ id=${newProduct.id}`)),
      catchError(this.handleError<Product>('addProduct'))
    ).toPromise();
  }

  /** PATCH **/
  updateProduct(id: bigint, product: any) {
    var updateHttpOptions = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Id', id.toString())
    };
    return this.http.patch(this.urlPrefix + '/Edit', JSON.stringify(product, (_, v) => typeof v === 'bigint' ? v.toString() : v), updateHttpOptions).pipe(
      tap(_ => this.log(`updated product id=${id}`)),
      catchError(this.handleError<any>('updateProduct'))
    ).toPromise();
  }

  /** DELETE **/
  deleteProduct(id: bigint) {
    var deleteHttpOptions = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Id', id.toString())
    };
    return this.http.delete<Product>(this.urlPrefix + '/Remove', deleteHttpOptions).pipe(
      tap(_ => this.log(`deleted product id=${id}`)),
      catchError(this.handleError<Product>('deleteProduct'))
    ).toPromise();
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   *
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      console.error(error);

      this.log(`${operation} failed: ${error.message}`);

      return of(result as T);
    };
  }

  private log(message: string) {
    console.log(`ProductService: ${message}`);
  }
}
