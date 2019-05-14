import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InternalProjectModel } from '../models/ProjectModels';
import { Observable } from 'rxjs';
import { UpdateUserPassword } from '../models/AccountModels';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {
  private appUrl: string = '/api/projects';
  constructor(private http: HttpClient) { }

  list(): Observable<InternalProjectModel[]> {
    let url = `${this.appUrl}/List`;
    return this.http.get<InternalProjectModel[]>(url);
  }

  getById(id: number): Observable<InternalProjectModel> {
    let url = `${this.appUrl}/GetById?id=${id}`;
    return this.http.get<InternalProjectModel>(url);
  }

  create(model: InternalProjectModel): Observable<void> {
    let url = `${this.appUrl}/Create`;
    return this.http.post<void>(url, model);
  }

  update(model: InternalProjectModel): Observable<void> {
    let url = `${this.appUrl}/Update`;
    return this.http.put<void>(url, model);
  }

  delete(id: number): Observable<void> {
    let url = `${this.appUrl}/Delete?id=${id}`;
    return this.http.delete<void>(url);
  }

  updatePassword(model: UpdateUserPassword): Observable<UpdateUserPassword> {
    let url = `${this.appUrl}/UpdatePassword`;
    return this.http.put<UpdateUserPassword>(url, model);
  }
}
