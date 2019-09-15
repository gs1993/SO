import { Component, OnInit } from '@angular/core';
import { PostsService } from './posts.service';
import { PostList } from './posts.interfaces';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.page.html',
  styleUrls: ['./posts.page.scss'],
})
export class PostsPage implements OnInit {
  posts: PostList[];

  constructor(
    private postService: PostsService
  ) { }

  ngOnInit() {
    this.getLastestPosts();
  }

   getLastestPosts() {
    return this.postService.getLatest(50)
      .subscribe((data: PostList[]) => { this.posts = data; }
    );
  }
}
