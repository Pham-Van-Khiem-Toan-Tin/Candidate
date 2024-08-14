import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import { InputSearchComponent } from '../../components/input-search/input-search.component';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import moment from 'moment';
import { Router } from '@angular/router';
import { NgIconComponent } from '@ng-icons/core';
import {
  remixArrowDropLeftLine,
  remixArrowDropRightLine,
  remixDeleteBin5Line,
  remixInfoI,
  remixLockLine,
  remixLockUnlockLine,
  remixPencilLine,
} from '@ng-icons/remixicon';
import {
  NgLabelTemplateDirective,
  NgOptionTemplateDirective,
  NgSelectComponent,
} from '@ng-select/ng-select';
@Component({
  selector: 'app-user',
  standalone: true,
  imports: [
    LayoutComponent,
    InputSearchComponent,
    ReactiveFormsModule,
    NgIconComponent,
    NgSelectComponent,
    NgOptionTemplateDirective,
    NgLabelTemplateDirective,
  ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
})
export class UserComponent implements OnInit {
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
  users: any = [];
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10; // Số lượng người dùng trên mỗi trang
  pagesArray: number[] = [];
  constructor(
    private userService: UserService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      status: [''],
    });
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(): void {
    this.userService.getAllUser(this.searchForm).subscribe({
      next: (data) => {
        console.log(data);
        this.totalPages = data.totalPages;
        this.pagesArray = Array.from(
          { length: this.totalPages },
          (_, i) => i + 1
        );
        this.users = data.items;
      },
      error: (error) => {
        console.error('Error fetching users', error);
      },
      complete: () => {
        console.log('User request complete');
      },
    });
  }
  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
    }
  }
  convertToDDMMYYYY(str: Date): string {
    return moment(str).format('DD/MM/YYYY');
  }
  onSearch(): void {
    console.log(this.searchForm.value);
    this.currentPage = 1;
    this.loadUsers();
  }
  getControl(name: string) {
    return this.searchForm.get(name) as FormControl;
  }
  viewUser(id: string): void {
    this.router.navigate([`user/view/${id}`]);
  }
  editUser(id: string): void {
    this.router.navigate([`user/edit/${id}`]);
  }
  async toggleStatus(id: string, isActive: boolean): Promise<void> {
    console.log(isActive);
    let result = await this.userService.changeActivated(id, isActive);
    if (result) {
      this.loadUsers();
    } else {
      console.error('Error changing activation status:');
    }
  }
  clearForm(): void {
    this.searchForm.reset();
    this.currentPage = 1;
    this.loadUsers();
  }
}
