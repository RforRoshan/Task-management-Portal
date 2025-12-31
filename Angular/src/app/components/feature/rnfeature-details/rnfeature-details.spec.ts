import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RNFeatureDetails } from './rnfeature-details';

describe('RNFeatureDetails', () => {
  let component: RNFeatureDetails;
  let fixture: ComponentFixture<RNFeatureDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RNFeatureDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RNFeatureDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
