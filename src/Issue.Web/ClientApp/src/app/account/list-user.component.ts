import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserForListModel } from '../models/AccountModels';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-list-user',
  templateUrl: './list-user.component.html',
  styleUrls: ['./list-user.component.css']
})
export class ListUserComponent implements OnInit {
  data: UserForListModel[] = [];
  errorMessage: string;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.authService.list().subscribe(response => { this.data = response });
  }

  delete(id: number) {
    this.errorMessage = "";
    this.authService.deleteUser(id).subscribe(() => this.getUsers(), (error: HttpErrorResponse) => {
      this.errorMessage = error.error;
    });

  }
}
