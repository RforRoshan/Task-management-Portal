import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../../constants/constants';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { EncryptionService } from '../../../../services/encryption-service';
import { ApiResult, FirstTimeUserRequestModel, FirstTimeUserResponseModel, RequestObject } from '../../../../models/common-model';

@Component({
  selector: 'rks-first-time-user',
  imports: [CommonModule, FormsModule],
  templateUrl: './first-time-user.html',
  styleUrl: './first-time-user.scss'
})
export class FirstTimeUser {
  newPassword: string = '';
  confirmPassword: string = '';
  showNewPassword: boolean = false;
  token:string='';
  currentYear: number = new Date().getFullYear();

  isLoading = false;
  errorMessage = '';

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private encryptionService: EncryptionService) {
    let token = this.route.snapshot.paramMap.get('token');
    this.token = token!;
  }

  changePassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    let requestObject: RequestObject<FirstTimeUserRequestModel> = new RequestObject<FirstTimeUserRequestModel>();
    let requestModel: FirstTimeUserRequestModel = new FirstTimeUserRequestModel();
    requestModel.password = this.encryptionService.encrypt(this.newPassword);
    requestModel.token = this.token;
    requestModel.source = 'Web';

    //console.log(requestModel.token);

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.FirstTimeUser;
    requestObject.requestModel = requestModel;

    this.httpService.postWithOutHeader(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<FirstTimeUserResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {  

          localStorage.setItem(SessionStorageKeyConstants.Auth, responseModel.responseData.token);  
          this.isLoading = false;
          this.alertService.showSuccess('Your password has been change successfully.');
          this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
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
      }
    });

  }
}
