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
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, BoxInputComponent, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm: FormGroup;
  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }
  getControl(name: string) {
    return this.loginForm.get(name) as FormControl;
  }
  login(): void {
    console.log(this.loginForm.valid);

    if (this.loginForm.valid) {
      this.authService.login(this.loginForm).subscribe({
        next: (data) => {
          console.log(this.loginForm.value);
          this.authService.isUserLoggedIn = true;
          localStorage.setItem('token', data.token);
          this.router.navigate(['/users']);
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
