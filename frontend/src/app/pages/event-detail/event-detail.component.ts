import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import Event from '../../models/Event';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../services/event/event.service';
import moment from 'moment';

@Component({
  selector: 'app-event-detail',
  standalone: true,
  imports: [LayoutComponent],
  templateUrl: './event-detail.component.html',
  styleUrl: './event-detail.component.scss'
})
export class EventDetailComponent implements OnInit {
  eventDetail: Event = new Event();
  id: string = '';
  constructor(private eventService: EventService , private route: ActivatedRoute) {

  }
  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadEvent();
  }
  test() {

  }
  convertDateToStringFormat(date: any, format: string): string {
    return moment(date).format(format);
  }
  loadEvent(): void {
    this.eventService.getEventDetail(this.id).subscribe({
      next: (data) => {
        console.log(data);
        
        this.eventDetail = {
          name: data.name,
          chanel: data.channels.map((c: any) => c.name).join(", "),
          startDate: this.convertDateToStringFormat(data.startDate, "DD/MM/YYYY"),
          endDate: this.convertDateToStringFormat(data.endDate, "DD/MM/YYYY"),
          partners: data.channels.map((p: any) => p.id).join(", ") ?? "N/A",
          targets: data.target,
          participants: data.participants,
          note: data.note,
          status: data.status
        }
        
      },
      error: (error) => {
        console.error('Error fetching users', error);
      },
      complete: () => {
        console.log('User request complete');
      },
    });
  }
}
