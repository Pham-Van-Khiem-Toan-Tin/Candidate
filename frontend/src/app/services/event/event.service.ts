import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EVENT_API_ENDPOINT } from '../../apis/event.api';
import { FormGroup } from '@angular/forms';
import moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  http = inject(HttpClient);
  constructor() { }
  getAllEvent(searchForm: FormGroup): Observable<any> {
    let startDate = searchForm.get('startDate')?.value;
    let endDate = searchForm.get('endDate')?.value;
    return this.http.get<any>(`${EVENT_API_ENDPOINT.ALL_EVENT}?name=${searchForm.get('name')?.value}&startDate=${startDate ? moment(startDate).format("YYYY/MM/DD") : ''}&endDate=${endDate ? moment(endDate).format("YYYY/MM/DD") : ''}`);
  }
  getAllEventInProgress(): Observable<any> {
    return this.http.get<any>(`${EVENT_API_ENDPOINT.ALL_EVENT_INPROGESS}`);
  }
  getEventDetail(id: string): Observable<any> {
    return this.http.get<any>(`${EVENT_API_ENDPOINT.DETAIL_EVENT + id}`)
  }
  deleteEvent(id: string): Observable<any> {
    return this.http.delete<any>(`${EVENT_API_ENDPOINT.DELETE_EVENT + id}`);
  }
  createEvent(eventForm: FormGroup): Observable<any> {
    console.log(eventForm);
    
    return this.http.post<any>(`${EVENT_API_ENDPOINT.CREATE_EVENT}`, {
      Name: eventForm.get('eventName')?.value,
      StartDate: eventForm.get('startDate')?.value,
      EndDate: eventForm.get('endDate')?.value,
      Target: eventForm.get('target')?.value,
      ChannelIds: eventForm.get('chanel')?.value,
      PartnerIds: eventForm.get('partners')?.value,
      Positions: eventForm.get('positions')?.value,
      Participants: eventForm.get('totalParticipants')?.value ?? 0,
      Note: eventForm.get('note')?.value,
    });
  }
  exportUsers(): Observable<any> {
    let headers = new HttpHeaders();
    headers = headers.set('Accept', 'application/octet-stream');
    return this.http.get(`${EVENT_API_ENDPOINT.EXPORT_EVENT}`, {headers: headers, responseType: 'blob'})
  }
}
