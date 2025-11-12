import { Component, OnDestroy, OnInit } from '@angular/core';
import { UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { takeWhile, catchError, Observable, of } from 'rxjs';
import { User } from 'src/app/models/user.model';
import { NotificationType } from 'src/app/enums/notifications.enum';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationsService } from 'src/app/services/notifications.service';
import { UserService } from 'src/app/services/user.service';
import { MatDialog } from "@angular/material/dialog";
import { ConfirmationDialogComponent } from "../confirmation-dialog/confirmation-dialog.component";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {
  alive: boolean;
  profileForm: UntypedFormGroup;
  user: User;

  constructor(private _authService: AuthService, private _dialog: MatDialog, private _notificationsService: NotificationsService, private _router: Router, private _userService: UserService) {
    this.profileForm = new UntypedFormGroup({
      email: new UntypedFormControl("", [Validators.required]),
      firstName: new UntypedFormControl("", [Validators.required]),
      lastName: new UntypedFormControl("", [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.alive = true;

    this._userService.getUser().pipe(
      takeWhile(() => this.alive),
    ).subscribe((user: User) => {
      if (user) {
        this.user = user;
        this.profileForm.patchValue({
          email: this.user.email,
          firstName: this.user.firstName,
          lastName: this.user.lastName
        });
      }
    });

    this.profileForm.get('email').disable();
  }

  get email(): UntypedFormControl {
    return this.profileForm.get('email') as UntypedFormControl;
  }

  get firstName(): UntypedFormControl {
    return this.profileForm.get('firstName') as UntypedFormControl;
  }

  get lastName(): UntypedFormControl {
    return this.profileForm.get('lastName') as UntypedFormControl;
  }

  saveChanges() {
    if (this.user.firstName !== this.profileForm.getRawValue().firstName ||
      this.user.lastName !== this.profileForm.getRawValue().lastName) {
      const payload = {
        id: this.user.id,
        firstName: this.profileForm.getRawValue().firstName,
        lastName: this.profileForm.getRawValue().lastName
      };

      this._userService.updateUserById(payload).pipe(
        takeWhile(() => this.alive),
        catchError(err => this.handleProfileError(err))
      ).subscribe((res) => {
        this._notificationsService.createMessage(NotificationType.SUCCESS, "Success", res.message);

        this.user.firstName = this.profileForm.getRawValue().firstName;
        this.user.lastName = this.profileForm.getRawValue().lastName;

        this.profileForm.patchValue({
          firstName: this.user.firstName,
          lastName: this.user.lastName
        });

        this._userService.setUser(this.user);
      });
    }
  }

  deleteAccount() {
    const payload = {
      id: this.user.id
    };

    this._userService.deleteUserById(payload).pipe(
      takeWhile(() => this.alive),
      catchError(err => this.handleProfileError(err))
    ).subscribe((res: any) => {
      this._userService.removeUser();
      this._authService.setToken();
      setTimeout(() => {
        this._router.navigate(['/login']);
      }, 500);
    });
  }

  openDialog() {
    const dialogRef = this._dialog.open(ConfirmationDialogComponent, {
      data: {
        message: 'Do you want to delete your account along with all your URL reports?'
      }
    });

    dialogRef.afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteAccount();
      }
    });
  }

  handleProfileError(error): Observable<any> {
    const errorMessage = (error?.error?.message) || 'An error occurred on the server.';
    this._notificationsService.createMessage(NotificationType.ERROR, 'Error', errorMessage);
    return of(null);
  }

  ngOnDestroy(): void {
    this.alive = false;
  }
}
