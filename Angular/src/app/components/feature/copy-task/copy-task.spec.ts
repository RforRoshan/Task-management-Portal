import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyTask } from './copy-task';

describe('CopyTask', () => {
  let component: CopyTask;
  let fixture: ComponentFixture<CopyTask>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CopyTask]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CopyTask);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
