import { Component, OnInit } from '@angular/core';
import { StudentService } from '../_services/student.service';
import { DomSanitizer } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { UserParams } from '../_models/user-params.model';
import { Subject } from '../_models/subject.model';
import { SubjectService } from '../_services/subject.service';
import { Student } from '../_models/get-all-students-request.motel';
import { Pagination } from '../_models/pagionation.model';

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  styleUrls: ['./students-list.component.css']
})
export class StudentsListComponent implements OnInit {

  constructor(
    private studentService: StudentService, 
    private domSanitizer: DomSanitizer, 
    private subjectService: SubjectService
  ){}

  students: Student[] = [];
  pagination: Pagination | undefined;
  availableSubjects: Subject[] = [];
  filteredStudents: Student[] = [] ;
  selectedSubjectId: string = '';
  userParams: UserParams = {
    pageNumber: 1,
    pageSize: 5
  };

  ngOnInit(): void {
    this.loadStudents();
    this.loadSubjects();
  };

  loadStudents(): void {
    this.studentService.getAllStudents(this.userParams).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.students = response.result;
          this.pagination = response.pagination;
          this.filteredStudents = this.students;
          this.loadStudentPhotos(this.students);
        }
      },
      error: error => console.log(error)
    });
  }

  loadStudentPhotos(students: Student[]): void {
    for (let student of students) {
      this.loadStudentPhoto(student.id).subscribe({
        next: photoData => {
          const imageUrl = this.domSanitizer.bypassSecurityTrustUrl(URL.createObjectURL(photoData));
          student.avatarUrl = imageUrl;
        },
        error: error => console.log('fetching error: ', error)
      });
    }
  }

  editStudent(student: Student){
    student.editing = true;
  };

  cancelEdit(student: Student){
    student.editing = false;
  };

  updateStudent(student: Student){
    this.studentService.updateStudent(student.id, student).subscribe({
      next: () => {
        console.log(`student with ID: ${student.id} has been updated`);
        student.editing = false;
      },
      error: error => console.log(error)
    });
  };

  deleteStudent(studentId: string){
    if(window.confirm('Are you sure you want to delete this student?')){
      this.studentService.deleteStudentById(studentId).subscribe({
        next: () => {
          console.log(`Student with Id ${studentId} has been deleted`);
          this.loadStudents();
        },
        error: error => console.log(error)
      });
    }
  }

  loadStudentPhoto(studentId: string): Observable<Blob> {
    return this.studentService.getStudentPhoto(studentId);
  }

  pageChanged(event: any){
    if(this.userParams && this.userParams.pageNumber !== event.page){
      this.userParams.pageNumber = event.page;
      this.loadStudents();
    }
  }

  exportCSV(){
    this.studentService.exportStudentsToCsv().subscribe(blob => {
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(blob);
      a.href = objectUrl;
      a.download = 'students.csv';
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }

  onSortChange(sortBy: string): void {
    this.userParams.sortBy = sortBy;
    this.userParams.pageNumber = 1;
    this.filteredStudents = this.students;
    this.loadStudents();
  }

  resetFilters(): void {
    this.userParams = {
      pageNumber: 1,
      pageSize: 5,
      sortBy: '',
      sortOrder: 'asc'
    };
    this.selectedSubjectId = '';
    this.filteredStudents = this.students;
    this.loadStudents();
    this.loadStudentPhotos(this.filteredStudents);
  }

  setSortOrder(event: Event): void {
    const target = event.target as HTMLSelectElement;
    const order = target.value;
    if (order) {
      this.userParams.sortOrder = order;
      this.userParams.pageNumber = 1;
      this.loadStudents();
    }
  }

  loadSubjects() {
    this.subjectService.getSubjects().subscribe(subjects => {
      this.availableSubjects = subjects;
    });
  }

  filterStudentsBySubject() {
    if (this.selectedSubjectId) {
      this.studentService.getStudentsBySubject(this.selectedSubjectId).subscribe({
        next: (students) => {
          this.filteredStudents = students;
          this.loadStudentPhotos(this.filteredStudents);
        },
        error: (error) => console.log(error)
      });
    } else {
      this.filteredStudents = this.students;
      this.loadStudentPhotos(this.filteredStudents);
    }
  }
}
