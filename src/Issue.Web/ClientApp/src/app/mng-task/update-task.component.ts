import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TaskModel } from '../models/TaskModel';
import { TaskService } from '../services/task.service';
import { ActivatedRoute, Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-update-task',
  templateUrl: './update-task.component.html',
  styleUrls: ['./update-task.component.css']
})
export class UpdateTaskComponent implements OnInit {
  currentId?: number;
  taskGroup: FormGroup;
  model: TaskModel;
  projectId?: number;

  constructor(private taskService: TaskService, private actRoute: ActivatedRoute, private router: Router, private formBuilder: FormBuilder) {
    this.taskGroup = this.formBuilder.group({
      id: [""],
      title: ["", [Validators.required, Validators.maxLength(50)]],
      description: ["", [Validators.required, Validators.maxLength(4000)]]
    });
  }

  ngOnInit() {
    this.actRoute.params.subscribe(params => {
      if (params.id) {
        this.currentId = parseInt(params.id);
      }
    });
    this.actRoute.queryParams.subscribe(params => {
      if (params.projectId) {
        this.projectId = parseInt(params.projectId);
      }
    });
    this.getTask();
  }

  getTask() {
    if (this.currentId) {
      this.taskService.getTaskById(this.currentId).subscribe(response => {
        console.log(response);
        this.taskGroup.patchValue({
          id: response.id,
          title: response.title,
          description: response.description
        });
      });
    }
    else {
      console.log("Nuk Vjen Tasku");
    }
  }

  submit({ value, valid }: { value: TaskModel, valid: boolean }) {
    if (valid) {
      this.taskService.updateTask(this.projectId, value).subscribe(response => {
        let navigationExtras: NavigationExtras = {
          queryParams: { projectId: this.projectId }
        }
        this.router.navigate(['./list-task'], navigationExtras);
      });
    }
    else {
      console.log("Model State is not Valid");
    }
  }

}
