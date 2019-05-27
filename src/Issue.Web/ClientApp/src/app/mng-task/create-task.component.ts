import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TaskModel } from '../models/TaskModel';
import { TaskService } from '../services/task.service';
import { Router, ActivatedRoute, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-create-task',
  templateUrl: './create-task.component.html',
  styleUrls: ['./create-task.component.css']
})
export class CreateTaskComponent implements OnInit {

  taskGroup: FormGroup;
  model: TaskModel;
  projectId: number;

  constructor(private taskService: TaskService, private router: Router, private formBuilder: FormBuilder, private actRoute: ActivatedRoute) {
    this.taskGroup = this.formBuilder.group({
      title: ["", [Validators.required, Validators.maxLength(50)]],
      description: ["", [Validators.required, Validators.maxLength(4000)]]
    });
  }

  ngOnInit() {
    this.actRoute.queryParams.subscribe(params => {
      console.log(params);
      if (params.projectId) {
        this.projectId = parseInt(params.projectId);
        this.createTask();
      }
      else
        console.log("Model State is not valid");
    });
  }

  createTask() {

  }



  submit({ value, valid }: { value: TaskModel, valid: boolean }) {
    console.log(valid);
    if (valid) {
      console.log(value);
      this.taskService.createTask(this.projectId, value).subscribe(response => {
        console.log("aaaaaa" + value);
        this.createTask();
        let navigationExtras: NavigationExtras = {
          queryParams: { projectId: this.projectId }
        }
        this.router.navigate(['./list-task'], navigationExtras);
      });
    }

    else {
      console.log("Model State is not valid");
    }
  }

}
