import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../services/projects.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InternalProjectModel } from '../models/ProjectModels';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.css']
})
export class CreateProjectComponent implements OnInit {
  intProject: InternalProjectModel;
  projectForm: FormGroup;

  constructor(private projectsService: ProjectsService, private formBuilder: FormBuilder, private router: Router) {
    this.projectForm = formBuilder.group({
      name: ["", [Validators.required, Validators.maxLength(50)]],
      description: ["", Validators.maxLength(500)],
      repositoryUrl: ["", Validators.maxLength(300)],
      stagingUrl: ["", Validators.maxLength(300)]
    });
  }

  ngOnInit() {
  }
  create() { }


  submit({ value, valid }: { value: InternalProjectModel, valid: boolean }) {
    if (valid) {
      // continue
      this.projectsService.create(value).subscribe(response => {
        this.create();
        if (valid)
          this.router.navigate(['./list-project']);
      });
    }
    else {
      console.log("Error");
    }
  }
}
