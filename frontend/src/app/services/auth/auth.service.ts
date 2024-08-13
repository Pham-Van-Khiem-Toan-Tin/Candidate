import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable} from 'rxjs';
import { AUTH_API_ENDPOINT } from '../../apis/auth.api';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  isUserLoggedIn: boolean = false;
  http = inject(HttpClient);
  login(login: FormGroup): Observable<any> {
    return this.http.post<any>(AUTH_API_ENDPOINT.LOGIN, {
      Email: login.get('email')?.value,
      Password: login.get('password')?.value,
    });
  }
  forgot(forgotForm: FormGroup): Observable<any> {
    return this.http.post<any>(AUTH_API_ENDPOINT.FORGOT, {
      Email: forgotForm.get('email')?.value
    });
  }
  logout(): void {
    this.isUserLoggedIn = false;
    localStorage.removeItem('token');
  }
  reset(): Observable<any> {
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'application/json');
    return this.http.get<any>(AUTH_API_ENDPOINT.LOGIN, { headers: headers });
  }
}
