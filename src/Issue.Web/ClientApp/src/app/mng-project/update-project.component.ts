import { Component, OnInit } from '@angular/core';
import { InternalProjectModel } from '../models/ProjectModels';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ProjectsService } from '../services/projects.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-update-project',
  templateUrl: './update-project.component.html',
  styleUrls: ['./update-project.component.css']
})
export class UpdateProjectComponent implements OnInit {
  intProject: InternalProjectModel;
  currentId?: number;
  projectForm: FormGroup;

  constructor(private projectService: ProjectsService, private activatedRouter: ActivatedRoute, private formBuilder: FormBuilder, private route: Router) {
    this.projectForm = formBuilder.group({
      id: ['', Validators.required],
      name: ['', [Validators.required, Validators.maxLength(50)]],
      description: ['', Validators.maxLength(500)],
      repositoryUrl: ['', Validators.maxLength(300)],
      stagingUrl: ['', Validators.maxLength(300)]
    });
  }

  ngOnInit() {
    this.activatedRouter.params.subscribe(params => {
      if (params.id) {
        this.currentId = parseInt(params.id);
        this.getProject();
      }
      else {
        console.log("Error");
      }
    });
  }

  getProject() {
    if (this.currentId) {
      this.projectService.getById(this.currentId).subscribe(response => {
        this.projectForm.patchValue({
          id: response.id,
          name: response.name,
          description: response.description,
          repositoryUrl: response.repositoryUrl,
          stagingUrl: response.stagingUrl
        });
      });
    }
    else
      console.log("Nuk vjen Projekti");
  }

  submit({ value, valid }: { value: InternalProjectModel, valid: boolean }) {
    if (valid) {
      this.projectService.update(value).subscribe(response => {
        this.route.navigate(['./list-project']);
      })
    }
    else {
      console.log("ERROR");
    }
  }
}
