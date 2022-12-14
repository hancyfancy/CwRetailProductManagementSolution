import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ProductAudit } from '../models/product-audit';
import { Settings } from '../settings';

@Injectable({
  providedIn: 'root'
})
export class ProductAuditService {
  private urlPrefix: string = '';
  public productAudits: ProductAudit[] = [];

  constructor(private http: HttpClient, private settings: Settings) {
    this.urlPrefix = this.settings.domain + '/api/ProductAudit';
  }

  /** GET **/
  getProductAuditUpdates(productId: bigint) {
    var httpOptions = {
      headers: new HttpHeaders()
        .set('Content-Type', 'application/json')
        .set('ProductId', productId.toString())
        .set('Authorization', 'Bearer ' + this.settings.jwtToken)
    };
    return this.http.get<ProductAudit[]>(this.urlPrefix + '/GetUpdates', httpOptions)
      .pipe(
        tap(_ => this.log('fetched product audits')),
        catchError(this.handleError<ProductAudit[]>('getProductAuditUpdates', []))
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

      confirm("No changes made, please check input values");

      return of(result as T);
    };
  }

  private log(message: string) {
    console.log(`ProductAuditService: ${message}`);
  }
}
