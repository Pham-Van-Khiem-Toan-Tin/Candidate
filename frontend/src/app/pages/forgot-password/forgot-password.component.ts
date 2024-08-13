import { Component } from '@angular/core';
import { BoxInputComponent } from '../../components/box-input/box-input.component';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [BoxInputComponent, ReactiveFormsModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss'
})
export class ForgotPasswordComponent {
  forgotForm: FormGroup;
  constructor(
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }
  getControl(name: string) {
    return this.forgotForm.get(name) as FormControl;
  }
  forgot(): void {
    console.log(this.forgotForm.valid);

    if (this.forgotForm.valid) {
      this.authService.forgot(this.forgotForm).subscribe({
        next: (data) => {
          console.log(this.forgotForm.value);
          this.authService.isUserLoggedIn = true;
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
