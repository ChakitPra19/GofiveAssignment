import { Component, Output, EventEmitter, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

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
  itemsPerPage: number = 5; // Default items per page
  totalItems: number = 0;
  itemsPerPageOptions: number[] = [5, 10, 20, 50]; // Options for items per page

  // Sorting properties
  currentSort: string = 'firstname';
  sortDirection: 'asc' | 'desc' = 'asc';

  // Search properties
  searchText: string = '';
  private searchSubject = new Subject<string>();

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {
    // Setup search debounce
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage = 1;
      this.loadUsers();
    });
  }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getPaginatedUsers(
      this.currentPage,
      this.itemsPerPage,
      this.currentSort,
      this.sortDirection,
      this.searchText
    ).subscribe({
      next: (response: any) => {
        const users = response.dataSource?.$values || [];

        this.users = users.map((user: any) => ({
          name: user.firstName + ' ' + user.lastName,
          email: user.email,
          role: user.roleId,
          date: new Date(user.createdDate).toLocaleDateString('en-US', {
            day: 'numeric',
            month: 'short',
            year: 'numeric'
          })
        }));

        this.totalItems = response.totalCount || 0;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error loading users:', error);
      }
    });
  }

  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
    }
  }

  onItemsPerPageChange(newValue: number) {
    this.itemsPerPage = newValue;
    this.currentPage = 1;
    this.loadUsers();
  }

  onSort(field: string) {
    if (this.currentSort === field) {
      // Toggle direction if same field
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // New field, set to ascending
      this.currentSort = field;
      this.sortDirection = 'asc';
    }
    this.loadUsers();
  }

  getSortIcon(field: string): string {
    if (this.currentSort !== field) return '↑';
    return this.sortDirection === 'asc' ? '↑' : '↓';
  }

  onSearch() {
    this.searchSubject.next(this.searchText);
  }

  // Helper properties for template
  get totalPages(): number {
    return Math.ceil(this.totalItems / this.itemsPerPage);
  }

  get startIndex(): number {
    if (this.totalItems === 0) return 0;
    return (this.currentPage - 1) * this.itemsPerPage;
  }

  get endIndex(): number {
    if (this.totalItems === 0) return 0;
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