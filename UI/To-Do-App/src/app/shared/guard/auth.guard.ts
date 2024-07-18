import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { routePaths } from '../route-paths/route-paths';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApiResponse } from '../../models/ApiResponse';
import { UserService } from '../../services/user/user.service';
export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Promise<boolean> => {
  const userService = inject(UserService); 
  const toaster = inject(ToastrService); // Instantiate your service if not injected
  const router = new Router(); // Instantiate your service if not injected
  const helper = new JwtHelperService();
  const accessToken = sessionStorage.getItem('AccessToken');
  const isAccessTokenExpired = accessToken ? helper.isTokenExpired(accessToken) : true;
  const refreshToken = sessionStorage.getItem('RefreshToken');
  const isRefreshTokenExpired = refreshToken ? helper.isTokenExpired(refreshToken) : true;

  if (accessToken && !isAccessTokenExpired ) {
    return true;
  } else if (!isRefreshTokenExpired) {
    return new Promise<boolean>((resolve) => {
      userService.getTokens<ApiResponse>().subscribe({next:(value) => {
        sessionStorage.setItem("AccessToken", value.result[0]);
        sessionStorage.setItem("RefreshToken", value.result[1]);
        resolve(true); 
      }, error:(error) => {
        console.error('Error fetching tokens:', error);
        resolve(false); 
      }});
    }); 
  } else {
    sessionStorage.removeItem('AccessToken');
    sessionStorage.removeItem('RefreshToken');
    toaster.error('Please login to continue');
    router.navigate(['/login']);
    return false;
  }
};