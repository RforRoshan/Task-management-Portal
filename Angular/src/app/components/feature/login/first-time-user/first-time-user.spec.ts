import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FirstTimeUser } from './first-time-user';

describe('FirstTimeUser', () => {
  let component: FirstTimeUser;
  let fixture: ComponentFixture<FirstTimeUser>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FirstTimeUser]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FirstTimeUser);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
