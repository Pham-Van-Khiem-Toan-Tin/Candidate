import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  NgLabelTemplateDirective,
  NgOptionTemplateDirective,
  NgSelectComponent,
  NgSelectModule,
} from '@ng-select/ng-select';
import { EventService } from '../../services/event/event.service';
import { PartnerService } from '../../services/partner/partner.service';
import { NgIconComponent } from '@ng-icons/core';
import { remixAttachmentLine } from '@ng-icons/remixicon';

@Component({
  selector: 'app-candidate-create',
  standalone: true,
  imports: [
    LayoutComponent,
    ReactiveFormsModule,
    NgSelectComponent,
    NgOptionTemplateDirective,
    NgLabelTemplateDirective,
    NgSelectModule,
    NgIconComponent
  ],
  templateUrl: './candidate-create.component.html',
  styleUrl: './candidate-create.component.scss',
})
export class CandidateCreateComponent implements OnInit {
  icons: {[key: string]: any} = {
    remixAttachmentLine
  }
  candidateForm: FormGroup;
  disabled: boolean = false;
  disableChannelAndPosition: boolean = true;
  genders: any = ['male', 'female'];
  events: any = [];
  fileName: string = 'Select a CV';
  universities: any = [];
  positions: any = [];
  channels: any = [];
  skills: any = [
    "C#", "ASP.NET", "C++", "OpenGL", "JavaScript", "ES6", 
    "Node.js", "Express.js", "Java", "Spring Boot", "Python",
    "Django", "Ruby on Rails", "TypeScript", "Angular", "React.js",
    "Vue.js", "PHP", "Laravel", "SQL", "MongoDB"
  ].sort((a, b) => a.localeCompare(b));
  years: any = [
    "1994", "1995", "1996", "1997", "1998", "1999", "2000", "2001",
    "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009",
    "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017",
    "2018", "2019", "2020", "2021", "2022", "2023", "2024"
  ];
  workingTimes: any = ["Full time", "Part time"];
  status: any = ["In-progress", "Recall", "Not quality", "Follow up", "Test"]
  popularLanguages: any = [
    "English", "Mandarin Chinese", "Spanish", "Hindi",
    "Arabic", "French", "Bengali", "Russian", "Portuguese",
    "Indonesian", "Urdu", "German", "Japanese", "Swahili", "Marathi",
    "Vietnamese"
  ].sort((a, b) => a.localeCompare(b));
  itMajors = [
    "Computer Science", "Software Engineering",
    "Information Technology", "Computer Engineering",
    "Data Science", "Network Engineering", "Business Administration",
    "User Interface Design", "Telecommunications Engineering",
    "Information Security Engineering", "Database Engineering", "Information Systems"
  ].sort((a, b) => a.localeCompare(b));
  constructor(private fb: FormBuilder,
    private eventService : EventService,
    private partnerService: PartnerService) {
    this.candidateForm = this.fb.group({
      idCard: ['', [Validators.required]],
      fullName: ['', [Validators.required]],
      email: ['', [Validators.required]],
      DOB: ['', [Validators.required]],
      address: ['', [Validators.required]],
      phoneNo: ['', [Validators.required]],
      gender: [null, [Validators.required]],
      university: [null, [Validators.required]],
      skill: [[], [Validators.required]],
      major: [null, [Validators.required]],
      language: [''],
      graduationYear: ['', [Validators.required]],
      linkCV: [''],
      GPA: ['', [Validators.required, Validators.pattern('^[0-9]*\\.?[0-9]+$')]],
      jobs: [[], [Validators.required]],
      event: [null, [Validators.required]],
      applyDate: ['', [Validators.required]],
      channel: [null, [Validators.required]],
      workingTime: [null, Validators.required],
      status: [null, Validators.required],
      note: [''],
    });
    this.candidateForm.get('event')?.valueChanges.subscribe(selectedEnvent => {
      this.onEventChange(selectedEnvent);
    })
  }
  ngOnInit(): void {
    this.disabled = true;
    Promise.all([this.getAllEventInProgress(), this.getAllUniversity()])
    .then(([eventsData, universitiesData]) => {
      this.disabled = false;
    }).catch((error) => {
      console.error('Error fetching data:', error);
    });
  }
  onInputNumber(event: any) {
    const value = event.target.value;
    event.target.value = value.replace(/[^0-9]/g, '');
  }
  onInputNumberFloat(event: any) {
    const value = event.target.value;
    event.target.value = value
      .replace(/[^0-9.]/g, '')        // Loại bỏ ký tự không phải số hoặc dấu chấm
      .replace(/(\..*)\./g, '$1');  
  }
  onFileChange(event: any): void {
    const fileInput = event.target;
    if (fileInput.files.length > 0) {
      this.fileName = fileInput.files[0].name;
    } else {
      this.fileName = 'Select a CV';
    }
  }
  onEventChange(eventId: string): void {
    console.log(eventId);
    console.log(this.events.find((item: any) => item.id == eventId));
    let eventSelect = this.events.find((item: any) => item.id == eventId);
    this.channels = eventSelect.channels;
    this.positions = eventSelect.positions;
    this.disableChannelAndPosition = false;
  }
  getAllUniversity(): void {
    this.partnerService.getAllPartner().subscribe({
      next: (data) => {
        this.universities = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    })
  }
  getAllEventInProgress(): void {
    this.eventService.getAllEventInProgress().subscribe({
      next: (data) => {
        this.events = data;
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
        console.log('Login request complete');
      },
    })
  }
}
