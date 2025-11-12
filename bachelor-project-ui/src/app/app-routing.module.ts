import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from "./modules/home/components/home/home.component";
import { LoginComponent } from "./modules/auth/components/login/login.component";
import { RegisterComponent } from "./modules/auth/components/register/register.component";
import { ProfileComponent } from './modules/profile/components/profile/profile.component';
import { AuthGuard } from "./guards/auth.guard";
import {UrlReportsComponent} from "./modules/url-reports/components/url-reports/url-reports.component";

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'url-reports',
    component: UrlReportsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
