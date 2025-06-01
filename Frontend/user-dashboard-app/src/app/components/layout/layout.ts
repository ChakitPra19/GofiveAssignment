import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddUserForm } from '../add-user-form/add-user-form';
import { UserList } from '../user-list/user-list';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, AddUserForm, UserList, HttpClientModule],
  templateUrl: './layout.html',
  styleUrls: ['./layout.css']
})
export class LayoutComponent {
  showAddUser = false;
}