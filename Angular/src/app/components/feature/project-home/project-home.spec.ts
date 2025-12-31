import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectHome } from './project-home';

describe('ProjectHome', () => {
  let component: ProjectHome;
  let fixture: ComponentFixture<ProjectHome>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectHome]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProjectHome);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
