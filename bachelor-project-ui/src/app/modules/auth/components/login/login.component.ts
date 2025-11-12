import { Component, OnDestroy, OnInit } from '@angular/core';
import {UntypedFormControl, UntypedFormGroup, Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, Observable, of, takeWhile } from 'rxjs';
import { User } from 'src/app/models/user.model';
import { NotificationType } from 'src/app/enums/notifications.enum';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationsService } from 'src/app/services/notifications.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {
  alive: boolean;
  loginForm: UntypedFormGroup;

  constructor(private _authService: AuthService, private _notificationsService: NotificationsService, private _router: Router, private _userService: UserService) {
    this.loginForm = new UntypedFormGroup({
      email: new UntypedFormControl(null),
      password: new UntypedFormControl(null, [Validators.required, Validators.minLength(8), Validators.maxLength(30)])
    });
  }

  ngOnInit(): void {
    this.alive = true;
  }

  submitLogin() {
    if (this.loginForm.valid) {
      const payload = this.loginForm.getRawValue();

      this._authService.login(payload).pipe(
        takeWhile(() => this.alive),
        catchError((error) => this.handleLoginError(error))
      ).subscribe((resLogin: any) => {
        if (resLogin && resLogin.token) {
          this._userService.getUserByEmail(payload.email).pipe(
            takeWhile(() => this.alive),
            catchError(err => this.handleLoginError(err))
          ).subscribe((resGetUser: User) => {
            if (!resGetUser) {
              return;
            }
            this._notificationsService.createMessage(NotificationType.SUCCESS, 'Success', resLogin.message);
            this._authService.setToken(resLogin.token);
            setTimeout(() => {
              this._router.navigate(['/home']);
              this._userService.setUser(resGetUser);
            }, 500);
          });
        }
      });
    } else {
      this._notificationsService.createMessage(NotificationType.ERROR, 'Error', "Invalid form input.");
    }
  }

  getUser(email: string) {
    this._userService.getUserByEmail(email).pipe(
      takeWhile(() => this.alive),
      catchError(err => this.handleLoginError(err))
    ).subscribe((res: User) => {
      if (!res) {
        return;
      }
      this._userService.setUser(res);
    });
  }

  handleLoginError(error): Observable<any> {
    const errorMessage = (error?.error?.message) || 'An error occurred on the server.';
    this._notificationsService.createMessage(NotificationType.ERROR, 'Error', errorMessage);
    return of(null);
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
