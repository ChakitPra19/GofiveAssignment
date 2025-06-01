import { Routes } from '@angular/router';
import { AddUserForm } from './components/add-user-form/add-user-form';
import { UserList } from './components/user-list/user-list';

export const routes: Routes = [
  { path: '', component: AddUserForm }
];