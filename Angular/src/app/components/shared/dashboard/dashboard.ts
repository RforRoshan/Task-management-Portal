import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterModule, RouterOutlet } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { ApiResult, GetDashBoardDataRequestModel, UserDetails, RequestObject } from '../../../models/common-model';
import { ActionConstants, AppRouteConstants, ControllerConstants, SessionStorageKeyConstants } from '../../../constants/constants';
import { AlertService } from '../../../services/alert-service';
import { ThemeService } from '../../../services/theme-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'rks-dashboard',
  imports: [RouterOutlet, CommonModule, RouterModule, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard {
  isLoading:boolean = true;
  projectname: string = '';
  userDetails: UserDetails;
  themeDropdownOpen = false;
  isSidebarVisible = true;
  manageDropdownOpen = false;

  profilePath: string= '';

  searchNavValue:string =''

  navItems:{ label: string, route: string, imagePath: string }[]= [];
  currentYear: number = new Date().getFullYear();


  constructor(private router: Router, private httpService: HttpService, private route: ActivatedRoute, private alertService: AlertService, private themeService:ThemeService) {
    const projectname = this.route.snapshot.data['projectname'];
    this.projectname = projectname!;
    this.userDetails = new UserDetails();
    this.profilePath = AppRouteConstants.MyProfile.Root( this.projectname);
  }

  ngOnInit():void {
    this.getData();
  }

  getData():void {
  
    let requestObject: RequestObject<GetDashBoardDataRequestModel> = new RequestObject<GetDashBoardDataRequestModel>();
    let requestModel: GetDashBoardDataRequestModel = new GetDashBoardDataRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.User;
    requestObject.action = ActionConstants.GetDashBoardData;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UserDetails> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.userDetails = responseModel.responseData;
          this.userDetails.firstName = this.userDetails.firstName.charAt(0).toUpperCase() + this.userDetails.firstName.slice(1);
          this.userDetails.lastName = this.userDetails.lastName.charAt(0).toUpperCase() + this.userDetails.lastName.slice(1);
          if(this.userDetails.middleName){
            this.userDetails.middleName = this.userDetails.middleName.charAt(0).toUpperCase() + this.userDetails.middleName.slice(1);
          }

          localStorage.setItem(SessionStorageKeyConstants.Username, this.userDetails.username);
          localStorage.setItem(SessionStorageKeyConstants.FirstName, this.userDetails.firstName);
          localStorage.setItem(SessionStorageKeyConstants.MiddleName, this.userDetails.middleName);
          localStorage.setItem(SessionStorageKeyConstants.LastName, this.userDetails.lastName); 
          localStorage.setItem(SessionStorageKeyConstants.UserId, this.userDetails.userId.toString());
          localStorage.setItem(SessionStorageKeyConstants.RoleId, this.userDetails.roleId.toString());
          localStorage.setItem(SessionStorageKeyConstants.ProjectId, this.userDetails.projectId.toString());
          localStorage.setItem(SessionStorageKeyConstants.UserRoleId, this.userDetails.userRoleId.toString());

          let fullName =  this.userDetails.firstName + (this.userDetails.middleName ? ' ' + this.userDetails.middleName : '') + ' ' + this.userDetails.lastName;
          
          localStorage.setItem(SessionStorageKeyConstants.FullName, fullName);
          
          if(localStorage.getItem(SessionStorageKeyConstants.MultiProjectAccessTo) == '1'){
            this.navItems.push(
            { label: `Change Project`, route: AppRouteConstants.SelectDashboard, imagePath: '' }
          );

          }
          this.navItems.push(
            { label: `Home`, route: AppRouteConstants.Dashboard(this.projectname), imagePath: '' },
            { label: 'Current Task', route: AppRouteConstants.Task.CurrentTask(this.projectname), imagePath: '' },
            { label: 'Done Task', route: AppRouteConstants.Task.DoneTask(this.projectname), imagePath: '' },
            { label: 'Task Setup', route: AppRouteConstants.Task.TaskSetup(this.projectname), imagePath: '' },
            { label: 'UseFull Links', route: AppRouteConstants.UseFullLinks(this.projectname), imagePath: '' },
            { label: 'RN / Feature', route: AppRouteConstants.RNFeature.Root(this.projectname), imagePath: '' }
          );

          if([101, 102, 103].includes(this.userDetails.roleId)){
            this.navItems.push( { label: 'User Management', route: AppRouteConstants.UserManagement.Root(this.projectname), imagePath: '' });
          }
          this.navItems.push( { label: 'My Profile', route: AppRouteConstants.MyProfile.Root(this.projectname), imagePath: '' })
          this.isLoading = false;
          //console.log('Data from the dashboard');
        }
        else {
          this.isLoading = false;
          this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
          this.alertService.showError(responseModel.message);
        }
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  
  }

  filternavItems() {
    if (!this.searchNavValue) return [];
    const lower = this.searchNavValue.toLowerCase();
    return this.navItems.filter(item =>
      item.label.toLowerCase().includes(lower)
    );
  }

  toggleSidebar(): void {
    this.isSidebarVisible = !this.isSidebarVisible;
  }
  
  getAllTheme(){
    return this.themeService.themes;
  } 
  
  switchTheme(themeName: string) {
    this.themeService.changeTheme(themeName);
  }

  goTo(path: string): void {
    this.searchNavValue =''
    this.router.navigateByUrl(path);
  }

  goToHome(): void {
    this.searchNavValue =''
    this.router.navigateByUrl(AppRouteConstants.Dashboard(this.projectname));
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
