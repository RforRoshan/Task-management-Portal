import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubTask } from './sub-task';

describe('SubTask', () => {
  let component: SubTask;
  let fixture: ComponentFixture<SubTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubTask);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
