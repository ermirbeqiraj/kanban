export class LoginViewModel {
  username: string;
  password: string;
}

export class TokenViewModel {
  access_token: string;
  expires_in: Date;
  refresh_token: string;
  roles: string[];
}
