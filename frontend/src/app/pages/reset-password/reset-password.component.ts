import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BoxInputComponent } from '../../components/box-input/box-input.component';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [RouterLink, BoxInputComponent, ReactiveFormsModule],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss'
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.resetForm = this.fb.group({
      confirmPass: ['', [Validators.required, Validators.minLength(6)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  getControl(name: string) {
    return this.resetForm.get(name) as FormControl;
  }
  reset(): void {
    console.log(this.resetForm.valid);

    if (this.resetForm.valid) {
      this.authService.reset().subscribe({
        next: (data) => {
          console.log(this.resetForm.value);
          // this.authService.isUserLoggedIn = true;
          // localStorage.setItem('isUserLoggedIn', data.token);
          // this.router.navigate(['/user']);
        },
        error: (err) => {
          //
        },
        complete: () => {
          console.log('Login request complete');
        },
      });
    }
  }
}
