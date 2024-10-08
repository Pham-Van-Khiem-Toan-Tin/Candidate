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
  NgSelectModule,
} from '@ng-select/ng-select';
import { ModalConfirmComponent } from '../../components/modal-confirm/modal-confirm.component';
import Modal from '../../models/Modal';
import { AlertService } from '../../services/alert/alert.service';
import { catchError, throwError } from 'rxjs';

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
    NgSelectModule,
    ModalConfirmComponent,
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
  modal: Modal = new Modal();
  totalPages: number = 0;
  currentPage: number = 1;
  pageSize: number = 10; // Số lượng người dùng trên mỗi trang
  pagesArray: number[] = [];
  constructor(
    private userService: UserService,
    private fb: FormBuilder,
    private router: Router,
    private alertService: AlertService
  ) {
    this.searchForm = this.fb.group({
      name: [''],
      status: null,
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
  exportUsers(): void {
    this.userService.exportUsers().pipe(
      catchError(error => {
        console.error('Error exporting users', error);
        return throwError(() => error);
      })
    )
    .subscribe(response => {
      if (response instanceof Blob) {
        const url = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'Users.xlsx';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove(); // Xóa thẻ a khỏi DOM
      } else {
        console.error('Unexpected response type');
      }
    });

  }
  onSearch(): void {
    console.log(this.searchForm.value);
    this.currentPage = 1;
    this.loadUsers();
  }
  getControl(name: string) {
    return this.searchForm.get(name) as FormControl;
  }
  openModalDelete(user: any): void {
    console.log(user);

    this.modal = {
      title: 'Are you want to delete?',
      content: `Are you sure you want to delete ${user.email} ?`,
      description: "All user's data will be lost.",
      data: user,
    };
  }
  viewUser(id: string): void {
    this.router.navigate([`users/view/${id}`]);
  }
  editUser(id: string): void {
    this.router.navigate([`users/edit/${id}`]);
  }
  deleteUser(): void {
    if (this.modal?.data?.id != null) {
      this.userService.deleteUser(this.modal.data.id).subscribe({
        next: (data) => {
          this.alertService.showAlert(data.message, 'success');
          setTimeout(() => {
            this.alertService.clearAlert();
            this.loadUsers();
          }, 1000);
        },
        error: (err) => {
          this.alertService.showAlert(err, 'error');
        },
        complete: () => {
          // console.log('Login request complete');
        },
      });
    }
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
    this.searchForm.patchValue({
      name: '',
      status: null,
    });
    this.currentPage = 1;
    this.loadUsers();
  }
}
