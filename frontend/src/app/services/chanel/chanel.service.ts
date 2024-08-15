import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CHANEL_API_ENDPOINT } from '../../apis/chanel.api';

@Injectable({
  providedIn: 'root'
})
export class ChanelService {
  http = inject(HttpClient);
  constructor() { }
  getAllChanel(): Observable<any> {
    return this.http.get<any>(`${CHANEL_API_ENDPOINT.ALL_CHANEL}`);
  }
}
