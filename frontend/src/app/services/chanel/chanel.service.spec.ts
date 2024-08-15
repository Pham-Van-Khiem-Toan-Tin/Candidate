import { TestBed } from '@angular/core/testing';

import { ChanelService } from './chanel.service';

describe('ChanelService', () => {
  let service: ChanelService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChanelService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
