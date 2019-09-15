import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Observer } from 'rxjs';
import { PostList, Result, PostDetails } from './posts.interfaces';
import { retry, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  private endpoint = 'http://localhost:5100/api/posts/';

  constructor(private http: HttpClient) { }


  private getByRoute<T>(route: string, retry: number = 1): Observable<T> {
    const url = `${this.endpoint}${route}`;
    return this.http.get<T>(url);
  }

  getLatest(size: number): Observable<PostList[]> {
    const route = `getLastest?size=` + size;

    return this.getByRoute<Result<PostList>>(route)
    .pipe(
      map(r => r.result)
    );
  }

  get(id: number): Observable<PostDetails> {
    const route = `` + id;
    return this.getByRoute<any>(route)
    .pipe(
      map(r => r.result)
    );
  }
}
