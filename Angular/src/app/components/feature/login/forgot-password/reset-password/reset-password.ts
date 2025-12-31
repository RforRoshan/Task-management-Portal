import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActionConstants, AppRouteConstants, ControllerConstants } from '../../../../../constants/constants';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../../services/http-service';
import { AlertService } from '../../../../../services/alert-service';
import { ApiResult, ForgotPasswardRequestModel, ForgotPasswardResponseModel, RequestObject } from '../../../../../models/common-model';
import { EncryptionService } from '../../../../../services/encryption-service';

@Component({
  selector: 'rks-reset-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './reset-password.html',
  styleUrl: './reset-password.scss'
})
export class ResetPassword {

  newPassword: string = '';
  confirmPassword: string = '';
  showNewPassword: boolean = false;
  showConfirmPassword: boolean = false;
  token:string='';

  isLoading = false;
  errorMessage = '';
  loginPath = AppRouteConstants.Login;

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private encryptionService: EncryptionService) {
    let token = this.route.snapshot.paramMap.get('token');
    this.token = token!;
  }

  resetPassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    let requestObject: RequestObject<ForgotPasswardRequestModel> = new RequestObject<ForgotPasswardRequestModel>();
    let requestModel: ForgotPasswardRequestModel = new ForgotPasswardRequestModel();
    requestModel.password = this.encryptionService.encrypt(this.newPassword);
    requestModel.token = this.token;
    requestModel.source = 'Web';

    //console.log(requestModel.token);

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.ForgotPasswardReset;
    requestObject.requestModel = requestModel;

    this.httpService.postWithOutHeader(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<ForgotPasswardResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {  
          this.isLoading = false;

          this.alertService.showSuccess('Your password has been reset successfully.');
          this.router.navigateByUrl(AppRouteConstants.Login);
        }
        else {
          this.isLoading = false;
          this.newPassword = '';
          this.confirmPassword = '';
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
        this.isLoading = false; 
        this.errorMessage = 'An error occurred during processing. Please try again later.';
        console.error('ResetPassword error:', error);
      }
    });

  }
}
