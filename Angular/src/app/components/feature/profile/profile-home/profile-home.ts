import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRouteConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-profile-home',
  imports: [],
  templateUrl: './profile-home.html',
  styleUrl: './profile-home.scss'
})
export class ProfileHome {
  projectname: string = '';
  navItems:{ label: string, route: string, className: string }[]= [];
  
  constructor(private router: Router, private route: ActivatedRoute) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  
  ngOnInit():void {
    this.getData();
  }

  getData():void {
      this.navItems.push(
        { label: 'Change Password', route: AppRouteConstants.MyProfile.ChangePassword(this.projectname), className: 'bi bi-key display-1 text-primary' },
        { label: 'User Details', route: AppRouteConstants.MyProfile.PersonalDetails(this.projectname), className: 'bi bi-clipboard-data display-1 text-warning' }
      );
  }

  goTo(path: string): void {
    this.router.navigateByUrl(path);
  }
}
