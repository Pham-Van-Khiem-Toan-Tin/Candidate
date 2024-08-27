import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { NgIconComponent } from '@ng-icons/core';
import { remixSearchLine } from '@ng-icons/remixicon';

@Component({
  selector: 'app-input-search',
  standalone: true,
  imports: [ReactiveFormsModule, NgIconComponent],
  templateUrl: './input-search.component.html',
  styleUrl: './input-search.component.scss'
})
export class InputSearchComponent {
  icon = remixSearchLine;
  @Input() control!: FormControl;
  @Input() placeholder: string = '';
}
