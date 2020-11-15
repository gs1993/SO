import { NgModule } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { IonicModule } from '@ionic/angular';

import { CalMainPageRoutingModule } from './cal-main-routing.module';

import { CalMainPage } from './cal-main.page';
import { CalModalPageModule } from '../cal-modal/cal-modal.module';
import { NgCalendarModule } from 'ionic2-calendar';
import localePl from '@angular/common/locales/pl';
registerLocaleData(localePl);

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CalMainPageRoutingModule,
    NgCalendarModule,
    CalModalPageModule,
  ],
  declarations: [CalMainPage]
})
export class CalMainPageModule {}
