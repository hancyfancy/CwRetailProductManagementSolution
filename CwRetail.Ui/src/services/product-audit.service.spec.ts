import { TestBed } from '@angular/core/testing';

import { ProductAuditService } from './product-audit.service';

describe('ProductAuditService', () => {
  let service: ProductAuditService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductAuditService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
