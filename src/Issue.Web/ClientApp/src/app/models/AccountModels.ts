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

export class UserForListModel {
  id: number;
  email: string;
  userName: string;
}

export class UserViewModel {
  id: number;
  email: string;
  password: string;
  phoneNumber: string;
  userName: string;
}

export class UserUpdateModel {
  id: number;
  email: string;
  phoneNumber: string;
  userName: string;
}



export class UpdateUserPassword {
  id: number;
  password: string;
  confirmPassword: string;
}
