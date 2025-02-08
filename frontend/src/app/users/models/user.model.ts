export interface User {
    id: string;
    name: string;
    email: string;
    username: string;
    address: string;
    birthDate: string;
    phoneNumber: string;
    role: 'Admin' | 'User';
    createdAt: string;
    updatedAt: string;
  }
  