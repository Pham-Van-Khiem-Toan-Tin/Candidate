import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BoxInfoComponent } from './box-info.component';

describe('BoxInfoComponent', () => {
  let component: BoxInfoComponent;
  let fixture: ComponentFixture<BoxInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BoxInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BoxInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
