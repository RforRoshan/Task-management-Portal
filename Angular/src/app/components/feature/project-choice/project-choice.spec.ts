import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectChoice } from './project-choice';

describe('ProjectChoice', () => {
  let component: ProjectChoice;
  let fixture: ComponentFixture<ProjectChoice>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectChoice]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectChoice);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
