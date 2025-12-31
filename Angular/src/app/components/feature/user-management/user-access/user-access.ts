import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlterUserAccessRequestModel, AlterUserAccessResponseModel, ApiResult, GetAllRoleByAccessRequestModel, GetAllRoleByAccessResponseModel, GetAllUserModel, GetAllUserRequestModel, GetAllUserResponseModel, GetProjectAccessByIdRequestModel, GetProjectAccessByIdResponseModel, GetProjectAccessRequestModel, GetProjectAccessResponseModel, ProjectAccessView, RequestObject, RoleModel, UserAccessModel } from '../../../../models/common-model';

@Component({
  selector: 'rks-user-access',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-access.html',
  styleUrl: './user-access.scss'
})
export class UserAccess {

  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  projectname: string = '';
  projectAccessListFresh:ProjectAccessView[]= [];
  projectAccessList:ProjectAccessView[]= [];
  roles: RoleModel[] = [];
  userIdNames: GetAllUserModel[] = [];

  userId:number = 0;
  showProject:boolean = false;

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
          this.projectAccessListFresh = (responseModel.responseData.projectAccess ?? []).map(u => 
            Object.assign(new ProjectAccessView(), {
              projectId: u.projectId,
              projectKey: u.projectKey,
              projectName: u.projectName,
              roleId: this.roles[this.roles.length - 1].roleId
            })
          );
          this.getAllUserData();
          //console.log(this.projectAccessListFresh);
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

  getAllUserData():void {
    let requestObject: RequestObject<GetAllUserRequestModel> = new RequestObject<GetAllUserRequestModel>();
    let requestModel: GetAllUserRequestModel = new GetAllUserRequestModel();

    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.GetAllUser;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllUserResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.userIdNames = responseModel.responseData.userIdNames;
          this.isFullyLoaded = true;
          //console.log(this.userIdNames);
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

  getUserAccess():void{
    let requestObject: RequestObject<GetProjectAccessByIdRequestModel> = new RequestObject<GetProjectAccessByIdRequestModel>();
    let requestModel: GetProjectAccessByIdRequestModel = new GetProjectAccessByIdRequestModel();

    requestModel.userId = this.userId;

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.GetProjectAccessById;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetProjectAccessByIdResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.projectAccessList = (this.projectAccessListFresh ?? []).map(element => {
            const proj = responseModel.responseData.projectAndRoles.find(p => p.projectId === element.projectId);

            // Update properties if matched
            const updatedRoleId = proj ? proj.roleId : element.roleId;
            const isSelected = !!proj;

            // Return new object
            return Object.assign(new ProjectAccessView(), {
              projectId: element.projectId,
              projectKey: element.projectKey,
              projectName: element.projectName,
              roleId: updatedRoleId,
              isSelected: isSelected
            });
          });

          this.showProject = true;
          this.isLoading = false;
          //console.log(responseModel);
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

  updateAccess():void{

    let userName:string = this.userIdNames.find(u=> u.userId === this.userId)?.userName ?? '';
    this.alertService.showConfirmation(`Are you sure, want to alter user access of ${userName}`, 'Yes, sure!').then((confirmed) => {
      if (confirmed) { 

        this.isLoading = true;
        let requestObject: RequestObject<AlterUserAccessRequestModel> = new RequestObject<AlterUserAccessRequestModel>();
        let requestModel: AlterUserAccessRequestModel = new AlterUserAccessRequestModel();
        
        requestModel.userId = this.userId;
        this.projectAccessList.forEach(element => {
          
          if(element.isSelected){
            requestModel.userAccess.push(
            Object.assign(new UserAccessModel(), {
              projectId: element.projectId,
              projectKey: element.projectKey,
              roleId: element.roleId,
              acessHave: true
            }));
          }
          else{
            requestModel.userAccess.push(
            Object.assign(new UserAccessModel(), {
              projectId: element.projectId,
              projectKey: element.projectKey,
              roleId: 0,
              acessHave: false
            }));
          }

        });

        requestObject.controller = ControllerConstants.User;
        requestObject.action = ActionConstants.AlterUserAccess;
        requestObject.requestModel = requestModel;
        //console.log(requestObject);

        this.httpService.post(requestObject).subscribe({
          next: (response) => {
            let responseModel: ApiResult<AlterUserAccessResponseModel> = response;
            if(responseModel.status  && responseModel.responseData != null) {       
              this.isLoading = false;
              this.userId = 0;
              this.showProject = false;
              this.alertService.showSuccess(`${userName} user access had been alter successfully.`);
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
    });

    
  }
}
