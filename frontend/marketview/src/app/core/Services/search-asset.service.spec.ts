import { TestBed } from '@angular/core/testing';

import { SearchAssetService } from './search-asset.service';

describe('SearchAssetService', () => {
  let service: SearchAssetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SearchAssetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
