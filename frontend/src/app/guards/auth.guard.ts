import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (!authService.isUserLoggedIn && localStorage.getItem("token") == null) {
    return true;
  } else {
    router.navigate(['/users']);
    return false;
  }
};
