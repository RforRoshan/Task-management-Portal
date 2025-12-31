import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoValidURL } from './no-valid-url';

describe('NoValidURL', () => {
  let component: NoValidURL;
  let fixture: ComponentFixture<NoValidURL>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoValidURL]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NoValidURL);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
