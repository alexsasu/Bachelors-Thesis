import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UrlReportsComponent } from './components/url-reports/url-reports.component';
import { SharedModule } from "../shared/shared.module";

@NgModule({
  declarations: [
    UrlReportsComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ]
})
export class UrlReportsModule { }
