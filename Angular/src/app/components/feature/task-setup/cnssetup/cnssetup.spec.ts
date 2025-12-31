import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CNSSetup } from './cnssetup';

describe('CNSSetup', () => {
  let component: CNSSetup;
  let fixture: ComponentFixture<CNSSetup>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CNSSetup]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CNSSetup);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
