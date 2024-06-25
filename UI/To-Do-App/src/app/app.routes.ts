import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { authGuard } from './shared/guard/auth.guard';
import { ActiveComponent } from './components/active/active.component';
import { AuthenticateComponent } from './components/authenticate/authenticate.component';
import { CompletedComponent } from './components/completed/completed.component';
import { IndexComponent } from './components/index/index.component';

export const routes: Routes = [
  {
    path: '',
    component: IndexComponent,
  },
  {
    path: 'signup',
    component: AuthenticateComponent,
  },
  {
    path: 'login',
    component: AuthenticateComponent,
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        redirectTo: '/home/dashboard',
        pathMatch: 'full',
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'active',
        component: ActiveComponent,
      },
      {
        path: 'completed',
        component: CompletedComponent,
      },
      {
        path: 'home/**',
        redirectTo: '/dashboard',
      },
    ],
  },
  {
    path: '**',
    redirectTo: '/home/dashboard',
  },
];
