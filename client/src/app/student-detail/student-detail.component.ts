import { Component, OnInit } from '@angular/core';
import { Student } from '../_models/get-all-students-request.motel';
import { StudentService } from '../_services/student.service';
import { ActivatedRoute } from '@angular/router';
import { SubjectService } from '../_services/subject.service';
import { EnrollStudentRequest } from '../_models/enroll-student-request.model';
import { Subject } from '../_models/subject.model';
import { StudentCourse } from '../_models/studentCourse.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-student-detail',
  templateUrl: './student-detail.component.html',
  styleUrls: ['./student-detail.component.css']
})
export class StudentDetailComponent implements OnInit {
  student: Student | undefined;
  availableSubjects: Subject[] | undefined;
  studentCourses: StudentCourse[] | undefined;
  selectedSubjectName: string = '';
  enrollRequest: EnrollStudentRequest = { studentEmail: '', subjectName: '' };


  constructor(private studentService: StudentService, private subjectService: SubjectService,
              private route: ActivatedRoute, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadStudent();
    this.loadAvailableSubjects();
    this.loadStudentCourses();
    
  }

  loadStudent() {
    const email = this.route.snapshot.paramMap.get('email')
    if (!email) return;

    this.studentService.getStudentByEmail(email).subscribe({
      next: student => {
        this.student = student;
        this.enrollRequest.studentEmail = student.email; 
      },
      error: error => console.log('Error loading student:', error)
    });

  }
  loadAvailableSubjects() {
    this.subjectService.getSubjects().subscribe(subjects => {
      this.availableSubjects = subjects;
    });
  }
  enrollStudent() {
    this.enrollRequest.subjectName = this.selectedSubjectName;
    this.studentService.enrollStudentInSubject(this.enrollRequest).subscribe({
      next: () => {
        this.toastr.success("student został zapisany!", "Udało się!")
        this.loadStudent();
        this.loadAvailableSubjects();
        this.loadStudentCourses();
      },
      error: error => console.log('Error enrolling student:', error)
    });
  }
  loadStudentCourses(){
    const email = this.route.snapshot.paramMap.get('email')
    if(!email) return;
    this.studentService.getStudentCourses(email).subscribe({
      next: courses => this.studentCourses = courses,
      error: error => console.log(error)
    });
  }
}




