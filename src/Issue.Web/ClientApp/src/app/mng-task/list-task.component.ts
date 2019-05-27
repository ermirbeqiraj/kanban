import { Component, OnInit } from '@angular/core';
import { TaskService } from '../services/task.service';
import { TaskModel } from '../models/TaskModel';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-list-task',
  templateUrl: './list-task.component.html',
  styleUrls: ['./list-task.component.css']
})
export class ListTaskComponent implements OnInit {
  data: TaskModel[] = [];
  errorMessage: string;
  projectId?: number;

  constructor(private taskService: TaskService, private router: Router, private actRoute: ActivatedRoute) { }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      if (params.projectId) {
        this.projectId = parseInt(params.projectId);
        this.listTask(this.projectId);
      }
    });
  }

  listTask(projectId: number) {
    this.taskService.list(this.projectId).subscribe(response => { this.data = response; });
  }

  delete(id: number) {
    this.taskService.deleteTask(id).subscribe(() => this.listTask(this.projectId), (error: HttpErrorResponse) => {
      this.errorMessage = error.error;
    });
  }

}
