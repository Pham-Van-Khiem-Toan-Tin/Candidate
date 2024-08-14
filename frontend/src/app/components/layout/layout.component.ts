import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NgIconComponent } from '@ng-icons/core';
import {remixBox3Line, remixGroupLine, remixIdCardLine, remixMegaphoneLine, remixPieChartLine} from '@ng-icons/remixicon';
@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [NgIconComponent, RouterLinkActive, RouterLink],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  icons: { [key: string]: any } = {
    remixIdCardLine,
    remixBox3Line,
    remixMegaphoneLine,
    remixPieChartLine,
    remixGroupLine
  }
  @Input() fc: string = '';
  @Input() mt: string = '';
}
