import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiResult, GetAllResourceNamesRequestModel, GetAllResourceNamesResponseModel, RequestObject, ResourceNameModel, ShowAndHideResourceNameRequestModel, ShowAndHideResourceNameResponseModel } from '../../../../models/common-model';
import { ActionConstants, ColorMap, ControllerConstants } from '../../../../constants/constants';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'rks-resource-setup',
  imports: [CommonModule, FormsModule],
  templateUrl: './resource-setup.html',
  styleUrl: './resource-setup.scss'
})
export class ResourceSetup {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  isOpen = true;
  projectname: string = '';
  resourceNames:ResourceNameModel[]= []
  colorMap: Record<string, string> = ColorMap;

   constructor(private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  ngOnInit():void {
    this.getResourceNamesData();
  }

  getResourceNamesData():void {

    let requestObject: RequestObject<GetAllResourceNamesRequestModel> = new RequestObject<GetAllResourceNamesRequestModel>();
    let requestModel: GetAllResourceNamesRequestModel = new GetAllResourceNamesRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllResourceNames;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllResourceNamesResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.resourceNames = responseModel.responseData.resourceNames;
          this.isFullyLoaded = true;

          //console.log(this.resourceNames);
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

  hideNameButton(resourceName:ResourceNameModel):void{

    this.alertService.showConfirmation(`Are you sure, want to hide ${resourceName.name} from dropdown!`, 'Yes, sure!').then((confirmed) => {
      if (confirmed) { 
        resourceName.isShow = false;
        this.showAndHideName(resourceName.guidId, resourceName.isShow)
      }
    });


  }

  showNameButton(resourceName:ResourceNameModel):void{

    this.alertService.showConfirmation(`Are you sure, want to show ${resourceName.name} in dropdown!`, 'Yes, sure!').then((confirmed) => {
      if (confirmed) { 
        resourceName.isShow = true;
        this.showAndHideName(resourceName.guidId, resourceName.isShow)
        
      }
    });
  }

  showAndHideName(guidId:string, isShow:boolean):void{
    this.isLoading = true;

    let requestObject: RequestObject<ShowAndHideResourceNameRequestModel> = new RequestObject<ShowAndHideResourceNameRequestModel>();
    let requestModel: ShowAndHideResourceNameRequestModel = new ShowAndHideResourceNameRequestModel();
    requestModel.guidId = guidId;
    requestModel.isShow = isShow;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.ShowAndHideResourceName;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<ShowAndHideResourceNameResponseModel> = response;
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
