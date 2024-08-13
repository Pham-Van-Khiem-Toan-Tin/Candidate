import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';

export const expenseGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (!authService.isUserLoggedIn && localStorage.getItem("token") == null) {
    router.navigate(['/login']);
    return false;
  } else {
    return true;
  }
};
