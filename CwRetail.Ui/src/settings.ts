import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class Settings {
  public readonly secretKey: string = 'a2203d87ae1c7b2f07c6075347a8351afcb7401de5912cee90989b9f5c26957c';
  public readonly domain: string = 'http://localhost:5138';
}
