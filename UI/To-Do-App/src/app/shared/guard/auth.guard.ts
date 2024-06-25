import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { routePaths } from '../route-paths/route-paths';

export const authGuard: CanActivateFn = (route, state) => {
  const toaster: ToastrService = inject(ToastrService);
  const router: Router = inject(Router);
  const token = sessionStorage.getItem('Token');
  if (token != null) {
    return true;
  } else {
    toaster.error('please login to continue');
    router.navigate(routePaths.index);
    return false;
  }
};
