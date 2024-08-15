import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AlertService } from './services/alert/alert.service';
import { AlertComponent } from './components/alert/alert.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AlertComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'Candidate';
  alertMessage: string = "";
  alertType: 'success' | 'warning' | 'error' = 'success';

  constructor(private alertService: AlertService) {}

  ngOnInit() {
    this.alertService.alert$.subscribe(alert => {
      if (alert) {
        this.alertMessage = alert.message;
        this.alertType = alert.type;
      } else {
        this.alertMessage = '';
        this.alertType = 'success'; // Reset type or set default
      }
    });
  }

  
}
