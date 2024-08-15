import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private alertSubject = new BehaviorSubject<{ message: string, type: 'success' | 'warning' | 'error' } | null>(null);
  alert$ = this.alertSubject.asObservable();

  showAlert(message: string, type: 'success' | 'warning' | 'error') {
    this.alertSubject.next({ message, type });
  }

  clearAlert() {
    this.alertSubject.next({message: "",type: "success"});
  }
}
