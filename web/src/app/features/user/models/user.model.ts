export interface User {
  id: number;
  username: string;
  email: string;
  permissions: string[];
  groups: string[];
  isBlocked: boolean;
}

export interface Permission {
  id: number;
  name: string;
  description: string;
}

export interface Group {
  id: number;
  name: string;
  description: string;
}
