import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HomeModule } from "./modules/home/home.module";
import { SharedModule } from "./modules/shared/shared.module";
import { ProfileModule } from "./modules/profile/profile.module";
import { AuthModule } from "./modules/auth/auth.module";
import { MessageService } from 'primeng/api';
import { MessagesModule } from 'primeng/messages';
import { StyleClassModule } from 'primeng/styleclass';
import { UserService } from './services/user.service';
import { AuthGuard } from './guards/auth.guard';
import { AuthInterceptor } from "./interceptors/auth.interceptor";
import { RouterModule } from "@angular/router";
import { UrlReportsModule } from "./modules/url-reports/url-reports.module";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MessagesModule,
    RouterModule,
    StyleClassModule,
    HomeModule,
    UrlReportsModule,
    AuthModule,
    ProfileModule,
    SharedModule
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    UserService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
