import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { AccessRegisterModel, ApiResult, GetAllRoleByAccessRequestModel, GetAllRoleByAccessResponseModel, GetProjectAccessRequestModel, GetProjectAccessResponseModel, ProjectAccess, ProjectAccessView, RegisterRequestModel, RegisterResponseModel, RequestObject, RoleModel } from '../../../../models/common-model';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-register-user',
  imports: [CommonModule, FormsModule],
  templateUrl: './register-user.html',
  styleUrl: './register-user.scss'
})
export class RegisterUser {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  projectname: string = '';
  projectAccessList:ProjectAccessView[]= [];
  roles: RoleModel[] = [];

  user = {firstName: '',
  middleName: '',
  lastName: '',
  employeeNumber:  0,
  email: '',
  roleId: 0
  };

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
    if(!['101', '102', '103'].includes(localStorage.getItem(SessionStorageKeyConstants.RoleId) ?? '')){
      this.router.navigateByUrl(AppRouteConstants.AccessDenied);
    }
  }
  
  ngOnInit():void {
    this.getRoleData();
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
          this.projectAccessList = (responseModel.responseData.projectAccess ?? []).map(u => 
            Object.assign(new ProjectAccessView(), {
              projectId: u.projectId,
              projectKey: u.projectKey,
              projectName: u.projectName,
              roleId: this.roles[this.roles.length - 1].roleId
            })
          );
          this.isFullyLoaded = true;
          //console.log(this.projectAccessList);
        }
        else {
          this.alertService.showError(response.message);
          this.isFullyLoaded = true;
        }
      },
      error: (error) => {
        this.isFullyLoaded = true;
      }
    });
  
  }

  getRoleData():void{
    let requestObject: RequestObject<GetAllRoleByAccessRequestModel> = new RequestObject<GetAllRoleByAccessRequestModel>();
    let requestModel: GetAllRoleByAccessRequestModel = new GetAllRoleByAccessRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.GetAllRoleByAccess;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllRoleByAccessResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.roles = responseModel.responseData.roles;
          this.getData()
          //console.log(this.roles);
        }
        else {
          this.alertService.showError(response.message);
          this.isFullyLoaded = true;
        }
      },
      error: (error) => {
        this.isFullyLoaded = true;
      }
    });
    
  }

  register():void {
    this.isLoading = true;

    let requestObject: RequestObject<RegisterRequestModel> = new RequestObject<RegisterRequestModel>();
    let requestModel: RegisterRequestModel = new RegisterRequestModel();
    
    requestModel.firstName = this.user.firstName;
    requestModel.middleName = this.user.middleName;
    requestModel.lastName = this.user.lastName;
    requestModel.employeeNumber = this.user.employeeNumber;
    requestModel.email = this.user.email;
    requestModel.username = this.user.email;

    (this.projectAccessList ?? []).map(u=>{
      if(u.isSelected){
        let accessRegisterModel:AccessRegisterModel = new AccessRegisterModel();
        accessRegisterModel.projectId = u.projectId;
        accessRegisterModel.roleId = u.roleId
        requestModel.accessDetails.push(accessRegisterModel);
      }
    });
    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.Register;
    requestObject.requestModel = requestModel;

    //console.log(requestObject);

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<RegisterResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.isLoading = false;
          this.alertService.showSuccess(responseModel.responseData.message);
          //console.log(responseModel.responseData);
          this.router.navigateByUrl(AppRouteConstants.UserManagement.Root(this.projectname));
        }
        else {
          this.alertService.showError(response.message);
          this.isLoading = false;
        }
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  }
}
