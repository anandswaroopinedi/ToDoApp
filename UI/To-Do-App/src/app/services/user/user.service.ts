import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../../models/User';
import { environment } from '../../../environments/environment';
import { ApiService } from '../api/api.service';

export const userApiUrls = {
  authenticateUser: 'Authentication',
  addUser: 'User/add',
  regenerateToken:'Authentication/regenerate'
};

@Injectable({
  providedIn: 'root',
})
export class UserService extends ApiService {
  private apiUrl: string;
  constructor(private client: HttpClient) {
    super(client);
    this.apiUrl = environment.apiUrl;
  }
  addUser<T>(user: User): Observable<T> {
    let url = this.apiUrl + userApiUrls.addUser;
    return this.post<T>(url, user);
  }
  authenticateUser<T>(user: User): Observable<T> {
    let url = this.apiUrl + userApiUrls.authenticateUser;
    return this.post<T>(url, user);
  }
  getTokens<T>():Observable<T>{
    let url=this.apiUrl+userApiUrls.regenerateToken;
    return this.get<T>(url);
  }
}
