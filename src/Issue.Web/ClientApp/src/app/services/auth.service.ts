import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LoginViewModel, TokenViewModel, UserForListModel, UserViewModel, UserUpdateModel, UpdateUserPassword } from '../models/AccountModels';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private appUrl: string = '/api/account';
  constructor(private http: HttpClient) { }

  login(model: LoginViewModel): Observable<TokenViewModel> {
    let url = `${this.appUrl}/Token`;
    return this.http.post<TokenViewModel>(url, model);
  }

  list(): Observable<UserForListModel[]> {
    let url = `${this.appUrl}/List`;
    return this.http.get<UserForListModel[]>(url);
  }
  createUser(model: UserViewModel): Observable<UserViewModel> {
    let url = `${this.appUrl}/CreateUser`;
    return this.http.post<UserViewModel>(url, model);
  }

  getUserById(id: number): Observable<UserUpdateModel> {
    let url = `${this.appUrl}/getUserById?id=${id}`;
    return this.http.get<UserUpdateModel>(url);
  }

  updateUser(model: UserUpdateModel): Observable<UserUpdateModel> {
    let url = `${this.appUrl}/UpdateUser`;
    return this.http.put<UserUpdateModel>(url, model);
  }

  deleteUser(id: number): Observable<void> {
    let url = `${this.appUrl}/DeleteUser?userId=${id}`;
    return this.http.delete<void>(url);
  }

  updatePassword(model: UpdateUserPassword): Observable<UpdateUserPassword> {
    let url = `${this.appUrl}/UpdatePassword`;
    return this.http.put<UpdateUserPassword>(url, model);
    
  }
}
