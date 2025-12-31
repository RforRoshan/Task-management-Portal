import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AddDropDownValuesRequestModel, AddDropDownValuesResponseModel, ApiResult, DeleteDropDownValuesRequestModel, DeleteDropDownValuesResponseModel, DropDownValueModelE, GetAllDropDownValuesByProjectKeyRequestModel, GetAllDropDownValuesByProjectKeyResponseModel, RequestObject, UpdateDropDownValuesRequestModel } from '../../../../models/common-model';
import { FormsModule } from '@angular/forms';
import { ActionConstants, ClassColorMap, ColorMap, ControllerConstants, StatusColorMap } from '../../../../constants/constants';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';

@Component({
  selector: 'rks-cnssetup',
  imports: [CommonModule, FormsModule],
  templateUrl: './cnssetup.html',
  styleUrl: './cnssetup.scss'
})
export class CNSSetup {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  isOpen = true;
  projectname: string = '';
  type:string = ''
  pageHeading:string = ''
  dropDownValues:DropDownValueModelE[]=[];
  colorMap: Record<string, string> = ColorMap;

  constructor(private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
    const projectname = this.route.parent?.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
    const type = this.route.snapshot.data['type'];
    this.type = type;
    this.pageHeading = this.type.charAt(0).toUpperCase() + this.type.slice(1).toLowerCase()
  }
  
  ngOnInit():void {
    this.getData();
    if(this.type === 'status'){
      this.colorMap = StatusColorMap;
    }
  }

  getData():void {

    let requestObject: RequestObject<GetAllDropDownValuesByProjectKeyRequestModel> = new RequestObject<GetAllDropDownValuesByProjectKeyRequestModel>();
    let requestModel: GetAllDropDownValuesByProjectKeyRequestModel = new GetAllDropDownValuesByProjectKeyRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.type = this.type;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllDropDownValuesByProjectKey;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllDropDownValuesByProjectKeyResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          (responseModel.responseData.dropDownValueModels ?? []).map(dropDown => {
            let dropDownValue = new DropDownValueModelE();
            Object.assign(dropDownValue, dropDown);
            this.dropDownValues.push(dropDownValue);
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
    let dropDownValue:DropDownValueModelE = new DropDownValueModelE();
    dropDownValue.isEditing = true;
    this.dropDownValues.push(dropDownValue);
  }

  updateButton(dropDownValue:DropDownValueModelE):void{
    if(dropDownValue.guidId === ''){
      this.addNewValue(dropDownValue);
    }
    else{
      this.updateValue(dropDownValue);
    }
  }

  addNewValue(dropDownValue:DropDownValueModelE):void{
    this.isLoading = true;
    
    let requestObject: RequestObject<AddDropDownValuesRequestModel> = new RequestObject<AddDropDownValuesRequestModel>();
    let requestModel: AddDropDownValuesRequestModel = new AddDropDownValuesRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.type = this.type;
    requestModel.value = dropDownValue.value;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.AddDropDownValues;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<AddDropDownValuesResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          dropDownValue.guidId = responseModel.responseData.guidId;
          dropDownValue.isEditing = false;
    
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

  deleteValue(dropDownValue:DropDownValueModelE):void{
    this.alertService.showConfirmation(`Do you want to delete value that is "${dropDownValue.value}"!`, 'Yes, delete it!').then((confirmed) => {
      if (confirmed) {
        this.isLoading = true;
        let requestObject: RequestObject<DeleteDropDownValuesRequestModel> = new RequestObject<DeleteDropDownValuesRequestModel>();
        let requestModel: DeleteDropDownValuesRequestModel = new DeleteDropDownValuesRequestModel();
        requestModel.guidId = dropDownValue.guidId;

        requestObject.controller = ControllerConstants.Task;
        requestObject.action = ActionConstants.DeleteDropDownValues;
        requestObject.requestModel = requestModel;

        this.httpService.post(requestObject).subscribe({
          next: (response) => {
            let responseModel: ApiResult<DeleteDropDownValuesResponseModel> = response;
            if(responseModel.status  && responseModel.responseData != null) { 
              this.dropDownValues = this.dropDownValues.filter(u => u.guidId !== dropDownValue.guidId);

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
    });
  }

  updateValue(dropDownValue:DropDownValueModelE):void{
    this.isLoading = true;
    let requestObject: RequestObject<UpdateDropDownValuesRequestModel> = new RequestObject<UpdateDropDownValuesRequestModel>();
    let requestModel: UpdateDropDownValuesRequestModel = new UpdateDropDownValuesRequestModel();
    requestModel.guidId = dropDownValue.guidId;
    requestModel.value = dropDownValue.value;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateDropDownValues;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateDropDownValuesRequestModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {  
          dropDownValue.isEditing = false;
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
