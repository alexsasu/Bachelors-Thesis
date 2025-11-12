import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Endpoints } from "../enums/endpoints.enum";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  tokenValue = new BehaviorSubject<string>(null);

  constructor(private _http: HttpClient) { }

  register(payload: { email: string, password: string, firstName: string, lastName: string }) {
    return this._http.post(Endpoints.REGISTER, payload);
  }

  login(payload: { name: string, password: string }) {
    return this._http.post(Endpoints.LOGIN, payload);
  }

  setToken(token?: string) {
    this.tokenValue.next(token);

    if (token) {
      localStorage.setItem('Token', token);
    } else {
      localStorage.removeItem('Token');
    }
  }

  refreshToken() {
    const token = localStorage.getItem("Token");
    if (!token) {
      return;
    }
    this.tokenValue.next(token);
  }
}
