import { Component } from '@angular/core';
import { ApiResult, GetProjectAccessRequestModel, GetProjectAccessResponseModel, ProjectAccess, RequestObject } from '../../../models/common-model';
import { Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../constants/constants';
import { CommonModule, UpperCasePipe } from '@angular/common';
import { ThemeService } from '../../../services/theme-service';
import { AlertService } from '../../../services/alert-service';

@Component({
  selector: 'rks-project-choice',
  imports: [UpperCasePipe, CommonModule],
  templateUrl: './project-choice.html',
  styleUrl: './project-choice.scss'
})
export class ProjectChoice {
  isFullyLoaded:boolean = false;
  projectAccess:ProjectAccess[] = []; 
  showNoProjectAccess: boolean = false;
  currentYear: number = new Date().getFullYear();
  themeDropdownOpen = false;

  constructor(private router: Router, private httpService: HttpService, private alertService: AlertService, private themeService:ThemeService) {}

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

          this.projectAccess = responseModel.responseData.projectAccess;
          if(this.projectAccess.length == 0){
            this.showNoProjectAccess = true;
          }
          else if(this.projectAccess.length == 1){
            localStorage.setItem(SessionStorageKeyConstants.MultiProjectAccessTo, '0');
            
            this.goToProject(this.projectAccess[0].projectKey);
          }
          else{
            localStorage.setItem(SessionStorageKeyConstants.MultiProjectAccessTo, '1');
          }
          //console.log(responseModel);
          this.isFullyLoaded = true;
        }
        else {
          this.isFullyLoaded = true;
        }
      },
      error: (error) => {
        this.isFullyLoaded = true;
      }
    });
  
  }

  goToProject(projectName:string){
    this.router.navigateByUrl(AppRouteConstants.Dashboard(projectName));
  }

  getAllTheme(){
    return this.themeService.themes;
  } 
  
  switchTheme(themeName: string) {
    this.themeService.changeTheme(themeName);
  }

  logout(): void {
    this.alertService.showConfirmation('Are you sure you want to logout?', 'Yes, logout!').then((confirmed) => {
      if (confirmed) {
        const pageSize = localStorage.getItem(SessionStorageKeyConstants.PageSize);
        const themeName = localStorage.getItem(SessionStorageKeyConstants.ThemeName);
        localStorage.clear();
        if (pageSize !== null) {
          localStorage.setItem(SessionStorageKeyConstants.PageSize, pageSize);
        }
        if (themeName !== null) {
          localStorage.setItem(SessionStorageKeyConstants.ThemeName, themeName);
        }
        this.router.navigateByUrl(AppRouteConstants.Login);
      }
    });
  }
}
