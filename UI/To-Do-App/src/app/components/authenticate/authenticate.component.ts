import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ErrorComponent } from 'c:/Users/anand.i/Downloads/To-Do App/UI/To-Do-App/src/app/shared/components/error/error.component';
import { ApiResponse } from '../../models/ApiResponse';
import { SpinnerComponent } from '../../shared/components/spinner/spinner.component';
import { TaskService } from '../../services/task/task.service';
import { UserService } from '../../services/user/user.service';
import { routePaths } from '../../shared/route-paths/route-paths';

@Component({
  selector: 'app-authenticate',
  standalone: true,
  templateUrl: './authenticate.component.html',
  styleUrl: './authenticate.component.scss',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ErrorComponent,
    SpinnerComponent,
  ],
})
export class AuthenticateComponent implements OnInit, AfterViewInit {
  @ViewChild('password') passwordRef!: ElementRef<HTMLInputElement>;
  @ViewChild('userName') userNameRef!: ElementRef<HTMLInputElement>;
  pageName: string = 'Sign Up';
  footerMsg: string = 'Already have an account? sign in';
  isSignUp: boolean = true;
  userForm!: FormGroup;
  isPasswordVisible: boolean = false;
  isSubmitted: boolean = false;
  requiredMessage: string = 'This Field is Required';
  passwordErrorMessaage: string =
    'Please ensure your password has at least 8 characters, including a capital letter, a lowercase letter, a digit, and a special symbol';
  usernameErrorMessage: string =
    'Username must start with an alphanumeric character and can only contain letters, numbers, underscores, and dots. Length must be between 3 and 20 characters.';
  constructor(
    private router: Router,
    private toaster: ToastrService,
    private userService: UserService,
    private taskService: TaskService
  ) {}
  ngOnInit() {
    this.isTokenExists();
    this.assignVariablesBasedOnPageName();
    this.createForm();
  }
  ngAfterViewInit(): void {
    this.passwordRef.nativeElement.placeholder = this.isSignUp
      ? 'Password'
      : 'Enter Your Password';
    this.userNameRef.nativeElement.placeholder = this.isSignUp
      ? 'Username'
      : 'Enter your username';
  }
  createForm() {
    this.userForm = new FormGroup({
      userName: new FormControl('', [
        Validators.required,
        Validators.pattern(
          /^(?=[a-zA-Z0-9._]{3,20}$)[a-zA-Z0-9][a-zA-Z0-9._]*$/
        ),
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.pattern(
          /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$/
        ),
      ]),
    });
  }
  assignVariablesBasedOnPageName() {
    var pageName = this.router.url.split('/').pop();
    if (pageName && pageName == 'signup') {
      this.isSignUp = true;
    } else {
      this.isSignUp = false;
    }
    this.pageName = this.isSignUp ? 'Sign Up' : 'Sign In';
    this.footerMsg = this.isSignUp
      ? 'Already have an account? sign in'
      : "Don't have an account? Create";
  }
  isTokenExists() {
    if (sessionStorage.getItem('Token') != null) {
      this.router.navigate(routePaths.dashboard);
    }
  }
  onSubmit() {
    this.isSubmitted = true;
    if (this.userForm.valid) {
      this.taskService.isLoading$.next(true);
      if (this.isSignUp) {
        this.addNewUser();
      } else {
        this.authenticateUser();
      }
    }
  }
  addNewUser() {
    this.userService.addUser<ApiResponse>(this.userForm.value).subscribe({
      next: (response) => {
        this.taskService.isLoading$.next(false);
        if (response.status == 1) {
          this.toaster.success(response.message);
            this.userForm.reset();
            this.toaster.warning('Sign in Now to view dashboard');
            this.taskService.isLoading$.next(false);
            this.navigate();
        } else {
          this.toaster.error(response.message);
        }
      },
      error: (error) => {
        this.taskService.isLoading$.next(false);
        this.toaster.error('Something went wrong. Please try again.');
      },
    });
  }
  authenticateUser() {
    this.userService
      .authenticateUser<ApiResponse>(this.userForm.value)
      .subscribe({
        next: (response) => {
          this.taskService.isLoading$.next(false);
          if (response.status == 1) {
            sessionStorage.setItem('Token', response.result);
              this.router.navigate(routePaths.dashboard);
            this.toaster.success(response.message);
          } else {
            this.toaster.error(response.message);
          }
        },
        error: (error) => {
          this.taskService.isLoading$.next(false);
          this.toaster.error('Something went wrong. Please try again.');
        },
      });
  }
  navigate() {
    this.clear();
    if (this.isSignUp) this.router.navigate(routePaths.login);
    else {
      this.router.navigate(routePaths.signup);
    }
  }
  clear() {
    this.userForm.reset();
    Object.keys(this.userForm.controls).forEach((field) => {
      this.userForm.get(field)!.setErrors(null);
    });
  }
  makePasswordVisible() {
    this.isPasswordVisible = !this.isPasswordVisible;
    this.passwordRef.nativeElement.type = this.isPasswordVisible
      ? 'text'
      : 'password';
  }
}
