import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResult, RequestObject } from '../models/common-model';
import { catchError, Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from './alert-service';
import { SessionStorageKeyConstants } from '../constants/constants';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private baseUrl = 'https://localhost:7114/api/';


  constructor(private http: HttpClient, private router: Router, private alertService: AlertService) { }


  private getAuthOptions(): { headers: HttpHeaders } {
    const token = localStorage.getItem(SessionStorageKeyConstants.Auth) || '';
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
    return { headers };
  }


  post(requestObject: RequestObject<any>): Observable<ApiResult<any>> {
    let url = this.baseUrl + requestObject.controller + '/' + requestObject.action;
    return this.http.post<ApiResult<any>>(url, requestObject.requestModel, this.getAuthOptions()).pipe(
        catchError(error => this.handleError(error, requestObject.action)) 
      );
  }

  postWithOutHeader(requestObject: RequestObject<any>): Observable<ApiResult<any>> {
    let url = this.baseUrl + requestObject.controller + '/' + requestObject.action;
    return this.http.post<ApiResult<any>>(url, requestObject.requestModel);
  }

  private handleError(error: Response | HttpErrorResponse, action:string) {
    //localStorage.setItem(SessionStorageKeyConstants.PreviousUrl, this.router.url);
    console.log("==============================================================");
    console.error(`Error == ${error}`);
    if (error instanceof HttpErrorResponse && error.status === 401) {
      console.log('Token expired or unauthorized access');
      this.alertService.showTokenError();
    } 
    else {
      console.log('Something went wrong. unknown error');
      this.alertService.showError('Something went wrong. Please login again!');
    }
    console.log(`Current action ${action}`);
    console.log('Navigating to login page...');
    this.router.navigateByUrl("/login");
    console.log("==============================================================");

    return throwError(() => new Error('Something went wrong. Please try again later.'));
  }

}
