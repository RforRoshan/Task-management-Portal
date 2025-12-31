import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { AddSubTaskRequestModel, AddSubTaskResponseModel, AddWorkTimeRequestModel, AddWorkTimeResponseModel, ApiResult, GetSubTasksByIdRequestModel, GetSubTasksByIdResponseModel, RequestObject, SubTaskModelE, UpdateSubTaskRequestModel, UpdateSubTaskResponseModel, UpdateWorkTimeRequestModel, UpdateWorkTimeResponseModel } from '../../../../models/common-model';
import { ActionConstants, ColorMap, ControllerConstants } from '../../../../constants/constants';
import { DisplayHrsPipe } from "../../../../pipes/display-hrs-pipe";
import { DisplayInputService } from '../../../../services/display-input-service';

@Component({
  selector: 'rks-sub-task-details',
  imports: [CommonModule, FormsModule, DisplayHrsPipe],
  templateUrl: './sub-task-details.html',
  styleUrl: './sub-task-details.scss'
})
export class SubTaskDetails {
  @Input({ required: true }) taskUniqueId!: string;
  //@Output() changeUsedETA = new EventEmitter<number>();
  Math = Math;
  isOpenSelfSubTasks = true;
  isOpenOtherSubTasks = true;
  isLoading:boolean = false;
  subTasks:SubTaskModelE[] = [];
  otherSubTasks:SubTaskModelE[] = [];
  subTaskStatusList: string[] = ['Pending', 'In-Progress', 'Done', 'On-Hold', 'Cancelled'];
  colorMap: Record<string, string> = ColorMap;
  fromDateSubTask:string = this.getTodayDate();
  toDateSubTask:string = this.getTodayDate();
  isFilteredSubTask:boolean = false;

  constructor(private httpService: HttpService, private alertService: AlertService, private displayInputService:DisplayInputService) {}
  
  ngOnInit():void {
    this.getSubTaskData();
  }

  get filteredSubTasks() {
    return this.subTasks.filter(task =>
      task.workEntrys.some(element =>
        element.entryDate >= this.fromDateSubTask && element.entryDate <= this.toDateSubTask
      ) || !this.isFilteredSubTask
    );
  }

  get filteredOtherSubTasks() {
    return this.otherSubTasks.filter(task =>
      task.workEntrys.some(element =>
        element.entryDate >= this.fromDateSubTask && element.entryDate <= this.toDateSubTask
      ) || !this.isFilteredSubTask
    );
  }

  private getTodayDate(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  // calculateUseETA(): void {
  //   const usedETA = this.subTasks.flatMap(st => st.workEntrys)
  //     .reduce((sum, e) => sum + (e.entryHrs || 0), 0);

  //   this.changeUsedETA.emit(usedETA);
  // }

  getSubTaskData():void {
    this.isLoading = true;

    let requestObject: RequestObject<GetSubTasksByIdRequestModel> = new RequestObject<GetSubTasksByIdRequestModel>();
    let requestModel: GetSubTasksByIdRequestModel = new GetSubTasksByIdRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetSubTasksById;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetSubTasksByIdResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.subTasks = (responseModel.responseData.subTasks ?? []).map(st => {
            const subTask = Object.assign(new SubTaskModelE(), st);
            return subTask;
          });

          this.otherSubTasks = (responseModel.responseData.otherSubTasks ?? []).map(st => {
            const subTask = Object.assign(new SubTaskModelE(), st);
            return subTask;
          });

          this.isLoading = false;
          // console.log(this.subTasks);
          // console.log(this.otherSubTasks);
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

  addSubTaskAPICall(subTask: SubTaskModelE, isSelf: boolean = true):void {
    this.isLoading = true;  
    let requestObject: RequestObject<AddSubTaskRequestModel> = new RequestObject<AddSubTaskRequestModel>();
    let requestModel: AddSubTaskRequestModel = new AddSubTaskRequestModel();
    requestModel.subTaskSeq = subTask.subTaskSeq;
    requestModel.taskUniqueId = this.taskUniqueId;
    requestModel.subTaskName = subTask.subTaskName;
    requestModel.subTaskETA = subTask.subTaskETA;
    requestModel.status = subTask.status;
    requestModel.remark = subTask.remark;
    requestModel.isColour = subTask.isColour;
    requestModel.isSelf = isSelf;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.AddSubTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<AddSubTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          subTask.subTaskUniqueId = responseModel.responseData.subTaskUniqueId;
          const totalETA = responseModel.responseData.totalETA;
          if(totalETA > 0){
            console.log(totalETA);
          }
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

  updateSubTaskAPICall(subTask: SubTaskModelE):void {
    this.isLoading = true;  
    let requestObject: RequestObject<UpdateSubTaskRequestModel> = new RequestObject<UpdateSubTaskRequestModel>();
    let requestModel: UpdateSubTaskRequestModel = new UpdateSubTaskRequestModel();
    requestModel.subTaskUniqueId = subTask.subTaskUniqueId;
    requestModel.subTaskSeq = subTask.subTaskSeq;
    requestModel.subTaskName = subTask.subTaskName;
    requestModel.subTaskETA = subTask.subTaskETA;
    requestModel.status = subTask.status;
    requestModel.remark = subTask.remark;
    requestModel.isColour = subTask.isColour;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateSubTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateSubTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          const totalETA = responseModel.responseData.totalETA;
          if(totalETA > 0){
            //console.log(totalETA);
          }
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


  addSelfSubTask():void {
    const newSubTask = Object.assign(new SubTaskModelE(), {
      isEditing: true,
      subTaskSeq: this.subTasks.length + 1,
      status: 'Pending'
    });
    this.subTasks.push(newSubTask);
    this.isFilteredSubTask = false;
  }

  addOtherSubTask():void {
    const newSubTask = Object.assign(new SubTaskModelE(), {
      isEditing: true,
      subTaskSeq: this.otherSubTasks.length + 1,
      status: 'Pending'
    });
    this.otherSubTasks.push(newSubTask);
    this.isFilteredSubTask = false;
  }

  updateSelfSubTask(subTask: SubTaskModelE):void {
    if(subTask.subTaskETA == null || subTask.subTaskETA < 0){
      subTask.subTaskETA = 0;
    }
    if(subTask.subTaskUniqueId){
      this.updateSubTaskAPICall(subTask);
    }
    else{
      this.addSubTaskAPICall(subTask);
    }
  }

  updateOtherSubTask(subTask: SubTaskModelE):void {
    if(subTask.subTaskETA == null || subTask.subTaskETA < 0){
      subTask.subTaskETA = 0;
    }
    if(subTask.subTaskUniqueId){
      this.updateSubTaskAPICall(subTask);
    }
    else{
      this.addSubTaskAPICall(subTask, false);
    }
  }


  // addWorkHrs(subTask: SubTaskModelE):void{
    // this.displayInputService.showWorkEntryInputBox().then((result) => {
    //   if (result != null) {
    //     let entryDate:number = this.dateToJulian(result.enterDate);
    //     //console.log(entryDate, result.enterHrs);

    //     if(result.enterHrs > 0){
    //       this.isLoading = true;

    //       if(!subTask.workEntrys.some(e => e.entryDate === entryDate)){
    //         subTask.workEntrys.unshift(Object.assign(new WorkEntryModelE(), { entryDate: entryDate }));
    //       }
    //       this.addWorkHrsAPI(subTask, entryDate, result.enterHrs);
    //       this.isLoading = false;
    //     }
    //   }
    //   else{
    //     //console.log('Cancel');
    //   }
    // });
  // }

  //updateWorkHrs(): void {
    // if(workEntry.workEntryUniqueId){
    //   this.isLoading = true; 
    //   if(workEntry.entryHrs == null || workEntry.entryHrs < 0){
    //     workEntry.entryHrs = 0;
    //   }
    //   let requestObject: RequestObject<UpdateWorkTimeRequestModel> = new RequestObject<UpdateWorkTimeRequestModel>();
    //   let requestModel: UpdateWorkTimeRequestModel = new UpdateWorkTimeRequestModel();
    //   requestModel.workEntryUniqueId = workEntry.workEntryUniqueId;
    //   requestModel.entryHrs = workEntry.entryHrs;

    //   requestObject.controller = ControllerConstants.Task;
    //   requestObject.action = ActionConstants.UpdateWorkTime;
    //   requestObject.requestModel = requestModel;

    //   this.httpService.post(requestObject).subscribe({
    //     next: (response) => {
    //       let responseModel: ApiResult<UpdateWorkTimeResponseModel> = response;
    //       if(responseModel.status  && responseModel.responseData != null) {
    //         if(workEntry.entryHrs == 0){
    //           workEntry.workEntryUniqueId = '';
    //         }
    //         this.calculateUseETA();
    //         this.isLoading = false;    
    //       }
    //       else {
    //         this.alertService.showError(response.message);
    //         this.isLoading = false;
    //       }
    //     },
    //     error: (error) => {
    //       this.isLoading = false; 
    //     }
    //   });
    // }   
  //}

  // addWorkHrsAPI(subTask: SubTaskModelE, entryDate:number, entryHrs: number):void {
    // let requestObject: RequestObject<AddWorkTimeRequestModel> = new RequestObject<AddWorkTimeRequestModel>();
    // let requestModel: AddWorkTimeRequestModel = new AddWorkTimeRequestModel();
    // requestModel.taskUniqueId = this.taskUniqueId;
    // requestModel.subTaskUniqueId = subTask.subTaskUniqueId;
    // requestModel.entryDate = entryDate;
    // requestModel.entryHrs = entryHrs;

    // requestObject.controller = ControllerConstants.Task;
    // requestObject.action = ActionConstants.AddWorkTime;
    // requestObject.requestModel = requestModel;

    // this.httpService.post(requestObject).subscribe({
    //   next: (response) => {
    //     let responseModel: ApiResult<AddWorkTimeResponseModel> = response;
    //     if(responseModel.status  && responseModel.responseData != null) {
    //       const entry = subTask.workEntrys.find(u => u.entryDate === entryDate);
    //       if (entry) {
    //         entry.entryHrs = responseModel.responseData.entryHrs;
    //         entry.workEntryUniqueId = responseModel.responseData.subTaskUniqueId;
    //       }
    //       this.calculateUseETA();  
    //     }
    //     else {
    //       this.alertService.showError(response.message);
    //       this.isLoading = false;
    //     }
    //   },
    //   error: (error) => {
    //   }
    // });
  // }

}
