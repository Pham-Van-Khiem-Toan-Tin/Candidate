import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
} from '@angular/core';
import { Subject } from 'rxjs';
import { AlertService } from '../../services/alert/alert.service';
import { NgIconComponent } from '@ng-icons/core';
import { remixCloseLine } from '@ng-icons/remixicon';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [NgIconComponent],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss',
})
export class AlertComponent implements OnChanges {
  iconClose = remixCloseLine;
  @Input() message: string = '';
  @Input() type: 'success' | 'warning' | 'error' = 'success';
  visible: boolean = false;

  constructor(private alertService: AlertService) {} // Inject AlertService

  ngOnChanges(changes: SimpleChanges) {
    if (changes['message']) {
      if (this.message != '') {
        this.visible = true;
      } else {
        this.visible = false;
      }
    }
  }

  close() {
    this.visible = false;
    this.alertService.clearAlert(); // Clear the alert
  }

  get typeClass() {
    return {
      'alert-success': this.type === 'success',
      'alert-warning': this.type === 'warning',
      'alert-error': this.type === 'error',
    };
  }
}
