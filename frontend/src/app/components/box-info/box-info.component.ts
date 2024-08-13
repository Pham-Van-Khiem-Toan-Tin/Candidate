import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-box-info',
  standalone: true,
  imports: [],
  templateUrl: './box-info.component.html',
  styleUrl: './box-info.component.scss'
})
export class BoxInfoComponent {
  @Input() label: string = '';
  @Input() value: string = '';
}
