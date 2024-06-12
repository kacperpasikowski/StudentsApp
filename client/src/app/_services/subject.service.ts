import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from '../_models/subject.model';
import { AddSubjectRequest } from '../_models/add-subject-request.model';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  baseUrl = 'https://localhost:5001/';

  constructor(private http: HttpClient) { }

  getSubjects(): Observable<Subject[]>{
    return this.http.get<Subject[]>(this.baseUrl + 'api/student/subject');
  }
  postSubjects(model: AddSubjectRequest){
    return this.http.post(this.baseUrl + 'api/student/subject', model)
  }

}
