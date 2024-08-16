import { Component, OnInit } from '@angular/core';
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
import { EventService } from '../../services/event/event.service';
import moment from 'moment';
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
export class EventComponent implements OnInit {
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
    private alertService: AlertService,
    private eventService: EventService
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      startDate: [''],
      endDate: ['']
    });
  }
  ngOnInit(): void {
    this.loadEvent();
  }
  loadEvent(): void {
    this.eventService.getAllEvent(this.searchForm).subscribe({
      next: (data) => {
        console.log(data);
        this.currentPage = data.pageNumber;
        this.totalPages = data.totalPages;
        this.pagesArray = Array.from(
          { length: this.totalPages },
          (_, i) => i + 1
        );
        this.events = data.items;
      },
      error: (error) => {
        console.error('Error fetching users');
      },
      complete: () => {
        console.log('User request complete');
      },
    })
  }
  convertToDate(date: any, pattern: string): string {
    return moment(date).format(pattern);
  }
  getChanel(channels: any): string {
    return channels.map((c: any) => c.name).join(",");
  }
  getPartner(partners: any): string {
    return partners.map((p: any) => p.id).join(",") || 'N/A';
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
    this.loadEvent();
  }
  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadEvent();
    }
  }
}
