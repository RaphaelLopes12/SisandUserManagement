import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequiredLabelComponent } from './required-label.component';

describe('RequiredLabelComponent', () => {
  let component: RequiredLabelComponent;
  let fixture: ComponentFixture<RequiredLabelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RequiredLabelComponent]
    });
    fixture = TestBed.createComponent(RequiredLabelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
