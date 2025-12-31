import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubTaskDetails } from './sub-task-details';

describe('SubTaskDetails', () => {
  let component: SubTaskDetails;
  let fixture: ComponentFixture<SubTaskDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubTaskDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubTaskDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
