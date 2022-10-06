import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductService {

  private domain: string = 'http://localhost:5138';
  private urlPrefix : string = this.domain + '/api/Product';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(
    private http: HttpClient) { }

  /** GET **/
  getProducts() {
    return this.http.get<Product[]>(this.urlPrefix + '/Get')
      .pipe(
        tap(_ => this.log('fetched products')),
        catchError(this.handleError<Product[]>('getProducts', []))
      ).toPromise();
  }


  /** POST **/
  addProduct(product: Product): Observable<Product> {
    return this.http.post<Product>(this.urlPrefix + '/Create', product, this.httpOptions).pipe(
      tap((newProduct: Product) => this.log(`added product w/ id=${newProduct.id}`)),
      catchError(this.handleError<Product>('addProduct'))
    );
  }

  /** PUT **/
  updateProduct(product: Product): Observable<any> {
    return this.http.put(this.urlPrefix + '/Edit', product, this.httpOptions).pipe(
      tap(_ => this.log(`updated product id=${product.id}`)),
      catchError(this.handleError<any>('updateProduct'))
    );
  }

  /** DELETE **/
  deleteProduct(id: bigint): Observable<Product> {
    var deleteHttpOptions = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Id', id.toString())
    };
    return this.http.delete<Product>(this.urlPrefix + '/Remove', deleteHttpOptions).pipe(
      tap(_ => this.log(`deleted product id=${id}`)),
      catchError(this.handleError<Product>('deleteProduct'))
    );
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
