import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, catchError, take } from "rxjs";
import { Endpoints } from "../enums/endpoints.enum";
import { User } from "../models/user.model";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  userValue = new BehaviorSubject<User>(null);

  constructor(private _http: HttpClient) { }

  getUserByEmail(email: string) {
    return this._http.get(Endpoints.USER + `/getByEmail/${email}`);
  }

  updateUserById(user: any) {
    return this._http.put(Endpoints.USER + `/updateById/${user.id}`, {
      firstName: user.firstName,
      lastName: user.lastName
    });
  }

  deleteUserById(user: any) {
    return this._http.delete(Endpoints.USER + `/deleteById/${user.id}`);
  }

  setUser(user: User) {
    localStorage.setItem("User Id", user.id.toString());
    localStorage.setItem("User", user.email);
    localStorage.setItem("First Name", user.firstName);
    localStorage.setItem("Last Name", user.lastName);

    this.userValue.next(user);
  }

  getUser() {
    return this.userValue.asObservable();
  }

  refreshUser() {
    const user = localStorage.getItem("User");
    if (!user) {
      return;
    }
    this.getUserByEmail(user).pipe(
      take(1),
      catchError(err => err)
    ).subscribe((res: User) => {
      if (!res) {
        return;
      }
      this.userValue.next(res);
    });
  }

  removeUser() {
    localStorage.removeItem('User Id');
    localStorage.removeItem('User');
    localStorage.removeItem('First Name');
    localStorage.removeItem('Last Name');

    this.userValue.next(null);
  }

  isLoggedIn() {
    const token = localStorage.getItem('Token');
    const user = this.userValue.getValue();

    if (token && user) {
      return true;
    }
    return false;
  }
}
