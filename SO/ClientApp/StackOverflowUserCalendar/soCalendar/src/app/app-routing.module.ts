import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./home/home.module').then( m => m.HomePageModule)
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'cal-modal',
    loadChildren: () => import('./pages/calendar/cal-modal/cal-modal.module').then( m => m.CalModalPageModule)
  },
  {
    path: 'cal-main',
    loadChildren: () => import('./pages/calendar/cal-main/cal-main.module').then( m => m.CalMainPageModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
