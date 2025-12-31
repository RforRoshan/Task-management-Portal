import { TestBed } from '@angular/core/testing';

import { DisplayInputService } from './display-input-service';

describe('DisplayInputService', () => {
  let service: DisplayInputService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DisplayInputService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
