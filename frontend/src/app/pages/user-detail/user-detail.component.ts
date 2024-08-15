import { Component, OnInit } from '@angular/core';
import { LayoutComponent } from '../../components/layout/layout.component';
import { UserService } from '../../services/user/user.service';
import { ActivatedRoute } from '@angular/router';
import User from '../../models/User';
import moment from 'moment';
@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [LayoutComponent],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss',
})
export class UserDetailComponent implements OnInit {
  user: User = new User();
  id: string = '';
  constructor(
    private userService: UserService,
    private route: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadUser();
  }
  loadUser(): void {
    this.userService.getUser(this.id).subscribe({
      next: (data) => {
        console.log(data);
        this.user.fullName = data.fullName;
        this.user.address = data.address;
        this.user.email = data.email;
        this.user.dob = moment(data.dateOfBirth).format("DD-MM-YYYY");
        this.user.phoneNo = data.phoneNumber;
        this.user.role = data.roles;
        this.user.status = data.status;
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
