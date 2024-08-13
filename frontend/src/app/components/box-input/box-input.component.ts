import { Component , Input } from '@angular/core';
import {  FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-box-input',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './box-input.component.html',
  styleUrl: './box-input.component.scss',
})
export class BoxInputComponent {
  @Input() id = '';
  @Input() label: string = '';
  @Input() type: string = 'text';
  @Input() innerValue: any = '';
  @Input() placeholder: string = '';
  @Input() required: boolean = false;
  @Input() name: string = '';
  @Input() control!: FormControl;
  
}
