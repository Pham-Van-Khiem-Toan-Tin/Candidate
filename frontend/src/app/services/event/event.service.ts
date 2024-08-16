import { HttpClient } from '@angular/common/http';
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
  createEvent(eventForm: FormGroup): Observable<any> {
    console.log(eventForm);
    
    return this.http.post<any>(`${EVENT_API_ENDPOINT.CREATE_EVENT}`, {
      Id: eventForm.get('eventId')?.value,
      Name: eventForm.get('eventName')?.value,
      StartDate: eventForm.get('startDate')?.value,
      EndDate: eventForm.get('endDate')?.value,
      Target: eventForm.get('target')?.value,
      ChannelIds: eventForm.get('chanel')?.value,
      PartnerIds: eventForm.get('partners')?.value,
      Participants: eventForm.get('totalParticipants')?.value ?? 0,
      Note: eventForm.get('note')?.value,
    });
  }
}
