import { Product } from "./product";

export class ProductHistory {
  constructor(public product: Product = new Product(), public dateTime: string = '') { }
}
