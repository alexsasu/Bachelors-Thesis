import { Component, OnDestroy, OnInit } from '@angular/core';
import { UrlReportsService } from "../../../../services/url-reports.service";
import {catchError, distinctUntilChanged, Observable, of, takeWhile} from "rxjs";
import { UrlAnalysis } from "../../../../models/urlAnalysis.model";
import { NotificationType } from "../../../../enums/notifications.enum";
import { NotificationsService } from "../../../../services/notifications.service";

@Component({
  selector: 'app-url-reports',
  templateUrl: './url-reports.component.html',
  styleUrls: ['./url-reports.component.scss']
})
export class UrlReportsComponent implements OnInit, OnDestroy {
  alive: boolean;
  urlsReports: UrlAnalysis[] = [];

  constructor(private _notificationsService: NotificationsService, private _urlReportsService: UrlReportsService) { }

  ngOnInit(): void {
    this.alive = true;

    const payload = {
      id: Number(localStorage.getItem("User Id"))
    };

    this._urlReportsService.getAllUrlReportsOfUser(payload).pipe(
      takeWhile(() => this.alive),
      distinctUntilChanged(),
      catchError(err => this.handleUrlReportsError(err))
    ).subscribe((urlsReports: UrlAnalysis[]) => {
      this.urlsReports = urlsReports;
    });
  }

  handleUrlReportsError(error): Observable<any> {
    const errorMessage = (error?.error?.message) || 'An error occurred on the server.';
    this._notificationsService.createMessage(NotificationType.ERROR, 'Error', errorMessage);
    return of(null);
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
