import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Product } from '../models/product';

@Injectable({ providedIn: 'root' })
export class ProductService {

  private domain: string = 'http://localhost:5138';
  private urlPrefix: string = this.domain + '/api/Product';
  public secretKey: string = 'a2203d87ae1c7b2f07c6075347a8351afcb7401de5912cee90989b9f5c26957c';

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
  addProduct(product: Product) {
    return this.http.post<Product>(this.urlPrefix + '/Create', JSON.stringify(product, (_, v) => typeof v === 'bigint' ? v.toString() : v), this.httpOptions).pipe(
      tap((newProduct: Product) => this.log(`added product w/ id=${newProduct.id}`)),
      catchError(this.handleError<Product>('addProduct'))
    ).toPromise();
  }

  /** PUT **/
  updateProduct(id: bigint, product: any) {
    var updateHttpOptions = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('Id', id.toString())
    };
    return this.http.put(this.urlPrefix + '/Edit', JSON.stringify(product, (_, v) => typeof v === 'bigint' ? v.toString() : v), updateHttpOptions).pipe(
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
