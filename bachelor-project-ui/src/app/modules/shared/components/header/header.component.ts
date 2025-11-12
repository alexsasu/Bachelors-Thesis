import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  alive: boolean;
  token: string;
  username: string;

  constructor(private _authService: AuthService, private _router: Router, private _userService: UserService) { }

  ngOnInit(): void {
    this.alive = true;
    if (localStorage.getItem("First Name")) {
      this.username = localStorage.getItem("First Name") + " " + localStorage.getItem("Last Name")
    }
  }

  navigateHome() {
    this._router.navigate(["/home"]);
  }

  isUserLoggedIn() {
    const status = this._userService.isLoggedIn();
    if (status) {
      this.username = localStorage.getItem("First Name") + " " + localStorage.getItem("Last Name");
    }
    return status;
  }

  logout() {
    this._userService.removeUser();
    this._authService.setToken();
    this.navigateHome();
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
