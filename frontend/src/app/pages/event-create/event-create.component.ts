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
import { PartnerService } from '../../services/partner/partner.service';
import { EventService } from '../../services/event/event.service';
import { PositionService } from '../../services/position/position.service';

@Component({
  selector: 'app-event-create',
  standalone: true,
  imports: [
    LayoutComponent,
    ReactiveFormsModule,
    NgSelectComponent,
    NgOptionTemplateDirective,
    NgLabelTemplateDirective,
    NgSelectModule,
  ],
  templateUrl: './event-create.component.html',
  styleUrl: './event-create.component.scss',
})
export class EventCreateComponent implements OnInit {
  eventForm: FormGroup;
  disabled: boolean = true;
  channels: any = [];
  partners: any = [];
  positions: any = [];
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private chanelService: ChanelService,
    private partnerService: PartnerService,
    private eventService: EventService,
    private positionService: PositionService
  ) {
    this.eventForm = this.fb.group({
      eventName: ['', [Validators.required]],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      target: [''],
      chanel: [[], [Validators.required]],
      partners: [[]],
      positions: [[], [Validators.required]],
      totalParticipants: ['', [Validators.min(0)]],
      note: [''],
    });
  }
  ngOnInit(): void {
    this.disabled = true;
    Promise.all([this.getAllChanel(), this.getAllPartner(), this.getAllPosition()])
      .then(([chanelData, partnerData, positionData]) => {
        this.disabled = false;
      })
      .catch((error) => {
        console.error('Error fetching data:', error);
      });
  }
  getAllPosition(): void {
    this.positionService.getAllPostion().subscribe({
      next: (data) => {
        this.positions = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    })
  }
  getAllPartner(): void {
    this.partnerService.getAllPartner().subscribe({
      next: (data) => {
        this.partners = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    });
  }
  getAllChanel(): void {
    this.chanelService.getAllChanel().subscribe({
      next: (data) => {
        this.channels = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    });
  }
  createEvent(): void {
    console.log(this.eventForm.valid);
    console.log('Event ID Errors:', this.eventForm.get('eventId')?.errors);
    console.log('Chanel Errors:', this.eventForm.get('chanel')?.errors);
    console.log('Partners Errors:', this.eventForm.get('partners')?.errors);
    if (this.eventForm.valid) {
      this.eventService.createEvent(this.eventForm).subscribe({
        next: (data) => {
          this.disabled = true;
          setTimeout(() => this.router.navigate(['/events']), 1000);
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          console.log('Login request complete');
        },
      });
      
    }
  }
}
