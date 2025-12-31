import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNewCurrentTask } from './add-new-current-task';

describe('AddNewCurrentTask', () => {
  let component: AddNewCurrentTask;
  let fixture: ComponentFixture<AddNewCurrentTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddNewCurrentTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNewCurrentTask);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
