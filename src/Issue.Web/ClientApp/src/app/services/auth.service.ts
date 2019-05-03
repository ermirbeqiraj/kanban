import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LoginViewModel, TokenViewModel } from '../models/AccountModels';

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
}
