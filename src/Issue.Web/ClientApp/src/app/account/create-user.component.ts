import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserViewModel } from '../models/AccountModels';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnInit {
  user: UserViewModel;
  userGroup: FormGroup;

  constructor(private authService: AuthService, private fb: FormBuilder, private router: Router) {
    this.userGroup = fb.group({
      userName: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      phoneNumber: [""]
    });
  }

  ngOnInit() {
  }

  createUser() { }

  submit({ value, valid }: { value: UserViewModel, valid: boolean }) {

    if (valid) {
      this.authService.createUser(value).subscribe(response => {
        this.createUser();
        this.router.navigate(['./list-user']);
      });
    }
    else {
      console.log("Error");

    }
  }
}
