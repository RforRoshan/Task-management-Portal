import { Component } from '@angular/core';
import { AppRouteConstants } from '../../../../constants/constants';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'rks-task-setup-home',
  imports: [],
  templateUrl: './task-setup-home.html',
  styleUrl: './task-setup-home.scss'
})
export class TaskSetupHome {
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
        { label: 'Network', route: AppRouteConstants.Task.TaskSetupByType(this.projectname, 'network'), className: 'bi bi-diagram-3 display-1 text-primary' },
        { label: 'Category', route: AppRouteConstants.Task.TaskSetupByType(this.projectname, 'category'), className: 'bi bi-tags display-1 text-info' },
        { label: 'Status', route: AppRouteConstants.Task.TaskSetupByType(this.projectname, 'status'), className: 'bi bi-check-circle display-1 text-success' },
         { label: 'Resource', route: AppRouteConstants.Task.TaskSetupByType(this.projectname, 'resource'), className: 'bi bi-people display-3 text-warning' }
      );
  }

  goTo(path: string): void {
    this.router.navigateByUrl(path);
  }
}
