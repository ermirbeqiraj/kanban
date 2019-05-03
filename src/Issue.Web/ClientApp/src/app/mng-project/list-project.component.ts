import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../services/projects.service';
import { InternalProjectModel } from '../models/ProjectModels';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-list-project',
  templateUrl: './list-project.component.html',
  styleUrls: ['./list-project.component.css']
})
export class ListProjectComponent implements OnInit {

  data: InternalProjectModel[] = [];
  errorMessage: string;
  constructor(private projectsService: ProjectsService) { }

  ngOnInit() {
    this.getProjects();
  }

  getProjects() {
    this.projectsService.list().subscribe(response => { this.data = response });
  }

  delete(id: number) {
    this.errorMessage = "";
    this.projectsService.delete(id).subscribe(() => this.getProjects(), (error: HttpErrorResponse) => {
      this.errorMessage = error.error;
    });
  }
}
