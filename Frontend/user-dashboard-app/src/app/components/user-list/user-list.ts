import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserList implements OnInit {
  @Output() addUser = new EventEmitter<void>();

  users: any[] = [];

  // Pagination properties
  currentPage: number = 1;
  itemsPerPage: number = 6; // Default items per page
  totalItems: number = 0;
  itemsPerPageOptions: number[] = [5, 10, 20, 50]; // Options for items per page

  constructor(private userService: UserService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getPaginatedUsers(this.currentPage, this.itemsPerPage).subscribe((response: any) => {
      // Access users from response.dataSource.$values
      const users = response.dataSource?.$values || [];

      this.users = users.map((user: any) => ({
        name: user.firstName + ' ' + user.lastName,
        email: user.email,
        role: user.roleId, // ตอนนี้มีแค่ roleId ถ้าอยากได้ชื่อ role ต้อง join เพิ่มใน backend
        date: new Date(user.createdDate).toLocaleDateString('en-US', {
          day: 'numeric',
          month: 'short',
          year: 'numeric'
        })
      }));

      // Update totalItems using response.totalCount
      this.totalItems = response.totalCount || 0;
    });
  }

  // Pagination actions
  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
    }
  }

  onItemsPerPageChange() {
    this.currentPage = 1; // Reset to first page when items per page changes
    this.loadUsers();
  }

  // Helper properties for template
  get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  get startIndex(): number {
    if (this.totalItems === 0) return 0; // Handle case with no items
    return (this.currentPage - 1) * this.itemsPerPage;
  }

  get endIndex(): number {
    if (this.totalItems === 0) return 0; // Handle case with no items
    const end = this.startIndex + this.itemsPerPage - 1;
    return Math.min(end, this.totalItems - 1);
  }

  get currentRange(): string {
    if (this.totalItems === 0) {
      return '0 of 0';
    } else {
      const start = this.startIndex + 1;
      const end = this.endIndex + 1;
      return `${start}-${end} of ${this.totalItems}`;
    }
  }
}