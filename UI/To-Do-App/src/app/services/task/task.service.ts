import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Task } from '../../models/Task';
import { environment } from '../../../environments/environment';
import { ApiService } from '../api/api.service';

export const TaskApiUrls = {
  getAllTasks: 'Item',
  createTask: 'Item',
  deleteTask: 'Item?idRequest=',
  updateTask: 'Item',
  getActiveTasks: 'Item/active-items',
  getCompletedTasks: 'Item/completed-items',
  deleteAllTasks: 'Item/all',
  getCompletionPercentage: 'Item/completion-percentage',
  makeCompleted: 'Item/completed',
  makeActive: 'Item/active',
  pendingTasks:'Item/pending-items',
  notifyTasks:'Item/notify-items',
  notifyFurtherTasks:'Item/notify-further-items',
  cancelNotifications:'Item/cancel-notifications',
  modifyNotificationStatus:'Item/modify-notification-status'
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
  notificationMessage$:Subject<string[]>;
  notifications$:Subject<Task[]>;
  constructor(private httpClient: HttpClient) {
    super(httpClient);
    this.editTask$ = new Subject<Task>();
    this.isLoading$ = new Subject<boolean>();
    this.deleteConfirm$ = new Subject<number>();
    this.pageManiulated$ = new Subject<string>();
    this.notificationMessage$=new Subject<string[]>();
    this.notifications$=new Subject<Task[]>();
    this.apiUrl = environment.apiUrl;
  }
  getAllTasks<T>(): Observable<T> {
    let url = this.apiUrl + TaskApiUrls.getAllTasks;
    return this.get<T>(url);
  }
  createTask<T>(t: Task): Observable<T> {
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
  getPendingTasks<T>(property:string,order:string): Observable<T> {
    const params = new HttpParams()
      .set('property', property)
      .set('order', order)
    let url = this.apiUrl + TaskApiUrls.pendingTasks;
    return this.get<T>(url,params);
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
  getNotifyTasks<T>():Observable<T>{
    let url = this.apiUrl + TaskApiUrls.notifyTasks;
    return this.get<T>(url);
  }
  getFurtherNotifyTasks<T>():Observable<T>{
    let url = this.apiUrl + TaskApiUrls.notifyFurtherTasks;
    return this.get<T>(url);
  }
  cancelNotifications<T>(ids:number[]):Observable<T>{
    let url=this.apiUrl+TaskApiUrls.cancelNotifications;
    return this.post<T>(url,ids);
  }
  modifyNotificationStatus<T>():Observable<T>{
    let url=this.apiUrl+TaskApiUrls.modifyNotificationStatus;
    return this.put<T>(url);
  }
}
