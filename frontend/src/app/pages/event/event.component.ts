import { Component } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  remixArrowDropLeftLine,
  remixArrowDropRightLine,
  remixDeleteBin5Line,
  remixInfoI,
  remixLockLine,
  remixLockUnlockLine,
  remixPencilLine,
} from '@ng-icons/remixicon';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { NgIconComponent } from '@ng-icons/core';
import { InputSearchComponent } from '../../components/input-search/input-search.component';
import { Router } from '@angular/router';
import { AlertService } from '../../services/alert/alert.service';
@Component({
  selector: 'app-event',
  standalone: true,
  imports: [
    LayoutComponent,
    NgIconComponent,
    ReactiveFormsModule,
    InputSearchComponent,
  ],
  templateUrl: './event.component.html',
  styleUrl: './event.component.scss',
})
export class EventComponent {
  icons: { [key: string]: any } = {
    remixInfoI,
    remixPencilLine,
    remixDeleteBin5Line,
    remixLockLine,
    remixLockUnlockLine,
    remixArrowDropLeftLine,
    remixArrowDropRightLine,
  };
  searchForm: FormGroup;
  events: any = [];
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10;
  pagesArray: number[] = [];
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private alertService: AlertService
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      startDate: [''],
      endDate: ['']
    });
  }
  getControl(name: string) {
    return this.searchForm.get(name) as FormControl;
  }
  clearForm(): void {
    this.searchForm.patchValue({
      name: '',
      startDate: '',
      endDate: ''
    });
    this.currentPage = 1;
    // this.loadUsers();
  }
}
