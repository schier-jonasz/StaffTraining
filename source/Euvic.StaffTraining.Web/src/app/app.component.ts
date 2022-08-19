import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { filter } from 'rxjs';
import { authCodeFlowConfig } from './core/interceptors/oidc/oidc.configuration';
import { LookupsService } from './features/shared/services/lookups.service';
import { AttendeeProfileService } from './features/shared/services/profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'staff-training';

  constructor(
    private lookupsService: LookupsService,
    private oauthService: OAuthService,
    private attendeeProfileService: AttendeeProfileService,
    private router: Router
  ) {
    this.configureCodeFlow();
  }

  ngOnInit(): void {}

  private configureCodeFlow() {
    this.oauthService.configure(authCodeFlowConfig);
    this.oauthService
      .loadDiscoveryDocumentAndLogin({
        customHashFragment: window.location.search,
      })
      .then((hasReceivedTokens) => {
        if (hasReceivedTokens) {
          this.oauthService.setupAutomaticSilentRefresh();
          return Promise.resolve();
        } else {
          if (
            this.oauthService.hasValidAccessToken() &&
            this.oauthService.hasValidIdToken()
          ) {
            this.oauthService.setupAutomaticSilentRefresh();
            return Promise.resolve();
          } else {
            return new Promise((resolve) => {
              this.oauthService.initCodeFlow();
            });
          }
        }
      })
      .then(() => {
        this.init();
        this.router.navigateByUrl('trainings');
      });
  }

  private init() {
    this.attendeeProfileService.getProfile();
  }
}
