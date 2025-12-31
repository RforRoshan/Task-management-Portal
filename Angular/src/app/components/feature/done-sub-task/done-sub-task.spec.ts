import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoneSubTask } from './done-sub-task';

describe('DoneSubTask', () => {
  let component: DoneSubTask;
  let fixture: ComponentFixture<DoneSubTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoneSubTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoneSubTask);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
