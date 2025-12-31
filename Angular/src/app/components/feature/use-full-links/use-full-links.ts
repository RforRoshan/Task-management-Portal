import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { DisplayInputService } from '../../../services/display-input-service';
import { AddUseFullLinkRequestModel, AddUseFullLinkResponseModel, ApiResult, DeleteUseFullLinkRequestModel, DeleteUseFullLinkResponseModel, GetAllUseFullLinkRequestModel, GetAllUseFullLinkResponseModel, RequestObject, UpdateUseFullLinkRequestModel, UpdateUseFullLinkResponseModel, UseFullLinkModel } from '../../../models/common-model';
import { ActionConstants, ControllerConstants, NumberColorMap } from '../../../constants/constants';

@Component({
  selector: 'rks-use-full-links',
  imports: [CommonModule, FormsModule],
  templateUrl: './use-full-links.html',
  styleUrl: './use-full-links.scss'
})
export class UseFullLinks {
    isLoading:boolean = false;
    isFullyLoaded:boolean = false;
    isOpen = true;
    projectname: string = '';
    useFullLinksList:UseFullLinkModel[] = []
    numberColorMap = NumberColorMap;
  
  constructor(private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private displayInputService:DisplayInputService) {
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  

  ngOnInit():void {
    this.GetAllUseFullLink();
  }

  GetAllUseFullLink():void {
    let requestObject: RequestObject<GetAllUseFullLinkRequestModel> = new RequestObject<GetAllUseFullLinkRequestModel>();
    let requestModel: GetAllUseFullLinkRequestModel = new GetAllUseFullLinkRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllUseFullLink;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllUseFullLinkResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.useFullLinksList = responseModel.responseData.useFullLinks;
          this.isFullyLoaded = true;
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
    this.displayInputService.showUseFullLinkInputBox(false, '', '').then((result)=>{
      if(result){
        let useFullLinkModel:UseFullLinkModel = new UseFullLinkModel();
        useFullLinkModel.linkName = result.linkName;
        useFullLinkModel.linkURL = result.linkURL;
        this.addNewLink(useFullLinkModel);
      }
    });
  }
  
  updateButton(useFullLinkModel:UseFullLinkModel):void{
    this.displayInputService.showUseFullLinkInputBox(true, useFullLinkModel.linkName, useFullLinkModel.linkURL).then((result)=>{
      if(result){
        useFullLinkModel.linkName = result.linkName;
        useFullLinkModel.linkURL = result.linkURL;
        this.updateLink(useFullLinkModel);
      }
    });
  }
  
  deleteButton(useFullLinkModel:UseFullLinkModel):void{
    this.alertService.showConfirmation(`Do you want to url link "${useFullLinkModel.linkName.slice(0, 30)}.."!`, 'Yes, delete it!').then((confirmed) => {
      if (confirmed) {
        this.deleteLink(useFullLinkModel);
      }
    });
  }

  addNewLink(useFullLinkModel:UseFullLinkModel):void{
    this.isLoading = true;
    let requestObject: RequestObject<AddUseFullLinkRequestModel> = new RequestObject<AddUseFullLinkRequestModel>();
    let requestModel: AddUseFullLinkRequestModel = new AddUseFullLinkRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.linkName = useFullLinkModel.linkName;
    requestModel.linkURL = useFullLinkModel.linkURL;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.AddUseFullLink;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<AddUseFullLinkResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          useFullLinkModel.guidId = responseModel.responseData.guidId;
          this.useFullLinksList.push(useFullLinkModel);
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

  updateLink(useFullLinkModel:UseFullLinkModel):void{
    this.isLoading = true;
    let requestObject: RequestObject<UpdateUseFullLinkRequestModel> = new RequestObject<UpdateUseFullLinkRequestModel>();
    let requestModel: UpdateUseFullLinkRequestModel = new UpdateUseFullLinkRequestModel();
    requestModel.guidId = useFullLinkModel.guidId;
    requestModel.linkName = useFullLinkModel.linkName;
    requestModel.linkURL = useFullLinkModel.linkURL;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateUseFullLink;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateUseFullLinkResponseModel> = response;
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

  deleteLink(useFullLinkModel:UseFullLinkModel):void{
    this.isLoading = true;
    let requestObject: RequestObject<DeleteUseFullLinkRequestModel> = new RequestObject<DeleteUseFullLinkRequestModel>();
    let requestModel: DeleteUseFullLinkRequestModel = new DeleteUseFullLinkRequestModel();
    requestModel.guidId = useFullLinkModel.guidId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.DeleteUseFullLink;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<DeleteUseFullLinkResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.useFullLinksList = this.useFullLinksList.filter(l => l !== useFullLinkModel);
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

  openLinkInNewTab(link: string) {
    window.open(link, "_blank");
  }
   
}
