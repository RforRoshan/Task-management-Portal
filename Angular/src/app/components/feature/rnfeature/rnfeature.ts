import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { AddRNAndFeatureRequestModel, AddRNAndFeatureResponseModel, ApiResult, DropDownValueModelE, GetAllRNsRequestModel, GetAllRNsResponseModel, RequestObject, UpdateRNAndFeatureRequestModel, UpdateRNAndFeatureResponseModel } from '../../../models/common-model';
import { ActionConstants, AppRouteConstants, ControllerConstants, NumberColorMap, SessionStorageKeyConstants } from '../../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'rks-rnfeature',
  imports: [CommonModule, FormsModule],
  templateUrl: './rnfeature.html',
  styleUrl: './rnfeature.scss'
})
export class RNFeature {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  isOpen = true;
  projectname: string = '';
  rnAndFeatureList: DropDownValueModelE[] = [];
  numberColorMap = NumberColorMap;


  constructor(private route: ActivatedRoute, private router: Router, private httpService: HttpService, private alertService: AlertService) {
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }

  ngOnInit():void {
    this.getData();
  }

  goToRNDetails(rnAndFeature: DropDownValueModelE): void {
    localStorage.setItem(SessionStorageKeyConstants.RNName, rnAndFeature.value);
    this.router.navigateByUrl(AppRouteConstants.RNFeature.Detalis(this.projectname, rnAndFeature.guidId));
  }

  getData():void {
    let requestObject: RequestObject<GetAllRNsRequestModel> = new RequestObject<GetAllRNsRequestModel>();
    let requestModel: GetAllRNsRequestModel = new GetAllRNsRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllRNs;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllRNsResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          (responseModel.responseData.rnAndFeatureList ?? []).map(dropDown => {
            let dropDownValue = new DropDownValueModelE();
            Object.assign(dropDownValue, dropDown);
            this.rnAndFeatureList.push(dropDownValue);
          });
          this.isFullyLoaded = true;

          //console.log(this.dropDownValues);
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
  
  addButton():void{
    let rnAndFeature:DropDownValueModelE = new DropDownValueModelE();
    rnAndFeature.isEditing = true;
    this.rnAndFeatureList.push(rnAndFeature);
  }

  updateButton(rnAndFeature:DropDownValueModelE):void{
    if(rnAndFeature.guidId === ''){
      this.addRN(rnAndFeature);
    }
    else{
      this.updateRN(rnAndFeature);
    }
  }

  addRN(rnAndFeature:DropDownValueModelE):void{
    this.isLoading = true;
    
    let requestObject: RequestObject<AddRNAndFeatureRequestModel> = new RequestObject<AddRNAndFeatureRequestModel>();
    let requestModel: AddRNAndFeatureRequestModel = new AddRNAndFeatureRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.rn = rnAndFeature.value;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.AddRNAndFeature;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<AddRNAndFeatureResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          rnAndFeature.guidId = responseModel.responseData.guidId;
    
          this.isLoading = false;    
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

  updateRN(rnAndFeature:DropDownValueModelE):void{
    this.isLoading = true;
    
    let requestObject: RequestObject<UpdateRNAndFeatureRequestModel> = new RequestObject<UpdateRNAndFeatureRequestModel>();
    let requestModel: UpdateRNAndFeatureRequestModel = new UpdateRNAndFeatureRequestModel();
    requestModel.guidId = rnAndFeature.guidId;
    requestModel.rn = rnAndFeature.value;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateRNAndFeature;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateRNAndFeatureResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
    
          this.isLoading = false;    
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
