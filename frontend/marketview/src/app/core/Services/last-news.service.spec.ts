import { TestBed } from '@angular/core/testing';

import { LastNewsService } from './last-news.service';

describe('LastNewsService', () => {
  let service: LastNewsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LastNewsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
