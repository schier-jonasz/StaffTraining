import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

export const authCodeFlowConfig: AuthConfig = {
  // Url of the Identity Provider
  issuer: environment.identityServerUrl,

  // URL of the SPA to redirect the user to after login
  redirectUri: window.location.origin + '/index.html',
  silentRefreshRedirectUri:
    window.location.origin + '/assets/silent-refresh.html',
  // The SPA's id. The SPA is registerd with this id at the auth-server
  // clientId: 'server.code',
  clientId: 'staff-training-web',
  responseType: 'code',

  // set the scope for the permissions the client should request
  // The first four are defined by OIDC.
  // Important: Request offline_access to get a refresh token
  // The api scope is a usecase specific one
  scope: 'openid offline_access profile staff-training-api',

  showDebugInformation: true,
};
