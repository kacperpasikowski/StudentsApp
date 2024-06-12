import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentsListComponent } from './students-list/students-list.component';
import { AppComponent } from './app.component';
import { AddStudentComponent } from './add-student/add-student.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { authGuard } from './_guards/auth.guard';
import { NotFoundComponent } from './not-found/not-found.component';
import { adminGuard } from './_guards/admin.guard';
import { StudentDetailComponent } from './student-detail/student-detail.component';
import { SubjectsComponent } from './subjects/subjects.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'login', component: LoginComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'register', component: RegisterComponent},
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [ 
      {path: 'admin/student/add', component:AddStudentComponent, canActivate:[adminGuard]},
      {path: 'welcome', component: WelcomeComponent},
      {path: 'students', component:StudentsListComponent, canActivate:[adminGuard] },
      {path: 'students/:email', component:StudentDetailComponent, canActivate:[adminGuard] },
      {path: 'subjects', component:SubjectsComponent, canActivate:[adminGuard] },
      {path: 'users', component:UsersComponent, canActivate:[adminGuard] },
    ]
  },
  {path: '**', component: HomeComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
