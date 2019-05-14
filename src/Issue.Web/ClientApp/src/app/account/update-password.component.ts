import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UpdateUserPassword } from '../models/AccountModels';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.css']
})
export class UpdatePasswordComponent implements OnInit {

  User: UpdateUserPassword;
  currentId?: number;
  userGroup: FormGroup;

  constructor(private authService: AuthService, private router: Router, private actRoute: ActivatedRoute, private fb: FormBuilder) {
    this.userGroup = fb.group({
      id: [""],
      password: ["",[Validators.required, Validators.minLength(6), Validators.maxLength(15)]],
      confirmPassword: ["", [Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    });
  }

  ngOnInit() {
    this.actRoute.params.subscribe(params => {
      if (params.id) {
        this.currentId = parseInt(params.id);
        this.userGroup.patchValue({
          id: params.id
        });
      }
    });
  }

  submit({ value, valid }: { value: UpdateUserPassword, valid: boolean }) {
    if (value.password != value.confirmPassword) {
      valid = false;
      window.alert("Password must match Confirm Password");
    }
    else {
      if (valid) {
        this.authService.updatePassword(value).subscribe(response => {
          this.router.navigate(['./list-user']);
        });
      }
      else {
        console.log("Error on submiting Form");
      }
    }
    
  }
}
