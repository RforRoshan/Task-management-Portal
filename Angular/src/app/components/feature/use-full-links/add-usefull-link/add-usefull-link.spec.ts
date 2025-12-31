import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUsefullLink } from './add-usefull-link';

describe('AddUsefullLink', () => {
  let component: AddUsefullLink;
  let fixture: ComponentFixture<AddUsefullLink>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddUsefullLink]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddUsefullLink);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
