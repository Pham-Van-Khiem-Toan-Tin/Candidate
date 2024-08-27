import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { POSITION_API_ENDPOINT } from '../../apis/postion.api';

@Injectable({
  providedIn: 'root'
})
export class PositionService {
  http = inject(HttpClient);
  constructor() { }
  getAllPostion(): Observable<any> {
    return this.http.get<any>(`${POSITION_API_ENDPOINT.ALL_POSITION}`)
  }
}
