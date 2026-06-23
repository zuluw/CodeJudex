export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  email: string;
  fullName: string;
  expiresAt: string;
}

export type UserRole = 'Student' | 'Admin' | 'Teacher' | 'Hr';