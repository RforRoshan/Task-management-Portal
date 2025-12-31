import { Component } from '@angular/core';
import { ApiResult, GetProjectAccessRequestModel, GetProjectAccessResponseModel, RequestObject } from '../../../../models/common-model';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActionConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-personal-details',
  imports: [CommonModule, FormsModule],
  templateUrl: './personal-details.html',
  styleUrl: './personal-details.scss'
})
export class PersonalDetails {
  isLoading:boolean = true;
  projectname: string = '';
  projectAccessTo:string[]=[];

  username: string = localStorage.getItem(SessionStorageKeyConstants.Username) ?? '';
  firstName: string =  localStorage.getItem(SessionStorageKeyConstants.FirstName) ?? '';
  middleName: string = localStorage.getItem(SessionStorageKeyConstants.MiddleName) ?? '';
  lastName: string = localStorage.getItem(SessionStorageKeyConstants.LastName) ?? '';
  themeName: string = localStorage.getItem(SessionStorageKeyConstants.ThemeName) ?? 'Ocean';
  
  constructor(private httpService: HttpService, private route: ActivatedRoute, private alertService: AlertService) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
    
  }

ngOnInit():void {
    this.getData();
  }

  getData():void {
  
    let requestObject: RequestObject<GetProjectAccessRequestModel> = new RequestObject<GetProjectAccessRequestModel>();
      let requestModel: GetProjectAccessRequestModel = new GetProjectAccessRequestModel();
  
      requestObject.controller = ControllerConstants.User;
      requestObject.action = ActionConstants.GetProjectAccess;
      requestObject.requestModel = requestModel;
  
      this.httpService.post(requestObject).subscribe({
        next: (response) => {
        let responseModel: ApiResult<GetProjectAccessResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.projectAccessTo = (responseModel.responseData.projectAccess ?? []).map(u=> u.projectName);
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  
  }
  
}
