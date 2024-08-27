import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { UserComponent } from './pages/user/user.component';
import { expenseGuard } from './guards/expense.guard';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { UserDetailComponent } from './pages/user-detail/user-detail.component';
import { UserEditComponent } from './pages/user-edit/user-edit.component';
import { CreateUserComponent } from './pages/create-user/create-user.component';
import { EventComponent } from './pages/event/event.component';
import { EventCreateComponent } from './pages/event-create/event-create.component';
import { EventDetailComponent } from './pages/event-detail/event-detail.component';
import { CandidateComponent } from './pages/candidate/candidate.component';
import { authGuard } from './guards/auth.guard';
import { CandidateCreateComponent } from './pages/candidate-create/candidate-create.component';

export const routes: Routes = [
    {path: "", redirectTo: "/login", pathMatch: "full"},
    {path: "login", component: LoginComponent, canActivate: [authGuard]},
    {path: "forgot-password", component: ForgotPasswordComponent},
    {path: "reset-password", component: ResetPasswordComponent},
    {path: "users", component: UserComponent, canActivate: [expenseGuard]},
    {path: "users/view/:id", component: UserDetailComponent, canActivate: [expenseGuard]},
    {path: "users/edit/:id", component: UserEditComponent, canActivate: [expenseGuard]},
    {path: "users/create", component: CreateUserComponent, canActivate: [expenseGuard]},
    {path: "events", component: EventComponent, canActivate: [expenseGuard]},
    {path: "events/create", component: EventCreateComponent, canActivate: [expenseGuard]},
    {path: "events/view/:id", component: EventDetailComponent, canActivate: [expenseGuard]},
    {path: "candidates", component: CandidateComponent, canActivate: [expenseGuard]},
    {path: "candidates/create", component: CandidateCreateComponent, canActivate: [expenseGuard] }
];
