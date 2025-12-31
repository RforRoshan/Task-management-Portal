import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskSetupHome } from './task-setup-home';

describe('TaskSetupHome', () => {
  let component: TaskSetupHome;
  let fixture: ComponentFixture<TaskSetupHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TaskSetupHome]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TaskSetupHome);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
