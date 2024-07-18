import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';
import { routePaths } from '../route-paths/route-paths';
import { TaskService } from '../../services/task/task.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserService } from '../../services/user/user.service';
import { ApiResponse } from '../../models/ApiResponse';

export const customInterceptor: HttpInterceptorFn = (req, next) => {
  const userService=inject(UserService);
  const helper = new JwtHelperService();
  const toaster = inject(ToastrService);
  const router=inject(Router);
  const taskService=inject(TaskService);
  let accessToken = sessionStorage.getItem('AccessToken');
  let refreshToken = sessionStorage.getItem('RefreshToken');
  const isAccessTokenExpired = accessToken ? helper.isTokenExpired(accessToken) : true;
  const isRefreshTokenExpired = refreshToken ? helper.isTokenExpired(refreshToken) : true;
  if(req.url.split('/').pop()=="regenerate")
    {
        req = req.clone({
          headers: req.headers.set('Authorization', `Bearer ${refreshToken}`),
        });
        return catchErrorInRequest(req,next,toaster,router,taskService);
      }
    else if(accessToken && isAccessTokenExpired && !isRefreshTokenExpired)
      {
        userService.getTokens<ApiResponse>().subscribe({next:(value) => {
          sessionStorage.setItem("AccessToken", value.result[0]);
          sessionStorage.setItem("RefreshToken", value.result[1]);
          req = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${value.result[0]}`),
          });
          return catchErrorInRequest(req,next,toaster,router,taskService);
        }, error:(error) => {
          console.error('Error fetching tokens:', error);
        }});
      }
    else{
    req = req.clone({
    headers: req.headers.set('Authorization', `Bearer ${accessToken}`),
      });
      return catchErrorInRequest(req,next,toaster,router,taskService);
    }
}
function catchErrorInRequest(req:any,next:any,toaster:ToastrService,router:Router,taskService:TaskService)
    {
      return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
          if(error.status==401)
          {
            toaster.error("please login")
            router.navigate(routePaths.index);
            taskService.isLoading$.next(false);
            sessionStorage.removeItem('AccessToken');
            sessionStorage.removeItem('RefreshToken');
          }
          return throwError(() => error);
        }));
    }
