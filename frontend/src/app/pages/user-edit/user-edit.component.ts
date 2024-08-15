import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import User from '../../models/User';
import { ActivatedRoute, Router } from '@angular/router';
import  _  from 'lodash';
import moment from 'moment';
import { AlertService } from '../../services/alert/alert.service';
@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [LayoutComponent, ReactiveFormsModule],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss',
})
export class UserEditComponent implements OnInit {
  userForm: FormGroup;
  disabled: boolean = false;
  user: User = new User();
  roles: string[] = [];
  id: string = '';
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService
  ) {
    this.userForm = this.fb.group({
      fullName: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', [Validators.required, Validators.email]],
      DOB: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNo: ['', [Validators.required]],
      role: ['', [Validators.required]],
    });
  }
  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadUser();
    this.loadRoleList();
  }
  loadRoleList(): void {
    this.userService.getRoleList().subscribe({
      next: (data) => {
        this.roles = data;
      },
      error: (error) => {
        console.error('Error fetching role', error);
      },
      complete: () => {
        console.log('Role request complete');
      },
    })
  }
  loadUser(): void {
    this.userService.getUser(this.id).subscribe({
      next: (data) => {
        this.userForm.patchValue({
          fullName: data.fullName.trim(),
          email: data.email.trim(),
          DOB: moment(data.dateOfBirth).format("YYYY-MM-DD"),
          address: data.address.trim(),
          phoneNo: data.phoneNumber.trim(),
          role: data.roles,
        });
        this.userForm.get('fullName')?.updateValueAndValidity({emitEvent: false});
        this.userForm.get('email')?.updateValueAndValidity({emitEvent: false});
        this.userForm.get('DOB')?.updateValueAndValidity({emitEvent: false});
        this.userForm.get('address')?.updateValueAndValidity({emitEvent: false});
        this.userForm.get('phoneNo')?.updateValueAndValidity({emitEvent: false});
        this.userForm.get('role')?.updateValueAndValidity({emitEvent: false});
        console.log(this.userForm.valid);
        
        this.user = {
          fullName: data.fullName.trim(),
          address: data.address.trim(),
          email: data.email.trim(),
          dob: moment(data.dateOfBirth).format("YYYY-MM-DD"),
          phoneNo: data.phoneNumber.trim(),
          role: data.roles,
          status: data.status
        };
      },
      error: (error) => {
        console.error('Error fetching users', error);
      },
      complete: () => {
        console.log('User request complete');
      },
    });
  }
  editUser(): void {
    if (this.userForm.valid) {
      const formUser: User = {
        fullName: this.userForm.get('fullName')?.value.trim(),
        address: this.userForm.get('address')?.value.trim(),
        email: this.userForm.get('email')?.value.trim(),
        dob: moment(this.userForm.get('DOB')?.value).format("YYYY-MM-DD"),
        phoneNo: this.userForm.get('phoneNo')?.value.trim(),
        role: this.userForm.get('role')?.value,
        status: this.user.status // Nếu bạn muốn so sánh với `status` hiện tại
      };      
      if (!_.isEqual(this.user, formUser)) {
        this.userService.updateUser(formUser, this.id).subscribe({
          next: (data) => {
            this.disabled = true;
            this.alertService.showAlert(data.message, "success")
            setTimeout(() => {
              this.alertService.clearAlert();
              this.router.navigate(['/users'])
            }, 1000);
          },
          error: (err) => {
            this.alertService.showAlert(err, "error");
          },
          complete: () => {
            // console.log('Login request complete');
          },
        })
      } else {
        this.alertService.showAlert("Data duplicate", "warning");
      }
      
    } else {
      console.log(this.userForm.errors);
      
    }
  }
}
