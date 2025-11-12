import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UrlReportsComponent } from './url-reports.component';

describe('UrlReportsComponent', () => {
  let component: UrlReportsComponent;
  let fixture: ComponentFixture<UrlReportsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UrlReportsComponent]
    });
    fixture = TestBed.createComponent(UrlReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
