import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoneTask } from './done-task';

describe('DoneTask', () => {
  let component: DoneTask;
  let fixture: ComponentFixture<DoneTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DoneTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoneTask);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
