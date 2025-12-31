import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertDisplay } from './alert-display';

describe('AlertDisplay', () => {
  let component: AlertDisplay;
  let fixture: ComponentFixture<AlertDisplay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlertDisplay]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlertDisplay);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
