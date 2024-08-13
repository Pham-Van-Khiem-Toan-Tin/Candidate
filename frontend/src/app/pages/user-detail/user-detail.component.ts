import { Component } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import { BoxInfoComponent } from '../../components/box-info/box-info.component';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [LayoutComponent, BoxInfoComponent],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss'
})
export class UserDetailComponent {

}
