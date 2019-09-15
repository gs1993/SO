import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpEventType } from '@angular/common/http';
import { Observable, } from 'rxjs';
import { tap, map } from 'rxjs/operators';

@Injectable()
export class UnpackResponseInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        tap(event => {
          if (event.type !== HttpEventType.Response)
            return;

          let error: string | undefined = event.body.errorMessage;
          if (error !== undefined && error !== null && error.length > 0) {
            return event.clone({
              body: error
            });
          } else {
            var result = event.body.result;
            console.log(result);
            return event.clone({
              body: 'test'
            });
          }
        })
      );
  }
}