import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {
  AUTH_TOKEN_KEY,
  LOGIN_ROUTE,
} from '../../shared/constants/auth.constants';
import { isTokenExpired } from '../../shared/utils/jwt.util';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private readonly router: Router) {}

  canActivate():
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    const token = localStorage.getItem(AUTH_TOKEN_KEY);
    if (token && !isTokenExpired(token)) {
      return true;
    }
    localStorage.removeItem(AUTH_TOKEN_KEY);
    return this.router.createUrlTree([LOGIN_ROUTE]);
  }
}
