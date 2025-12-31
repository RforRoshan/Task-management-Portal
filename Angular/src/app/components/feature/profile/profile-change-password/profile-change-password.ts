import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { ApiResult, ChangePasswardRequestModel, ChangePasswardResponseModel, RequestObject } from '../../../../models/common-model';
import { ActionConstants, AppRouteConstants, ControllerConstants } from '../../../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EncryptionService } from '../../../../services/encryption-service';

@Component({
  selector: 'rks-profile-change-password',
  imports: [CommonModule, FormsModule],
  templateUrl: './profile-change-password.html',
  styleUrl: './profile-change-password.scss'
})
export class ProfileChangePassword {
  isLoading:boolean = false;
  projectname: string = '';

  oldPassword: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  showOld = false;
  showNew = false;

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private encryptionService: EncryptionService) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  ngOnInit():void {
    
  }


  changePassword():void {
    this.isLoading = true;

    let requestObject: RequestObject<ChangePasswardRequestModel> = new RequestObject<ChangePasswardRequestModel>();
    let requestModel: ChangePasswardRequestModel = new ChangePasswardRequestModel();
    
    requestModel.oldPassword = this.encryptionService.encrypt(this.oldPassword);
    requestModel.newPassword =  this.encryptionService.encrypt(this.newPassword);
    requestModel.source = 'Web';

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.ChangePassward;
    requestObject.requestModel = requestModel;

    //console.log(requestObject);

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<ChangePasswardResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.isLoading = false;

          this.alertService.showSuccess('Password has been successfully change, please login again.');
          //console.log(responseModel.responseData);
          this.router.navigateByUrl(AppRouteConstants.Login);
        }
        else {
          this.isLoading = false;
          this.alertService.showError(response.message);

          this.oldPassword = '';
          this.newPassword = '';
          this.confirmPassword = '';

        }
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  }
}
