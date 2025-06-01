import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService, Role } from '../../services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-add-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-user-form.html',
  styleUrls: ['./add-user-form.css']
})
export class AddUserForm implements OnInit, OnDestroy {
  @Output() close = new EventEmitter<void>();
  @Output() userAdded = new EventEmitter<void>();

  userForm: FormGroup;
  roles: Role[] = [];
  errorMessage: string = '';
  private subscription: Subscription = new Subscription();

  constructor(
    private fb: FormBuilder,
    private userService: UserService
  ) {
    this.userForm = this.fb.group({
      userId: ['', [Validators.required]],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      roleId: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }
  }

  ngOnInit() {
    // Subscribe to roles changes
    this.subscription.add(
      this.userService.roles$.subscribe({
        next: (roles) => {
          this.roles = roles;
        },
        error: (error) => {
          console.error('Error getting roles:', error);
          this.errorMessage = 'Failed to load roles';
        }
      })
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onSubmit() {
    if (this.userForm.valid) {
      const formValue = this.userForm.value;
      // Remove confirmPassword before sending to API
      const { confirmPassword, ...userData } = formValue;
      
      this.userService.addUser(userData).subscribe({
        next: () => {
          this.userService.getUsers().subscribe(); // Reload users immediately
          this.userAdded.emit();
          this.close.emit();
        },
        error: (error) => {
          this.errorMessage = error.error || 'Failed to add user';
        }
      });
    }
  }

  onCancel() {
    this.close.emit();
  }
}