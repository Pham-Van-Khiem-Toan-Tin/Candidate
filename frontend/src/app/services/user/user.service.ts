import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { USER_API_ENDPOINT } from '../../apis/user.api';
import axios from 'axios';
import User from '../../models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  http = inject(HttpClient);
  getAllUser(searchForm: FormGroup): Observable<any> {
    return this.http.get<any>(
      `${USER_API_ENDPOINT.ALL_USER}?name=${
        searchForm.get('name')?.value
      }&status=${searchForm.get('status')?.value}`
    );
  }
  getUser(id: string): Observable<any> {
    return this.http.get<any>(`${USER_API_ENDPOINT.USER_DETAIL + id}`);
  }
  exportUsers(): Observable<any> {
    let headers = new HttpHeaders();
    headers = headers.set('Accept', 'application/octet-stream');

    return this.http.get(`${USER_API_ENDPOINT.EXPORT_USER}`, { headers: headers, responseType: 'blob' });
  }
  getRoleList(): Observable<any> {
    return this.http.get(`${USER_API_ENDPOINT.ALL_ROLE}`);
  }
  createUser(userFrom: FormGroup): Observable<any> {
    return this.http.post<any>(USER_API_ENDPOINT.CREATE_USER, {
      Username: userFrom.get('userName')?.value.trim(),
      FullName: userFrom.get('fullName')?.value.trim(),
      Email: userFrom.get('email')?.value.trim(),
      PhoneNumber: userFrom.get('phoneNo')?.value.trim(),
      Address: userFrom.get('address')?.value.trim(),
      DateOfBirth: userFrom.get('DOB')?.value,
    });
  }
  updateUser(user: User, id: string): Observable<any> {
    return this.http.put<any>(`${USER_API_ENDPOINT.UPDATE_USER + id}`, {
      FullName: user.fullName,
      Email: user.email,
      PhoneNumber: user.phoneNo,
      Address: user.address,
      DateOfBirth: user.dob,
      Role: user.role,
    });
  }
  deleteUser(id: string): Observable<any> {
    return this.http.delete<any>(`${USER_API_ENDPOINT.DELETE_USER + id}`);
  }
  async changeActivated(id: string, status: boolean): Promise<boolean> {
    const token = localStorage.getItem('token') ?? '';

    let config = {
      headers: {
        Authorization: token ? `Bearer ${token}` : '',
      },
    };

    try {
      await axios.patch(
        `${USER_API_ENDPOINT.CHANGE_ACTIVATED}/${id}`,
        { IsActivated: status },
        config
      );
      return true;
    } catch (error) {
      return false;
      // Handle the error as needed
    }
  }
}
