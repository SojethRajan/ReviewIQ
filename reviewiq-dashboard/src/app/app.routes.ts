import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  },
  {
    path: 'dashboard',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component').then(
        (m) => m.DashboardComponent,
      ),
  },
  {
    path: 'pr/:id',
    loadComponent: () =>
      import('./features/pr-detail/pr-detail.component/pr-detail.component').then(
        (m) => m.PrDetailComponent,
      ),
  },
  {
    path: '**',
    redirectTo: 'dashboard',
  },
];
