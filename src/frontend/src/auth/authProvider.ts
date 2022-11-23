import {
  MsalAuthProvider,
  LoginType,
  IMsalAuthProviderConfig,
} from 'react-aad-msal';
import { Configuration, AuthenticationParameters } from 'msal';

const USER_IMPERSONATION_SCOPE = `${process.env.REACT_APP_TAM_CLIENT_ID}/user_impersonation`;

// Msal Configurations
const config: Configuration = {
  auth: {
    authority: process.env.REACT_APP_TAM_AUTHORITY,
    clientId: process.env.REACT_APP_TAM_CLIENT_ID || '',
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: false,
  },
};

// Authentication Parameters
const authenticationParameters: AuthenticationParameters = {
  scopes: [
    USER_IMPERSONATION_SCOPE || '',
  ],
};

// Options
const options: IMsalAuthProviderConfig = {
  loginType: LoginType.Redirect,
  tokenRefreshUri: `${window.location.origin}/auth.html`,
};

export const authProvider = new MsalAuthProvider(
  config,
  authenticationParameters,
  options,
);
