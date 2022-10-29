export class ProductAudit {
  constructor(public productAuditId: bigint = 0n, public json: string = '', public dateTime: Date = new Date()) { }
}
