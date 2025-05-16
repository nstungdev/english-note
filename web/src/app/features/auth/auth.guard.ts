import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LOGIN_ROUTE } from '../../shared/constants/auth.constants';
import { isTokenExpired } from '../../shared/utils/jwt.util';
import { AuthService } from './services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private readonly router: Router,
    private readonly authService: AuthService
  ) {}

  canActivate():
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const token = this.authService.getAccessToken();
    if (token && !isTokenExpired(token)) {
      return true;
    }
    this.authService.logout();
    return this.router.createUrlTree([LOGIN_ROUTE]);
  }
}
