import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourceSetup } from './resource-setup';

describe('ResourceSetup', () => {
  let component: ResourceSetup;
  let fixture: ComponentFixture<ResourceSetup>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ResourceSetup]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResourceSetup);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
