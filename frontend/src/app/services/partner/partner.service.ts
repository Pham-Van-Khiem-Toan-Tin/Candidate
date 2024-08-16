import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PARTNER_API_ENDPOIMT } from '../../apis/partner.api';

@Injectable({
  providedIn: 'root'
})
export class PartnerService {
  http = inject(HttpClient);
  constructor() { }
  getAllPartner(): Observable<any> {
    return this.http.get<any>(`${PARTNER_API_ENDPOIMT.ALL_PARTNER}`);
  }
}
