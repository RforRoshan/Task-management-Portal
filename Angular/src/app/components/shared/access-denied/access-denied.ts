import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { AppRouteConstants } from '../../../constants/constants';

@Component({
  selector: 'rks-access-denied',
  imports: [],
  templateUrl: './access-denied.html',
  styleUrl: './access-denied.scss'
})
export class AccessDenied {
  constructor(private router: Router, private location: Location) {}

  goBack(): void {
    this.location.back();
  }
  goHome(): void {
    this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
  }
}
