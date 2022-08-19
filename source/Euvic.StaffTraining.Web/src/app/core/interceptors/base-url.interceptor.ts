import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs/internal/Observable';
import { AttendeeProfileService } from 'src/app/features/shared/services/profile.service';

@Injectable()
export class BaseUrlInterceptor implements HttpInterceptor {
  constructor(
    @Inject('BASE_API_URL') private baseUrl: string,
    private injector: Injector
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authService = this.injector.get(OAuthService);
    let apiRequest = request.clone({
      url: !request.url.includes('http')
        ? `${this.baseUrl}/${request.url}`
        : request.url,
      setHeaders: {
        Authorization: `Bearer ${authService.getAccessToken()}`,
      },
    });

    return next.handle(apiRequest);
  }
}
