import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActionConstants, AppRouteConstants, ControllerConstants } from '../../../../constants/constants';
import { Router } from '@angular/router';
import { ApiResult, ForgotPasswardLinkGenerateRequestModel, ForgotPasswardLinkGenerateResponseModel, RequestObject } from '../../../../models/common-model';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';

@Component({
  selector: 'rks-forgot-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss'
})
export class ForgotPassword {

  loginPath = AppRouteConstants.Login;
  currentYear: number = new Date().getFullYear();

  emailId: string = '';
  isLoading: boolean = false;
  showError: boolean = false;
  errorMessage: string = '';
  showSuccess: boolean = false;

  constructor(private router: Router, private httpService: HttpService, private alertService: AlertService) {}

  sendResetLink() {
    this.isLoading = true;
    this.showError = false;
    this.showSuccess = false;

    let requestObject: RequestObject<ForgotPasswardLinkGenerateRequestModel> = new RequestObject<ForgotPasswardLinkGenerateRequestModel>();
    let requestModel: ForgotPasswardLinkGenerateRequestModel = new ForgotPasswardLinkGenerateRequestModel();
    requestModel.emailId = this.emailId;
    requestModel.linkPrefix = window.location.origin + AppRouteConstants.ForgotPassword;

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.ForgotPasswardLinkGenerate;
    requestObject.requestModel = requestModel;

    this.httpService.postWithOutHeader(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<ForgotPasswardLinkGenerateResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {  
          this.isLoading = false;
          this.showSuccess = true;
          this.alertService.showSuccess('If the email is registered, a reset link has been sent. Please check your inbox.');
          this.router.navigateByUrl(AppRouteConstants.Login);
        }
        else {
          this.showError = true;
          this.isLoading = false;
          this.errorMessage = responseModel.message;
        }
      },
      error: (error) => {
        this.isLoading = false; 
        this.showError = true;
        this.errorMessage = 'An error occurred during processing. Please try again later.';
        console.error('ForgotPassword error:', error);
      }
    });
  }

  goToLogin() {
    this.router.navigateByUrl(AppRouteConstants.Login);
  }
}