export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  createDate: string;
  permissions: {
    [role: string]: {
      read: boolean;
      write: boolean;
      delete: boolean;
    };
  };
}
