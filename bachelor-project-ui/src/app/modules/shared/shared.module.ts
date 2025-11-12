import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {UrlAnalysisCardComponent} from "./components/url-analysis-card/url-analysis-card.component";
import { HeaderComponent } from './components/header/header.component';
import { RouterModule } from "@angular/router";

@NgModule({
  declarations: [
    UrlAnalysisCardComponent,
    HeaderComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    UrlAnalysisCardComponent,
    HeaderComponent
  ]
})
export class SharedModule { }
