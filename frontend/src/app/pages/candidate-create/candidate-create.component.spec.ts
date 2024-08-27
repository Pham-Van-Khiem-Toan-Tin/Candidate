import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateCreateComponent } from './candidate-create.component';

describe('CandidateCreateComponent', () => {
  let component: CandidateCreateComponent;
  let fixture: ComponentFixture<CandidateCreateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidateCreateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CandidateCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
