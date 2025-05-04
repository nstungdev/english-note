export interface AuthData {
  userId: number;
  username: string;
  email: string;
  accessToken: string;
  refreshToken: string;
}

export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}
