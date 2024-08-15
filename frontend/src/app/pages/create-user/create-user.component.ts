import { Component } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UserService } from '../../services/user/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-user',
  standalone: true,
  imports: [LayoutComponent, ReactiveFormsModule],
  templateUrl: './create-user.component.html',
  styleUrl: './create-user.component.scss',
})
export class CreateUserComponent {
  userForm: FormGroup;
  disabled: boolean = false;
  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      userName: ['', [Validators.required, Validators.pattern('^[a-z]+-cdd$')]],
      fullName: ['', [Validators.required, Validators.minLength(8)]],
      email: ['', [Validators.required, Validators.email]],
      DOB: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNo: ['', [Validators.required]],
    });
  }
  createUser(): void {
    if (this.userForm.valid) {
      this.userService.createUser(this.userForm).subscribe({
        next: (data) => {
          this.disabled = true;
          setTimeout(() => this.router.navigate(['/users']), 1000);
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          console.log('Login request complete');
        },
      });
    }
  }
}
