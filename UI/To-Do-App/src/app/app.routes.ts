import { Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { authGuard } from './shared/guard/auth.guard';
import { ActiveComponent } from './components/active/active.component';
import { CompletedComponent } from './components/completed/completed.component';
import { PendingComponent } from './components/pending/pending.component';
import { LandingComponent } from './components/landing/landing.component';
import { LoginComponent } from './components/login/login.component';

export const routes: Routes = [
  {
    path: 'index',
    component: LandingComponent,
  },
  {
    path: 'signup',
    component: LoginComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: '',
    component: HomeComponent,
    canActivate: [authGuard],
    children: [
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
        path:'pending',
        component:PendingComponent,
      },
      {
        path: '**',
        redirectTo: '/dashboard',
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];
