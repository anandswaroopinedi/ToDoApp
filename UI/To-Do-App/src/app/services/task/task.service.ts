import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Task } from '../../models/Task';
import { environment } from '../../../environments/environment';
import { ApiService } from '../api/api.service';

export const TaskApiUrls = {
  getAllTasks: 'Item/all',
  createTask: 'Item/create',
  deleteTask: 'Item/delete?id=',
  updateTask: 'Item/update',
  getActiveTasks: 'Item/active-items',
  getCompletedTasks: 'Item/completed-items',
  deleteAllTasks: 'Item/delete-all',
  getCompletionPercentage: 'Item/completion-percentage',
  makeCompleted: 'Item/completed',
  makeActive: 'Item/active',
};

@Injectable({
  providedIn: 'root',
})
export class TaskService extends ApiService {
  private apiUrl: string;
  pageManiulated$: Subject<string>;
  editTask$: Subject<Task>;
  isLoading$: Subject<boolean>;
  deleteConfirm$: Subject<number>;
  constructor(private httpClient: HttpClient) {
    super(httpClient);
    this.editTask$ = new Subject<Task>();
    this.isLoading$ = new Subject<boolean>();
    this.deleteConfirm$ = new Subject<number>();
    this.pageManiulated$ = new Subject<string>();
    this.apiUrl = environment.apiUrl;
  }
  getAllTasks<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.getAllTasks;
    return this.get<T>(url);
  }
  createTask<T>(t: Task): Observable<T> {
    debugger;
    let url = this.apiUrl + TaskApiUrls.createTask;
    return this.post<T>(url, t);
  }
  updateTask<T>(t: Task): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.updateTask;
    return this.put<T>(url, t);
  }
  deleteTask<T>(data?: any): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.deleteTask;
    const deleteUrl = data ? url + data : url;
    return this.delete<T>(deleteUrl);
  }
  getActiveTasks<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.getActiveTasks;
    return this.get<T>(url);
  }
  getCompletedTasks<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.getCompletedTasks;
    return this.get<T>(url);
  }
  deleteAllTasks<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.deleteAllTasks;
    return this.delete<T>(url);
  }
  getCompletionpercentage<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.getCompletionPercentage;
    return this.get<T>(url);
  }
  makeTaskAsCompleted<T>(id: number): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.makeCompleted;
    return this.post<T>(url, id);
  }
  makeTaskAsActive<T>(id: number): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.makeActive;
    return this.post<T>(url, id);
  }
}
