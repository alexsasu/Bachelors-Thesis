import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {Endpoints} from "../enums/endpoints.enum";

@Injectable({
  providedIn: 'root'
})
export class UrlReportsService {
  constructor(private _http: HttpClient) { }

  getAllUrlReportsOfUser(user: any) {
    return this._http.get(Endpoints.URL_REPORTS + `/getAllReportsOfUser/${user.id}`);
  }
}
