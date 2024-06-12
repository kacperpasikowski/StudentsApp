import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService, private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error) {
          switch (error.status) {
            case 400:
              if (error.error && error.error.errors && typeof error.error.errors === 'object') {
                const modelStateErrors = Object.values(error.error.errors).flat();
                this.toastr.error(modelStateErrors.join('\n'), 'Validation Error');
              } else if (error.error && typeof error.error === 'string') {
                this.toastr.error(error.error, 'Validation Error');
              } else {
                if (error.error && error.error.message && error.error.message.includes('username')) {
                  this.toastr.error('Username already exists. Please choose a different username.', 'Validation Error');
                } else {
                  this.toastr.error('Bad request. Please check your input and try again.', 'Validation Error');
                }
              }
              break;
            case 401:
              this.toastr.error('Unauthorized. Please log in to continue.', 'Error');
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              this.toastr.error('Server error. Please try again later.', 'Server Error');
              // Optional: Redirect to a generic error page
              this.router.navigateByUrl('/server-error');
              break;
            default:
              this.toastr.error('Something unexpected happened. Please try again later.', 'Error');
              console.error(error);
              break;
          }
        }
        return throwError(() => new Error(error.message));
      })
    );
  }
}
