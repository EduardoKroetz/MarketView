import { TestBed } from '@angular/core/testing';

import { AssetPageService } from './asset-page.service';

describe('AssetPageService', () => {
  let service: AssetPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AssetPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
