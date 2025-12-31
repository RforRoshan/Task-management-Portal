import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ApiResult, GetShowSubTasksToCopyRequestModel, GetShowSubTasksToCopyResponseModel, GetShowTasksToCopyRequestModel, GetShowTasksToCopyResponseModel, RequestObject, ShowSubTaskToCopyDTOE, ShowTaskToCopyDTO } from '../../../models/common-model';
import { ActionConstants, ClassColorMap, ColorMap, ControllerConstants, StatusColorMap } from '../../../constants/constants';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'rks-copy-task',
  imports: [CommonModule, FormsModule],
  templateUrl: './copy-task.html',
  styleUrl: './copy-task.scss'
})
export class CopyTask {

  @Input() projectname: string = '';

  isLoading:boolean = false;
  tasks:ShowTaskToCopyDTO[] = [];
  subTasks:ShowSubTaskToCopyDTOE[] = [];
  pageTypeToDisplay:number = 1;
  searchTerm = '';
  classColorMap: Record<string, string> = ClassColorMap;
  colorMap: Record<string, string> = ColorMap;
  statusColorMap: Record<string, string> = StatusColorMap;
  isAllSubTasksSelected: boolean = true;

  constructor(private httpService: HttpService, private alertService: AlertService) {
  }

  @Output() closed = new EventEmitter<ShowSubTaskToCopyDTOE[]>();

  onCancel() {
    this.closed.emit([]);
  }

  getTaskData(isAlive:boolean):void {
    this.isLoading = true;

    let requestObject: RequestObject<GetShowTasksToCopyRequestModel> = new RequestObject<GetShowTasksToCopyRequestModel>();
    let requestModel: GetShowTasksToCopyRequestModel = new GetShowTasksToCopyRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.isAlive = isAlive;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetShowTasksToCopy;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetShowTasksToCopyResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.tasks = responseModel.responseData.tasks;
          console.log(this.tasks);
          this.isLoading = false;
          this.pageTypeToDisplay = 2;
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

  get filteredTasks() {
    return this.tasks.filter(task =>
      (task.taskName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.status.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.subProject.toLowerCase().includes(this.searchTerm.toLowerCase())
      )
    );
  }

  getSubTaskData(taskUniqueId:string):void {
    this.isLoading = true;

    let requestObject: RequestObject<GetShowSubTasksToCopyRequestModel> = new RequestObject<GetShowSubTasksToCopyRequestModel>();
    let requestModel: GetShowSubTasksToCopyRequestModel = new GetShowSubTasksToCopyRequestModel();
    requestModel.taskUniqueId = taskUniqueId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetShowSubTasksToCopy;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetShowSubTasksToCopyResponseModel> = response;
        if(responseModel.status  && responseModel.responseData.subTasks != null) {

          this.subTasks = []
          this.subTasks = responseModel.responseData.subTasks.map(st => {return Object.assign(new ShowSubTaskToCopyDTOE(), st);});
          console.log(this.subTasks);
          this.isLoading = false;
          this.pageTypeToDisplay = 3;
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

  addSubTask():void{
    this.closed.emit(this.subTasks.filter(u=> u.isSelected));
  }

  toggleSelectUnSelectAllSubTasks():void{
    if(this.isAllSubTasksSelected){
      this.subTasks.forEach(st => st.isSelected = false);
      this.isAllSubTasksSelected = false;
    }
    else{
      this.subTasks.forEach(st => st.isSelected = true);
      this.isAllSubTasksSelected = true;
    }
  }

  toggleSubTaskSelection(subTask:ShowSubTaskToCopyDTOE):void{
    subTask.isSelected = !subTask.isSelected;
    if(!subTask.isSelected){
      this.isAllSubTasksSelected = false;
    }
  }

}
