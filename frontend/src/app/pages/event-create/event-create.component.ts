import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  NgLabelTemplateDirective,
  NgOptionTemplateDirective,
  NgSelectComponent,
  NgSelectModule,
} from '@ng-select/ng-select';
import { ChanelService } from '../../services/chanel/chanel.service';

@Component({
  selector: 'app-event-create',
  standalone: true,
  imports: [
    LayoutComponent,
    ReactiveFormsModule,
    NgSelectComponent,
    NgOptionTemplateDirective,
    NgLabelTemplateDirective,
    NgSelectModule
  ],
  templateUrl: './event-create.component.html',
  styleUrl: './event-create.component.scss',
})
export class EventCreateComponent implements OnInit {
  eventForm: FormGroup;
  disabled: boolean = false;
  channels: any = []
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private chanelService: ChanelService
  ) {
    this.eventForm = this.fb.group({
      eventId: [
        '',
        [Validators.required, Validators.pattern('^[a-z]+-[a-z]+-d{4}$')],
      ],
      eventName: ['', [Validators.required]],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      target: ['', [Validators.required]],
      chanel: [[], [Validators.required]],
      partners: [[], [Validators.required]],
      totalParticipants: ['', Validators.required, Validators.min(0)],
      note: [''],
    });
  }
  ngOnInit(): void {
    this.getAllChanel();
  }
  getAllChanel(): void {
    this.chanelService.getAllChanel().subscribe({
      next: (data) => {
        console.log(data);
        this.channels = data;
        
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    })
  }
  createEvent(): void {
    let valueDiv = document.querySelector(".editable-div");
    console.log(valueDiv?.textContent);
    
    
  }
}
