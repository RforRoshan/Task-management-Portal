import { Component } from '@angular/core';
import { AppRouteConstants, SessionStorageKeyConstants } from '../../../constants/constants';
import { ActivatedRoute, Router } from '@angular/router';
import { UpperCasePipe } from '@angular/common';

@Component({
  selector: 'rks-project-home',
  imports: [],
  templateUrl: './project-home.html',
  styleUrl: './project-home.scss'
})
export class ProjectHome {
  isFullyLoaded:boolean = false;
  projectname: string = '';
  fullName: string = '';
  navItems:{ label: string, route: string, imagePath: string }[]= [];

   constructor(private router: Router, private route: ActivatedRoute) {
    const projectname = this.route.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  ngOnInit():void {
    this.getData();
  }

  getData():void {
      if(localStorage.getItem(SessionStorageKeyConstants.MultiProjectAccessTo) == '1'){
        this.navItems.push({ label: `Change Project`, route: AppRouteConstants.SelectDashboard, imagePath: '/images/dashboard-home/project.jpg' });
      }
      this.navItems.push(
        { label: 'Current Task', route: AppRouteConstants.Task.CurrentTask(this.projectname), imagePath: '/images/dashboard-home/current.jpg' },
        { label: 'Done Task', route: AppRouteConstants.Task.DoneTask(this.projectname), imagePath: '/images/dashboard-home/done.jpg' },
        { label: 'Task Setup', route: AppRouteConstants.Task.TaskSetup(this.projectname), imagePath: '/images/dashboard-home/setup.jpg' },
        { label: 'UseFull Links', route: AppRouteConstants.UseFullLinks(this.projectname), imagePath: '/images/dashboard-home/usefull-link.png' },
        { label: 'RN / Feature', route: AppRouteConstants.RNFeature.Root(this.projectname), imagePath: '/images/dashboard-home/rn.jpg' }
      );
      let roleId : string = localStorage.getItem(SessionStorageKeyConstants.RoleId) || '';

      if(['101', '102', '103'].includes(roleId)){
        this.navItems.push( { label: 'User Management', route: AppRouteConstants.UserManagement.Root(this.projectname), imagePath: '/images/dashboard-home/user.jpg' });
      }
      this.navItems.push( { label: 'My Profile', route: AppRouteConstants.MyProfile.Root(this.projectname), imagePath: '/images/dashboard-home/my-profile.jpg' });
      this.fullName = localStorage.getItem(SessionStorageKeyConstants.FullName) || '';
      this.isFullyLoaded = true;
  }

  goTo(path: string): void {
    this.router.navigateByUrl(path);
  }

}
