import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RNFeature } from './rnfeature';

describe('RNFeature', () => {
  let component: RNFeature;
  let fixture: ComponentFixture<RNFeature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RNFeature]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RNFeature);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
