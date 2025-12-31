import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule, NgModel } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../../constants/constants.js';
import { ApiResult, LoginRequestModel, LoginResponseModel, RequestObject } from '../../../../models/common-model.js';
import { EncryptionService } from '../../../../services/encryption-service.js';
import { HttpService } from '../../../../services/http-service.js';

@Component({
  selector: 'rks-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  @ViewChild('passwordRef') passwordField!: NgModel;
  @ViewChild('emailRef') emailField!: NgModel;

  emailId: string = '';
  password: string = '';

  isLoading:boolean = false;
  currentYear: number = new Date().getFullYear();
  showError:boolean = false;
  errorMessage: string = '';

  constructor(private router: Router, private encryptionService: EncryptionService, private httpService: HttpService) { 
    //localStorage.removeItem(SessionStorageKeyConstants.Auth);
  }


  login():void {
    this.isLoading = true;

    let requestObject: RequestObject<LoginRequestModel> = new RequestObject<LoginRequestModel>();
    let requestModel: LoginRequestModel = new LoginRequestModel();
    requestModel.emailId = this.emailId;
    requestModel.password = this.encryptionService.encrypt(this.password);

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.Login;
    requestObject.requestModel = requestModel;

    this.httpService.postWithOutHeader(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<LoginResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.isLoading = false;
          if(responseModel.responseData.isFirstTimeLogin){
            this.router.navigateByUrl(AppRouteConstants.FirstTimeUser(responseModel.responseData.token));
          }
          else{
            localStorage.setItem(SessionStorageKeyConstants.Auth, responseModel.responseData.token);
            this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
          }
        }
        else {
          this.emailId = '';
          this.password = '';
          this.passwordField.control.markAsUntouched();
          this.emailField.control.markAsUntouched();

          this.showError = true;
          this.isLoading = false;
          this.errorMessage = responseModel.message;
        }
      },
      error: (error) => {
        this.isLoading = false; 
        this.showError = true;
      }
    });

  }

  goToSelectDashboard() {
    this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
  }
  getForgotPassword():string {
    return AppRouteConstants.ForgotPassword;
  }
}


