import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginViewModel } from '../../models/AccountModels';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errorMessage: string;
  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) {
    this.form = fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit() {
  }

  onSubmit({ value, valid }: { value: LoginViewModel, valid: boolean }) {
    this.errorMessage = '';
    if (valid) {
      console.log(value);
      this.authService.login(value).subscribe(response => {
        localStorage.setItem('token', response.access_token);
        localStorage.setItem('roles', JSON.stringify(response.roles));

        this.router.navigate(['/']);
      }, (error: HttpErrorResponse) => {
        this.errorMessage = error.error;
      });
    }
    else {
      this.errorMessage = "Form is not valid";
    }
  }
}
