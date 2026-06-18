import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./layout/public-layout/public-layout.component').then(m => m.PublicLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/home/pages/landing/landing.component').then(m => m.LandingComponent)
      },
      {
        path: 'about',
        loadComponent: () => import('./features/home/pages/about/about.component').then(m => m.AboutComponent)
      },
      {
        path: 'tasks',
        loadComponent: () => import('./features/home/pages/tasks/tasks.component').then(m => m.TasksComponent)
      },
      {
        path: 'team',
        loadComponent: () => import('./features/home/pages/team/team.component').then(m => m.TeamComponent)
      },
      {
        path: 'auth/login',
        loadComponent: () => import('./features/identity/pages/login/login.component').then(m => m.LoginComponent)
      },
      {
        path: 'auth/register',
        loadComponent: () => import('./features/identity/pages/register/register.component').then(m => m.RegisterComponent)
      }
    ]
  },
  {
    path: 'app',
    loadComponent: () => import('./layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    children: [
      {
        path: 'audit/workspace',
        loadComponent: () => import('./features/audit/pages/audit-workspace/audit-workspace.component').then(m => m.AuditWorkspaceComponent)
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/dashboard/pages/dashboard-home/dashboard-home.component').then(m => m.DashboardHomeComponent)
      },
    ]
  },
  { path: '**', redirectTo: '' }
];