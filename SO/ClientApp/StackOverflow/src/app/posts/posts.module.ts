import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';
import {TimeAgoPipe} from 'time-ago-pipe';

import { PostsPage } from './posts.page';

const routes: Routes = [
  {
    path: '',
    component: PostsPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [PostsPage, TimeAgoPipe]
})
export class PostsPageModule {}
