export interface User {
  id: string;
  email: string;
  displayName: string;
  roles: string[];
}

export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  user: User;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest extends LoginRequest {
  displayName: string;
}
