import { Component } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { InputSearchComponent } from '../../components/input-search/input-search.component';
import { NgIconComponent } from '@ng-icons/core';
import { NgLabelTemplateDirective, NgOptionTemplateDirective, NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-candidate',
  standalone: true,
  imports: [
    LayoutComponent,
    ReactiveFormsModule,
    InputSearchComponent,
    NgIconComponent,
    NgSelectComponent,
    NgOptionTemplateDirective,
    NgLabelTemplateDirective,
  ],
  templateUrl: './candidate.component.html',
  styleUrl: './candidate.component.scss',
})
export class CandidateComponent {
  searchForm: FormGroup;
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10; // Số lượng người dùng trên mỗi trang
  pagesArray: number[] = [];
  constructor(private fb: FormBuilder) {
    this.searchForm = this.fb.group({
      name: '',
      eventName: '',
      status: '',
    });
  }
  getControl(name: string) {
    return this.searchForm.get(name) as FormControl;
  }
  onSearch(): void {
    console.log(this.searchForm.value);
    this.currentPage = 1;

  }
  clearForm(): void {
    this.searchForm.patchValue({
      name: '',
      status: '',
    });
    this.currentPage = 1;
  }
}
