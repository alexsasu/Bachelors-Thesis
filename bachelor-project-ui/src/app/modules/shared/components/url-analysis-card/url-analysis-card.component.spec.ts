import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UrlAnalysisCardComponent } from './url-analysis-card.component';

describe('UrlAnalysisCardComponent', () => {
  let component: UrlAnalysisCardComponent;
  let fixture: ComponentFixture<UrlAnalysisCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UrlAnalysisCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UrlAnalysisCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
