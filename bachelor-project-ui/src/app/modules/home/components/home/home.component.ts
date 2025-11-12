import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { HomeService } from "../../../../services/home.service";
import { UntypedFormControl, UntypedFormGroup, Validators } from "@angular/forms";
import { UrlAnalysisRequest } from 'src/app/models/urlAnalysisRequest.model';
import { UrlAnalysis } from "../../../../models/urlAnalysis.model";
import { catchError, Observable, of, takeWhile } from "rxjs";
import { NotificationsService } from "../../../../services/notifications.service";
import { NotificationType } from "../../../../enums/notifications.enum";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy, OnChanges {
  alive: boolean;
  @Input() urlAnalysisRequest!: UrlAnalysisRequest;
  urlAnalysisRequestForm: UntypedFormGroup;
  urlsAnalyses: UrlAnalysis[] = [];

  constructor(private _homeService: HomeService, private _notificationsService: NotificationsService) {
    this.urlAnalysisRequestForm = new UntypedFormGroup({
      url: new UntypedFormControl(null, [Validators.required]),
      userId: new UntypedFormControl(null)
    });
  }

  ngOnInit(): void {
    this.alive = true;
    document.getElementById("loading").style.display = "none";
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['urlAnalysisRequest']) {
      if (this.urlAnalysisRequest) {
        this.urlAnalysisRequestForm.patchValue(this.urlAnalysisRequest);
      } else {
        this.urlAnalysisRequestForm.reset();
      }
    }
  }

  processUrl() {
    if (!this.isValidUrl(this.urlAnalysisRequestForm.getRawValue().url)) {
      this._notificationsService.createMessage(NotificationType.ERROR, 'Error', "Please input a valid URL.");
      return;
    }

    this.urlsAnalyses = [];

    document.getElementById("loading").style.display = "inline-block";

    if (localStorage.getItem("User Id") === null) {
      const payload = {
        url: this.urlAnalysisRequestForm.getRawValue().url,
        userId: null
      };

      this._homeService.getUrlAnalysis(payload).pipe(
        takeWhile(() => this.alive),
        catchError(err => this.handleHomeError(err))
      ).subscribe((urlAnalysis: UrlAnalysis) => {
        this.urlsAnalyses = [urlAnalysis];
        document.getElementById("loading").style.display = "none";
      });
    } else {
      const payload = {
        url: this.urlAnalysisRequestForm.getRawValue().url,
        userId: Number(localStorage.getItem("User Id"))
      };

      this._homeService.addOrUpdateUrlReport(payload).pipe(
        takeWhile(() => this.alive),
        catchError(err => this.handleHomeError(err))
      ).subscribe((urlAnalysis: UrlAnalysis) => {
        this.urlsAnalyses = [urlAnalysis];
        document.getElementById("loading").style.display = "none";
      });
    }
  }

  isValidUrl(urlString: string) {
    let url;
    try {
      url = new URL(urlString);
    }
    catch(e) {
      return false;
    }
    return url.protocol === "http:" || url.protocol === "https:";
  }

  handleHomeError(error): Observable<any> {
    const errorMessage = (error?.error?.message) || 'An error occurred on the server.';
    this._notificationsService.createMessage(NotificationType.ERROR, 'Error', errorMessage);
    return of(null);
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
