import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Student } from '../_models/get-all-students-request.motel';
import { AddStudentRequest } from '../_models/add-student-request.model';
import { Observable, map } from 'rxjs';
import { PaginatedResult } from '../_models/pagionation.model';
import { UserParams } from '../_models/user-params.model';
import { EnrollStudentRequest } from '../_models/enroll-student-request.model';
import { StudentCourse } from '../_models/studentCourse.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  baseUrl = 'https://localhost:5001/';


  constructor(private http: HttpClient) { }

  getAllStudents(userParams: UserParams) {
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    if (userParams.sortBy !== undefined) {
      params = params.append('sortBy', userParams.sortBy);
    }
    if (userParams.sortOrder !== undefined) {
      params = params.append('sortOrder', userParams.sortOrder);
    }

    return this.getPaginatedResults<Student[]>(this.baseUrl + 'api/student', params)
  };

  private getPaginatedResults<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        console.log('Headers:', response.headers.keys()); 
        response.headers.keys().forEach(key => {
          console.log(`${key}: ${response.headers.get(key)}`); 
        });
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        console.log('Pagination:', pagination); 
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);

    return params;
  }

  addStudent(model: AddStudentRequest) {
    return this.http.post(this.baseUrl + 'api/admin/addstudent', model)
  }
  updateStudent(studentId: string, updatedData: any) {
    return this.http.put(this.baseUrl + `api/student/${studentId}`, updatedData)
  }
  deleteStudentById(studentId: string) {
    return this.http.delete(this.baseUrl + `api/student/${studentId}`)
  }
  getStudentPhoto(studentId: string): Observable<any> {
    return this.http.get(this.baseUrl + `api/student/${studentId}/photo`, { responseType: 'blob' });
  }
  exportStudentsToCsv(){
    return this.http.get(this.baseUrl +'api/student/export', {responseType: 'blob'});
  }
  getStudentByEmail(studentemail: string): Observable<Student>{
    return this.http.get<Student>(this.baseUrl + `api/student/${studentemail}`)
  }
  enrollStudentInSubject(enrollRequest: EnrollStudentRequest) : Observable<StudentCourse>{
    return this.http.post<StudentCourse>(this.baseUrl + `api/student/enroll`, enrollRequest)
  }
  getStudentCourses(email: string): Observable<StudentCourse[]>{
    return this.http.get<StudentCourse[]>(this.baseUrl + `api/student/${email}/courses`)
  }
  getStudentsBySubject(subjectId: string): Observable<Student[]> {
    return this.http.get<Student[]>(this.baseUrl + `api/student/subject/${subjectId}/students`);
  }

}
