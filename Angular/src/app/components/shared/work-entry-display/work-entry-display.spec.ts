import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkEntryDisplay } from './work-entry-display';

describe('WorkEntryDisplay', () => {
  let component: WorkEntryDisplay;
  let fixture: ComponentFixture<WorkEntryDisplay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WorkEntryDisplay]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkEntryDisplay);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
