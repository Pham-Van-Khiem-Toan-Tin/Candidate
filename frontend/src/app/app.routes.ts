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

export const routes: Routes = [
    {path: "", redirectTo: "/login", pathMatch: "full"},
    {path: "login", component: LoginComponent},
    {path: "users", component: UserComponent, canActivate: [expenseGuard]},
    {path: "users/view/:id", component: UserDetailComponent, canActivate: [expenseGuard]},
    {path: "users/edit/:id", component: UserEditComponent, canActivate: [expenseGuard]},
    {path: "forgot-password", component: ForgotPasswordComponent},
    {path: "reset-password", component: ResetPasswordComponent},
    {path: "users/create", component: CreateUserComponent},
    {path: "events", component: EventComponent, canActivate: [expenseGuard]}
];
