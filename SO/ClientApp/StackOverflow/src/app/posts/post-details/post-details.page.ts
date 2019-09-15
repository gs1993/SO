import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import { PostsService } from '../posts.service';
import { PostDetails } from '../posts.interfaces';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.page.html',
  styleUrls: ['./post-details.page.scss'],
})
export class PostDetailsPage implements OnInit {
  post: PostDetails;

  constructor(private route: ActivatedRoute,
              private postService: PostsService) { }

  ngOnInit() {
    const postId: number = parseInt(this.route.snapshot.params['id']);
    this.postService.get(postId).subscribe(
      (data: PostDetails) => { this.post = data; console.log(this.post); }
    );
  }

}
