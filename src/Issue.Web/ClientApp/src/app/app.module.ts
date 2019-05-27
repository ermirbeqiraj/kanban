import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { httpInterceptorProviders } from './services/interceptors';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './account/login/login.component';
import { ListProjectComponent } from './mng-project/list-project.component';
import { CreateProjectComponent } from './mng-project/create-project.component';
import { UpdateProjectComponent } from './mng-project/update-project.component';
import { CreateUserComponent } from './account/create-user.component';
import { ListUserComponent } from './account/list-user.component';
import { UpdateUserComponent } from './account/update-user.component';
import { UpdatePasswordComponent } from './account/update-password.component';
import { ListTaskComponent } from './mng-task/list-task.component';
import { CreateTaskComponent } from './mng-task/create-task.component';
import { UpdateTaskComponent } from './mng-task/update-task.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    ListProjectComponent,
    CreateProjectComponent,
    UpdateProjectComponent,
    CreateUserComponent,
    ListUserComponent,
    UpdateUserComponent,
    UpdatePasswordComponent,
    ListTaskComponent,
    CreateTaskComponent,
    UpdateTaskComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'list-project', component: ListProjectComponent },
      { path: 'update-project/:id', component: UpdateProjectComponent },
      { path: 'create-project', component: CreateProjectComponent },
      { path: 'create-user', component: CreateUserComponent },
      { path: 'list-user', component: ListUserComponent },
      { path: 'update-user/:id', component: UpdateUserComponent },
      { path: 'update-password/:id', component: UpdatePasswordComponent },
      { path: 'list-task', component: ListTaskComponent },
      { path: 'create-task', component: CreateTaskComponent },
      { path: 'update-task/:id', component: UpdateTaskComponent }
    ])
  ],
  providers: [httpInterceptorProviders],
  bootstrap: [AppComponent]
})
export class AppModule { }
