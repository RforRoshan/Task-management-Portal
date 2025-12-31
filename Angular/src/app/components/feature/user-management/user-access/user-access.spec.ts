import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAccess } from './user-access';

describe('UserAccess', () => {
  let component: UserAccess;
  let fixture: ComponentFixture<UserAccess>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserAccess]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAccess);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
