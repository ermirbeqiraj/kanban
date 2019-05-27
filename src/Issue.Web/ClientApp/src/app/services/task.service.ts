import { Injectable } from '@angular/core';
import { TaskModel } from '../models/TaskModel';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private appUrl: string = '/api/task';
  constructor(private http: HttpClient) { }

  list(projectId: number): Observable<TaskModel[]> {
    let url = `${this.appUrl}/List?projectId=${projectId}`;
    return this.http.get<TaskModel[]>(url);
  }

  getTaskById(id: number): Observable<TaskModel> {
    let url = `${this.appUrl}/GetTaskById?id=${id}`;
    return this.http.get<TaskModel>(url);
  }

  createTask(projectId: number, model: TaskModel): Observable<TaskModel> {
    let url = `${this.appUrl}/CreateTask?projectId=${projectId}`;
    return this.http.post<TaskModel>(url, model);
  }

  updateTask(projectId: number ,model: TaskModel): Observable<TaskModel> {
    let url = `${this.appUrl}/UpdateTask?projectId=${projectId}`;
    return this.http.put<TaskModel>(url, model);
  }

  deleteTask(id: number): Observable<void> {
    let url = `${this.appUrl}/DeleteTask?id=${id}`;
    return this.http.delete<void>(url);
  }
}
