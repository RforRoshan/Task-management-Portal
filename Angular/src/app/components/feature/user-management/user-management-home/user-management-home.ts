import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRouteConstants, SessionStorageKeyConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-user-management-home',
  imports: [],
  templateUrl: './user-management-home.html',
  styleUrl: './user-management-home.scss'
})
export class UserManagementHome {
  projectname: string = '';
  navItems:{ label: string, route: string, className: string }[]= [];
  
  constructor(private router: Router, private route: ActivatedRoute) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
    if(!['101', '102', '103'].includes(localStorage.getItem(SessionStorageKeyConstants.RoleId) ?? '')){
      this.router.navigateByUrl(AppRouteConstants.AccessDenied);
    }
  }
  
  
  ngOnInit():void {
    this.getData();
  }

  getData():void {
      this.navItems.push(
        { label: 'Register New User', route: AppRouteConstants.UserManagement.RegisterUser(this.projectname), className: 'bi bi-person-plus display-1 text-primary' },
        { label: 'Edit User Access', route: AppRouteConstants.UserManagement.UserAccess(this.projectname), className: 'bi bi-person-gear display-1 text-warning' }
      );
  }

  goTo(path: string): void {
    this.router.navigateByUrl(path);
  }
}
