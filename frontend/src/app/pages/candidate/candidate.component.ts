import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  remixArrowDropLeftLine,
  remixArrowDropRightLine,
  remixDeleteBin5Line,
  remixInfoI,
  remixLockLine,
  remixLockUnlockLine,
  remixPencilLine,
} from '@ng-icons/remixicon';
import { InputSearchComponent } from '../../components/input-search/input-search.component';
import { NgIconComponent } from '@ng-icons/core';
import { NgLabelTemplateDirective, NgOptionTemplateDirective, NgSelectComponent, NgSelectModule } from '@ng-select/ng-select';
import { CandidateService } from '../../services/candidate/candidate.service';
import moment from 'moment';

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
    NgSelectModule
  ],
  templateUrl: './candidate.component.html',
  styleUrl: './candidate.component.scss',
})
export class CandidateComponent implements OnInit {
  searchForm: FormGroup;
  candidates: any = [];
  totalPages: number = 0;
  currentPage: number = 1;
  showAdvancedFilter: boolean = false;
  cars = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
];
  icons: { [key: string]: any } = {
    remixInfoI,
    remixPencilLine,
    remixDeleteBin5Line,
    remixLockLine,
    remixLockUnlockLine,
    remixArrowDropLeftLine,
    remixArrowDropRightLine,
  };
  pageSize: number = 10; // Số lượng người dùng trên mỗi trang
  pagesArray: number[] = [];
  constructor(private fb: FormBuilder, private candidateService: CandidateService) {
    this.searchForm = this.fb.group({
      name: '',
      eventName: '',
      status: null,
    });
  }
  ngOnInit(): void {
    this.loadCandidate();  
  }
  loadCandidate(): void {
    this.candidateService.getAllCandidate().subscribe({
      next: (data) => {
        this.candidates = data;
        console.log(data);
        
      },
      error: (error) => {
        console.error('Error fetching candidate');
      },
      complete: () => {
        console.log('Candidate request complete');
      },
    })
  }
  getControl(name: string) {
    return this.searchForm.get(name) as FormControl;
  }
  onSearch(): void {
    this.currentPage = 1;
  }
  getApplyJob(jobs: any): string {
    return jobs.map((item: any) => item.name).join(", ");
  }
  convertToDate(date: any, pattern: string): string {
    return moment(date).format(pattern);
  }
  toggleAdvancedFilter(): void {
    this.showAdvancedFilter = !this.showAdvancedFilter;
  }
  clearForm(): void {
    this.searchForm.patchValue({
      name: '',
      status: '',
    });
    this.currentPage = 1;
  }
}
