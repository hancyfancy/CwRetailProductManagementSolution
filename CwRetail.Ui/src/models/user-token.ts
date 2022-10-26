export class UserToken {
  constructor(public userTokenId: bigint = 0n, public token: string = '', public refreshAt: Date = new Date()) { }
}
