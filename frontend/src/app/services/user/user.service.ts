import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { USER_API_ENDPOINT } from '../../apis/user.api';
import axios from 'axios';

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
  async changeActivated(
    id: string,
    status: boolean,
  ): Promise<boolean> {
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
