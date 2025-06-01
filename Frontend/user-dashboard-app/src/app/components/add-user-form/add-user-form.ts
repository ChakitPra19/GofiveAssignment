import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-user-form',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './add-user-form.html',
  styleUrls: ['./add-user-form.css']
})
export class AddUserForm {
  @Output() close = new EventEmitter<void>();
}