import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { ApiResult, GetTaskLogByIdRequestModel, GetTaskLogByIdResponseModel, RequestObject, ResourceNameModel, TasksLog, TasksLogModel } from '../../../../models/common-model';
import { ActionConstants, ColorMap, ControllerConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-sub-task-history',
  imports: [CommonModule, FormsModule],
  templateUrl: './sub-task-history.html',
  styleUrl: './sub-task-history.scss'
})
export class SubTaskHistory {
  @Input() taskUniqueId!: string;
  @Input() resourceNamesListForLog: ResourceNameModel[] = [];
  colorMap: Record<string, string> = ColorMap;
  isOpen = false;
  isAPINeedToCall:boolean= true;
  tasksLogs:TasksLog[]=[];

  constructor(private httpService: HttpService, private alertService: AlertService) {}

  callLogData():void{
    this.isOpen = !this.isOpen

    if(this.isAPINeedToCall){
      this.isAPINeedToCall = false;
      this.getLogData()
    }
  }

  getLogData():void {
    let requestObject: RequestObject<GetTaskLogByIdRequestModel> = new RequestObject<GetTaskLogByIdRequestModel>();
    let requestModel: GetTaskLogByIdRequestModel = new GetTaskLogByIdRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetTaskLogById;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetTaskLogByIdResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.tasksLogs = (responseModel.responseData.tasksLogs ?? []).map(e => ({
            log: e.log,
            date: e.date,
            firstName: this.resourceNamesListForLog.find(r => r.userId === e.userId)?.name || '',
            lastName: this.resourceNamesListForLog.find(r => r.userId === e.userId)?.lastName || ''
          } as TasksLog));
          //console.log(this.tasksLogs);
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
      }
    });
  }
}
