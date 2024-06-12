import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { SubjectService } from '../_services/subject.service';
import { Subject } from '../_models/subject.model';
import { AddSubjectRequest } from '../_models/add-subject-request.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-subjects',
  templateUrl: './subjects.component.html',
  styleUrls: ['./subjects.component.css']
})
export class SubjectsComponent implements OnInit {
  availableSubjects: Subject[] | undefined;
  showAddSubjectForm = false; // Bazowo formularz jest schowany
  model: AddSubjectRequest;
  newSubjectName: string = '';
  newSubjectDescription: string = '';

  constructor(private http: HttpClient, private subjectService: SubjectService, private toastr: ToastrService) {
    this.model = {
      subjectName: '',
      subjectDescription: ''
    };
  }

  ngOnInit(): void {
    this.loadAvailableSubjects();
  }

  loadAvailableSubjects() {
    this.subjectService.getSubjects().subscribe(subjects => {
      this.availableSubjects = subjects;
    });
  }

  toggleAddSubjectForm() {
    this.showAddSubjectForm = !this.showAddSubjectForm; 
    // Czyszczenie pól formularza po schowaniu:
    if (!this.showAddSubjectForm) { 
      this.newSubjectName = '';
      this.newSubjectDescription = '';
    }
  }

  addSubject() {
    const newSubject: AddSubjectRequest = {
      subjectName: this.newSubjectName,
      subjectDescription: this.newSubjectDescription
    };
    this.subjectService.postSubjects(newSubject).subscribe({
      next: () => {
        this.toastr.success("Przedmiot został dodany", "Udało się!");
        this.loadAvailableSubjects(); 
        this.toggleAddSubjectForm(); 
      },
      error: error => console.error('Wystąpił błąd podczas dodawania przedmiotu:', error)
    });
  }
}
