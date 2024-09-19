import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CANDIDATE_API_ENDPOINT } from '../../apis/candidate.api';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  http = inject(HttpClient)
  constructor() { }
  getAllCandidate(): Observable<any> {
    return this.http.get<any>(`${CANDIDATE_API_ENDPOINT.ALL_CANDIDATE}`);
  }
  createCandidate(candidate: FormGroup, candidateFormData: FormData): Observable<any> {
    let dataFormData = new FormData();
    const appendValue = (key: string, value: any) => {
      if (value !== null && value !== undefined) {
        if (Array.isArray(value) || typeof value === 'object') {
          dataFormData.append(key, JSON.stringify(value));
        } else {
          dataFormData.append(key, value);
        }
      }
    };
    appendValue("Id", candidate.get("idCard")?.value);
    appendValue("FullName", candidate.get("fullName")?.value);
    appendValue("Email", candidate.get("email")?.value);
    appendValue("DateOfBirth", candidate.get("DOB")?.value);
    appendValue("PhoneNumber", candidate.get("phoneNo")?.value);
    appendValue("Address", candidate.get("address")?.value);
    appendValue("Gender", candidate.get("gender")?.value);
    appendValue("UniversityId", candidate.get("university")?.value);
    appendValue("Skills", candidate.get("skill")?.value);
    appendValue("Major", candidate.get("major")?.value);
    appendValue("Language", candidate.get("language")?.value);
    appendValue("Graduation", candidate.get("graduationYear")?.value);
    appendValue("GPA", candidate.get("GPA")?.value);
    appendValue("ApplyDate", candidate.get("applyDate")?.value);
    appendValue("WorkingTime", candidate.get("workingTime")?.value);
    appendValue("Status", candidate.get("status")?.value);
    appendValue("Note", candidate.get("note")?.value);
    appendValue("Positions", candidate.get("jobs")?.value);
    appendValue("EventId", candidate.get("event")?.value);
    appendValue("channelId", candidate.get("channel")?.value);
    if (candidateFormData.has("file")) {
      dataFormData.append("file", candidateFormData.get("file") as File);
    }
    return this.http.post<any>(`${CANDIDATE_API_ENDPOINT.CREATE_CANDIDATE}`, dataFormData);
  }
  
}
