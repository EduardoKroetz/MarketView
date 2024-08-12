import { TestBed } from '@angular/core/testing';

import { MostTradedService } from './most-traded.service';

describe('MostTradedService', () => {
  let service: MostTradedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MostTradedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
