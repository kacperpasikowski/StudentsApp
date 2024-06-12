import { Component, OnInit } from '@angular/core';
import { AddStudentRequest } from '../_models/add-student-request.model';
import { StudentService } from '../_services/student.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { passwordValidator } from '../validators/passwordValidator';

@Component({
  selector: 'app-add-student',
  templateUrl: './add-student.component.html',
  styleUrls: ['./add-student.component.css']
})
export class AddStudentComponent implements OnInit {
  model: AddStudentRequest;
  registerForm: FormGroup = new FormGroup({});
  isSubmitting = false;

  ngOnInit(): void {
    this.initializeForm();
  }

  constructor(private studentService: StudentService, private router: Router) {
    this.model = {
      firstName: '',
      lastName: '',
      email: '',
      semester: 0,
      wiek: 0,
      password: ''
    };
  }

  initializeForm() {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.email]),
      semester: new FormControl('', [Validators.min(1), Validators.max(7)]),
      wiek: new FormControl('', [Validators.min(20), Validators.max(90)]),
      password: new FormControl('', [Validators.required,
        Validators.minLength(8), Validators.maxLength(16),
        passwordValidator()
      ]),
    });
  }

  onFormSubmit() {
    this.model = { ...this.registerForm.value }; // Update model with form values
    this.studentService.addStudent(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('students');
      },
      error: error => {
        console.log(error);
        this.isSubmitting = false;
      }
    });
  }
}
