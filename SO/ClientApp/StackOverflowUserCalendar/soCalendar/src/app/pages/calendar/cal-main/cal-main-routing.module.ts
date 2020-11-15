import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CalMainPage } from './cal-main.page';

const routes: Routes = [
  {
    path: '',
    component: CalMainPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CalMainPageRoutingModule {}
