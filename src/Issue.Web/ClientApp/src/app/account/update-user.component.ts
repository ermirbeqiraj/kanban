import { Component, OnInit } from '@angular/core';
import { UserUpdateModel } from '../models/AccountModels';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-update-user',
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css']
})
export class UpdateUserComponent implements OnInit {
  currentId?: number;
  User: UserUpdateModel;
  userGroup: FormGroup;


  constructor(private authService: AuthService, private userBuilder: FormBuilder, private activatedRoute: ActivatedRoute, private router: Router) {
    this.userGroup = userBuilder.group({
      id: [""],
      userName: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]],
      phoneNumber: [""]
    })
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      if (params.id) {
        this.currentId = parseInt(params.id);
        this.getUser();
      }
      else
        console.log("Error");
    });
  }


  getUser() {
    if (this.currentId) {
      this.authService.getUserById(this.currentId).subscribe(response => {
        this.userGroup.patchValue({
          id: response.id,
          userName: response.userName,
          email: response.email,
          phoneNumber: response.phoneNumber
        });
      });
    }
    else
      console.log("Model is not Valid");
  }



  submit({ value, valid }: { value: UserUpdateModel, valid: boolean }) {
    if (valid) {
      this.authService.updateUser(value).subscribe(response => {
        this.router.navigate(['./list-user']);
      });
    }
    else
      console.log("Error");
  }
}
