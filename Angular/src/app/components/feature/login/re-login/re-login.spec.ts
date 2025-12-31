import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReLogin } from './re-login';

describe('ReLogin', () => {
  let component: ReLogin;
  let fixture: ComponentFixture<ReLogin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReLogin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReLogin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
