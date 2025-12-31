import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubTaskHistory } from './sub-task-history';

describe('SubTaskHistory', () => {
  let component: SubTaskHistory;
  let fixture: ComponentFixture<SubTaskHistory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubTaskHistory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubTaskHistory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
