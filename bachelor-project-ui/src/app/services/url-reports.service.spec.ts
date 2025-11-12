import { TestBed } from '@angular/core/testing';

import { UrlReportsService } from './url-reports.service';

describe('UrlReportsService', () => {
  let service: UrlReportsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UrlReportsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
