import { Component, OnDestroy, OnInit, Input } from '@angular/core';
import { UrlAnalysis } from 'src/app/models/urlAnalysis.model';

@Component({
  selector: 'app-url-analysis-card',
  templateUrl: './url-analysis-card.component.html',
  styleUrls: ['./url-analysis-card.component.scss']
})
export class UrlAnalysisCardComponent implements OnInit, OnDestroy {
  @Input() urlAnalysis: UrlAnalysis;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }
}
