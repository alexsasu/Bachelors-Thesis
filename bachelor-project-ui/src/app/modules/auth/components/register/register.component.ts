import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { catchError, take, takeWhile } from 'rxjs/operators';
import { NotificationsService } from 'src/app/services/notifications.service';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { NotificationType } from 'src/app/enums/notifications.enum';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  alive: boolean;
  registerForm: UntypedFormGroup;

  constructor(private _authService: AuthService, private _notificationsService: NotificationsService, private _router: Router) {
    this.registerForm = new UntypedFormGroup({
      email: new UntypedFormControl(null, [Validators.required, Validators.email]),
      password: new UntypedFormControl(null, [Validators.required, Validators.minLength(8), Validators.maxLength(30)]),
      confirmPassword: new UntypedFormControl(null, [Validators.required, Validators.minLength(8), Validators.maxLength(30)]),
      firstName: new UntypedFormControl(null, [Validators.required]),
      lastName: new UntypedFormControl(null, [Validators.required])
    });
  }

  ngOnInit(): void {
    this.alive = true;
  }

  get email(): UntypedFormControl {
    return this.registerForm.get('email') as UntypedFormControl;
  }

  get password(): UntypedFormControl {
    return this.registerForm.get('password') as UntypedFormControl;
  }

  get firstName(): UntypedFormControl {
    return this.registerForm.get('firstName') as UntypedFormControl;
  }

  get lastName(): UntypedFormControl {
    return this.registerForm.get('lastName') as UntypedFormControl;
  }

  submitRegister() {
    if (this.registerForm.valid) {
      if (this.registerForm.getRawValue().password !== this.registerForm.getRawValue().confirmPassword) {
        this._notificationsService.createMessage(NotificationType.ERROR, 'Error', 'The same password must be entered in both fields.');
        return;
      }

      const payload = {
        email: this.registerForm.getRawValue().email,
        password: this.registerForm.getRawValue().password,
        firstName: this.registerForm.getRawValue().firstName,
        lastName: this.registerForm.getRawValue().lastName
      };

      this._authService.register(payload).pipe(
        takeWhile(() => this.alive),
        catchError((error) => this.handleRegisterError(error))
      ).subscribe((res: any) => {
        if (res) {
          this._notificationsService.createMessage(NotificationType.SUCCESS, 'Success', res.message);
          setTimeout(() => {
            this._router.navigate(['/login']);
          }, 1000);
        }
      });
    } else {
      this._notificationsService.createMessage(NotificationType.ERROR, 'Error', "Invalid form input.");
    }
  }

  handleRegisterError(error): Observable<any> {
    const errorMessage = (error?.error?.message) || 'An error occurred on the server.';
    console.log(errorMessage);
    this._notificationsService.createMessage(NotificationType.ERROR, 'Error', errorMessage);
    return of(null);
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
