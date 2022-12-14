import { ProductType } from "../enumerations/product-type";

export class Product {
  constructor(public productId: bigint = 0n, public name: string = '', public price: number = 0, public type: ProductType = ProductType.Books, public active: boolean = false, public lastUpdated: string = '') { }
}
