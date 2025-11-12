import { HttpClient } from "@angular/common/http";
import { Injectable } from '@angular/core';
import { Endpoints } from "../enums/endpoints.enum";
import { UrlAnalysisRequest } from "../models/urlAnalysisRequest.model";

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  constructor(private _http: HttpClient) { }

  getUrlAnalysis(urlAnalysisRequest: UrlAnalysisRequest) {
    return this._http.get(Endpoints.HOME + "/getUrlAnalysis", {params: {
        url: urlAnalysisRequest.url
      }});
  }

  addOrUpdateUrlReport(urlAnalysisRequest: UrlAnalysisRequest) {
    return this._http.put(Endpoints.HOME + "/addOrUpdateUrlReport", {
        url: urlAnalysisRequest.url,
        userId: urlAnalysisRequest.userId
      });
  }
}
