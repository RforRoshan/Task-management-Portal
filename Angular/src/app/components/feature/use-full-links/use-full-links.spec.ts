import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UseFullLinks } from './use-full-links';

describe('UseFullLinks', () => {
  let component: UseFullLinks;
  let fixture: ComponentFixture<UseFullLinks>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UseFullLinks]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UseFullLinks);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
