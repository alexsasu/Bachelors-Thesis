import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'bachelor-project-ui';

  constructor(private _authService: AuthService, private _userService: UserService) {
    this._authService.refreshToken();
    this._userService.refreshUser();
  }
}
