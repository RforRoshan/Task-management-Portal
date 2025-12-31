import { Component } from '@angular/core';
import { AddSubTaskRequestModel, AddSubTaskResponseModel, AddWorkTimeRequestModel, AddWorkTimeResponseModel, ApiResult, ApproveETARequestModel, ApproveETAResponseModel, DropDownValueModel, EHrsLastDTO, GetAllDropDownValuesRequestModel, GetAllDropDownValuesResponseModel, GetSubTasksByIdRequestModel, GetSubTasksByIdResponseModel, GetTaskByIdModelE, GetTaskByIdRequestModel, GetTaskByIdResponseModel, RequestObject, ResourceNameModel, ShowSubTaskToCopyDTOE, SubTaskModelE, UpdateSubTaskRequestModel, UpdateSubTaskResponseModel, UpdateTaskRequestModel, UpdateTaskResponseModel, UpdateWorkTimeRequestModel, UpdateWorkTimeResponseModel, WorkEntryDTOE } from '../../../models/common-model';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { ActionConstants, AppRouteConstants, ClassColorMap, ColorMap, ControllerConstants, JiraColorMap, SessionStorageKeyConstants, StatusColorMap, URLConstants, XlsxConstants } from '../../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DisplayHrsPipe } from '../../../pipes/display-hrs-pipe';
import { LeftETAPipe } from "../../../pipes/left-eta-pipe";
import { SubTaskHistory } from "./sub-task-history/sub-task-history";
import { TaskComments } from "./task-comments/task-comments";
import { DisplayInputService } from '../../../services/display-input-service';
import { CopyTask } from "../copy-task/copy-task";
import { Workbook } from 'exceljs';
import {saveAs} from 'file-saver';

@Component({
  selector: 'rks-sub-task',
  imports: [CommonModule, FormsModule, DisplayHrsPipe, LeftETAPipe, SubTaskHistory, TaskComments, CopyTask],
  templateUrl: './sub-task.html',
  styleUrl: './sub-task.scss'
})
export class SubTask {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  isManager: boolean = ['101', '102'].includes(localStorage.getItem(SessionStorageKeyConstants.RoleId) || '');
  currentUserId: number = Number(localStorage.getItem(SessionStorageKeyConstants.UserId) ?? 0);
  isSameUser: boolean = false;
  classColorMap: Record<string, string> = ClassColorMap;
  colorMap: Record<string, string> = ColorMap;
  jiraColorMap: Record<string, string> = JiraColorMap;
  statusColorMap: Record<string, string> = StatusColorMap;
  isEditing: boolean = false;
  isOpen = true;
  taskUniqueId: string = '';
  projectname: string = '';
  
  resourceNamesList: ResourceNameModel[] = [];
  resourceNamesListForLog: ResourceNameModel[] = [];
  statusList: DropDownValueModel[] = [];
  categoryList: DropDownValueModel[] = [];
  projectList: DropDownValueModel[] = [];
  networkList: DropDownValueModel[] = [];
  rnAndFeatureList: DropDownValueModel[] = [];
  displayDate:string = '';
  
  userId: number = 0;
  userName: string = 'UnAssigned';
  taskSeq: number = 0;
  catagory: string = '';
  taskName: string = '' ;
  subProject: string = '';
  network: string = '';
  status: string = ''; 
  totalETA: number = 0;
  usedETA: number = 0;
  otherUsedETA: number = 0;
  lastDayWork: boolean = false;
  todayDayWork: boolean = false;
  itemToDiscuss: boolean = false;
  eHrsToday: string = '';
  eHrsLast: EHrsLastDTO[] = [];
  myComments: string = '';
  managerComments: string = '';
  jira: string = '';
  firstName: string = '';
  lastName: string = ''
  isApprove: boolean = false;
  rnGuidId: string = '';
  featureName: string = '';
  fixVersion: string = '';
  rnComments: string = '';
  mailTitled: string = '';
  rnName: string = '';

  Math = Math;
  isOpenSelfSubTasks = true;
  isOpenOtherSubTasks = true;
  subTasks:SubTaskModelE[] = [];
  otherSubTasks:SubTaskModelE[] = [];
  subTaskStatusList: string[] = ['Pending', 'In-Progress', 'Done', 'On-Hold', 'Cancelled'];
  isFilteredSubTask:boolean = false;
  otherTotalUsedHrs: EHrsLastDTO[] = [];

  isNeedToCallCopyPage:boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private displayInputService:DisplayInputService) {
    let taskUniqueId = this.route.snapshot.paramMap.get('taskUniqueId');
    this.taskUniqueId = taskUniqueId!;
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }

  ngOnInit():void {
    if(this.projectname != undefined && this.projectname != null && this.projectname != ''){
      this.getDropDownData();
    }
  }

  getTaskData(displayDate:string):void {
    if(displayDate == '' || displayDate == null || displayDate == undefined || displayDate.length < 7){
      return;
    }
    let requestObject: RequestObject<GetTaskByIdRequestModel> = new RequestObject<GetTaskByIdRequestModel>();
    let requestModel: GetTaskByIdRequestModel = new GetTaskByIdRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;
    requestModel.displayDate = displayDate;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetTaskById;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetTaskByIdResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.userId = responseModel.responseData.userId;
          this.userName = responseModel.responseData.userName;
          this.taskSeq = responseModel.responseData.taskSeq;
          this.catagory = responseModel.responseData.catagory;
          this.taskName = responseModel.responseData.taskName;
          this.subProject = responseModel.responseData.subProject;
          this.network = responseModel.responseData.network;
          this.status = responseModel.responseData.status;
          this.todayDayWork = responseModel.responseData.todayDayWork;
          this.eHrsToday = responseModel.responseData.eHrsToday;
          this.myComments = responseModel.responseData.myComments;
          this.managerComments = responseModel.responseData.managerComments;
          this.jira = responseModel.responseData.jira;


          this.totalETA = responseModel.responseData.totalETA;
          this.usedETA = responseModel.responseData.usedETA;
          this.otherUsedETA = responseModel.responseData.otherUsedETA;
          this.lastDayWork = responseModel.responseData.lastDayWork;
          this.itemToDiscuss = responseModel.responseData.itemToDiscuss;
          this.eHrsLast = responseModel.responseData.eHrsLast;
          this.isApprove = responseModel.responseData.isApprove;

          this.rnGuidId = responseModel.responseData.rnGuidId;
          this.featureName = responseModel.responseData.featureName;
          this.fixVersion = responseModel.responseData.fixVersion;
          this.rnComments = responseModel.responseData.rnComments;
          this.mailTitled = responseModel.responseData.mailTitled;
          
          this.rnName = this.rnAndFeatureList.find(r => r.guidId === this.rnGuidId)?.value || '';
          
          this.isSameUser = (localStorage.getItem(SessionStorageKeyConstants.Username) ?? '') === this.userName;

          this.firstName = this.resourceNamesList.find(r => r.userName === this.userName)?.name || '';
          this.lastName = this.resourceNamesList.find(r => r.userName === this.userName)?.lastName || '';

          
          this.isFullyLoaded = true;
          //console.log(this.task);
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

  getDropDownData():void {

    let requestObject: RequestObject<GetAllDropDownValuesRequestModel> = new RequestObject<GetAllDropDownValuesRequestModel>();
    let requestModel: GetAllDropDownValuesRequestModel = new GetAllDropDownValuesRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllDropDownValues;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllDropDownValuesResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.resourceNamesList = responseModel.responseData.resourceNames;
          this.statusList = responseModel.responseData.status;
          this.categoryList = responseModel.responseData.category;
          this.projectList = responseModel.responseData.project;
          this.networkList = responseModel.responseData.network;
          this.rnAndFeatureList = responseModel.responseData.rnAndFeatureList ?? [];

          this.resourceNamesListForLog = [...this.resourceNamesList];
          this.resourceNamesList = this.resourceNamesList.filter(r => r.isShow);

          const today = new Date();
          if(this.projectname == 'ccp' || this.projectname == 'plat'){
            const day = today.getDay(); // 0 = Sunday, 1 = Monday, ... 5 = Friday, 6 = Saturday
            // Monday → go back 3 days (to Friday)
            // Sunday → go back 2 days (to Friday)
            // All other days → go back 1 day
            if (day === 1) {
              today.setDate(today.getDate() - 3);
            } 
            else if (day === 0) {
              today.setDate(today.getDate() - 2);
            } 
            else {
              today.setDate(today.getDate() - 1);
            }
          }
          const yyyy = today.getFullYear();
          const mm = String(today.getMonth() + 1).padStart(2, '0');
          const dd = String(today.getDate()).padStart(2, '0');
          this.displayDate = `${yyyy}-${mm}-${dd}`;

          this.getTaskData(this.displayDate);
          this.getSubTaskData();

          // console.log(this.resourceNamesList);
          // console.log(this.statusList);
          // console.log(this.categoryList);
          // console.log(this.projectList);
          // console.log(this.networkList);
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

  updateDailyTask():void {
    this.isLoading = true;

    this.rnName = this.rnAndFeatureList.find(r => r.guidId === this.rnGuidId)?.value || '';
    this.firstName = this.resourceNamesList.find(r => r.userName === this.userName)?.name || '';
    this.lastName = this.resourceNamesList.find(r => r.userName === this.userName)?.lastName || '';

    let requestObject: RequestObject<UpdateTaskRequestModel> = new RequestObject<UpdateTaskRequestModel>();
    let requestModel: UpdateTaskRequestModel = new UpdateTaskRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;
    requestModel.userId = this.resourceNamesList.find(r => r.userName === this.userName)?.userId || 0;
    requestModel.userName = this.userName;
    requestModel.catagory = this.catagory;
    requestModel.taskName = this.taskName;
    requestModel.subProject = this.subProject;
    requestModel.network = this.network;
    requestModel.status = this.status;
    requestModel.itemToDiscuss = this.itemToDiscuss;
    requestModel.myComments = this.myComments;
    requestModel.jira = this.jira;
    requestModel.rnGuidId = this.rnGuidId;
    requestModel.featureName = this.featureName;
    requestModel.fixVersion = this.fixVersion;
    requestModel.rnComments = this.rnComments;
    requestModel.mailTitled = this.mailTitled;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.userName = responseModel.responseData.userName;
          this.isSameUser = (localStorage.getItem(SessionStorageKeyConstants.Username) ?? '') === this.userName;
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

  // goTo(path: string): void {
  //   this.router.navigateByUrl(path);
  // }

  goToRNDetails(guidId: string): void {
    localStorage.setItem(SessionStorageKeyConstants.RNName, this.rnName || '' );
    this.router.navigateByUrl(AppRouteConstants.RNFeature.Detalis(this.projectname, guidId));
  }
  // ================Sub Task=========================

  private getTodayDate(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }

  getSubTaskData():void {
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

          // this.subTasks = responseModel.responseData.subTasks.map(st => {
          //   const subTask = Object.assign(new SubTaskModelE(), st);
          //   return subTask;
          // });

          // this.otherSubTasks = responseModel.responseData.otherSubTasks.map(st => {
          //   const subTask = Object.assign(new SubTaskModelE(), st);
          //   return subTask;
          // });

          this.subTasks = [];
          this.subTasks = (responseModel.responseData.subTasks ?? []).map(st => {
            const mapped = Object.assign(new SubTaskModelE(), st);

            mapped.workEntrys = st.workEntrys.map(w => {
              const entry = Object.assign(new WorkEntryDTOE(), w);
              entry.firstName = this.resourceNamesListForLog.find(r => r.userId === w.userId)?.name || '';
              return entry;
            });

            return mapped;
          });

          this.otherSubTasks = [];
          this.otherSubTasks = (responseModel.responseData.otherSubTasks ?? []).map(st => {
            const mapped = Object.assign(new SubTaskModelE(), st);

            mapped.workEntrys = (st.workEntrys ?? []).map(w => {
              const entry = Object.assign(new WorkEntryDTOE(), w);
              entry.firstName = this.resourceNamesListForLog.find(r => r.userId === w.userId)?.name || '';
              return entry;
            });

            return mapped;
          });
          this.calculateOtherTotalUsedHrs();
          // console.log(this.subTasks);
          // console.log(this.otherSubTasks);
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
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
          if(responseModel.responseData.totalETA > 0){
            this.totalETA = responseModel.responseData.totalETA
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
          if(responseModel.responseData.totalETA > 0){
            this.totalETA = responseModel.responseData.totalETA
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

  approveETAAPICall(uniqueId: string, isAll:boolean = false):void {
    this.isLoading = true;  
    let requestObject: RequestObject<ApproveETARequestModel> = new RequestObject<ApproveETARequestModel>();
    let requestModel: ApproveETARequestModel = new ApproveETARequestModel();
    requestModel.uniqueId = uniqueId;
    requestModel.isAll = isAll;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.ApproveETA;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<ApproveETAResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          if(responseModel.responseData.totalETA > 0){
            this.totalETA = responseModel.responseData.totalETA
            this.isApprove = true;
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

  approveSubTaskETA(subTask: SubTaskModelE):void {
    this.approveETAAPICall(subTask.subTaskUniqueId, false);
    subTask.isApprove = true;
  }

  approveAllETA(taskUniqueId: string):void {
    this.alertService.showConfirmation(`Are you sure, want to approve ETA = ${this.totalETA} hours`, 'Yes, approve it!').then((confirmed) => {
      if (confirmed) {
        this.approveETAAPICall(taskUniqueId, true);
        this.subTasks.forEach(subTask => {
            subTask.isApprove = true;
        });
      }
    });
  }
  

  // ================Work Entry=========================

  addWorkHrs(subTask: SubTaskModelE):void{
    this.displayInputService.showWorkEntryInputBox().then((result) => {
      if (result != null && result.enterHrs > 0) {   
        this.isLoading = true;  
        let requestObject: RequestObject<AddWorkTimeRequestModel> = new RequestObject<AddWorkTimeRequestModel>();
        let requestModel: AddWorkTimeRequestModel = new AddWorkTimeRequestModel();
        requestModel.subTaskUniqueId = subTask.subTaskUniqueId;
        requestModel.taskUniqueId = this.taskUniqueId;
        requestModel.entryDate = result.enterDate;
        requestModel.entryHrs = result.enterHrs;

        requestObject.controller = ControllerConstants.Task;
        requestObject.action = ActionConstants.AddWorkTime;
        requestObject.requestModel = requestModel;

        this.httpService.post(requestObject).subscribe({
          next: (response) => {
            let responseModel: ApiResult<AddWorkTimeResponseModel> = response;
            if(responseModel.status  && responseModel.responseData != null) {

              let workEntry = subTask.workEntrys.find(u => u.workEntryUniqueId === responseModel.responseData.workEntryUniqueId);

              
              if (workEntry) {
                //workEntry.workEntryUniqueId = responseModel.responseData.workEntryUniqueId;
                workEntry.entryHrs = responseModel.responseData.entryHrs;
              } 
              else {
                let workEntry: WorkEntryDTOE = new WorkEntryDTOE();
                
                workEntry.workEntryUniqueId = responseModel.responseData.workEntryUniqueId;
                workEntry.entryDate = requestModel.entryDate;
                workEntry.entryHrs = responseModel.responseData.entryHrs;
                workEntry.userId = responseModel.responseData.userId;
                workEntry.firstName = this.resourceNamesListForLog.find(r => r.userId === workEntry.userId)?.name || '';
                subTask.workEntrys.unshift(workEntry);
              }
              this.usedETA = responseModel.responseData.usedETA;
              this.otherUsedETA = responseModel.responseData.otherUsedETA;
              this.calculateOtherTotalUsedHrs();
              if(requestModel.entryDate === this.displayDate){
                this.calculateEHrsLast() 
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
        this.isLoading = false;
      }   
    });
  }

  updateWorkTimeAPICall(workEntry: WorkEntryDTOE, subTask:SubTaskModelE):void {
    this.isLoading = true;  
    if(workEntry.entryHrs == null || workEntry.entryHrs < 0){
        workEntry.entryHrs = 0;
    }

    let requestObject: RequestObject<UpdateWorkTimeRequestModel> = new RequestObject<UpdateWorkTimeRequestModel>();
    let requestModel: UpdateWorkTimeRequestModel = new UpdateWorkTimeRequestModel();
    requestModel.workEntryUniqueId = workEntry.workEntryUniqueId;
    requestModel.entryHrs = workEntry.entryHrs;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateWorkTime;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateWorkTimeResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {     
          workEntry.entryHrs = responseModel.responseData.entryHrs;
          this.usedETA = responseModel.responseData.usedETA;
          this.otherUsedETA = responseModel.responseData.otherUsedETA;
          this.calculateOtherTotalUsedHrs();
          if(workEntry.entryDate === this.displayDate){
            this.calculateEHrsLast() 
          }

          if (workEntry.entryHrs === 0) {  
            subTask.workEntrys = subTask.workEntrys.filter(w => w.workEntryUniqueId !== workEntry.workEntryUniqueId);
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


  calculateEHrsLast():void{
    this.eHrsLast = [];
    this.subTasks.forEach(subTask => {
      subTask.workEntrys.forEach(entry => {
        if (entry.entryDate === this.displayDate) {

          let eHrsLastDTO = this.eHrsLast.find(
            u => u.firstName === entry.firstName
          );

          if (eHrsLastDTO) {
            eHrsLastDTO.worksHrs += entry.entryHrs;
          } 
          else {
            this.eHrsLast.push({
              firstName: entry.firstName,
              worksHrs: entry.entryHrs
            });
          }
        }
      });
    });
    this.otherSubTasks.forEach(subTask => {
      subTask.workEntrys.forEach(entry => {
        if (entry.entryDate === this.displayDate) {

          let eHrsLastDTO = this.eHrsLast.find(
            u => u.firstName === entry.firstName
          );

          if (eHrsLastDTO) {
            eHrsLastDTO.worksHrs += entry.entryHrs;
          } 
          else {
            this.eHrsLast.push({
              firstName: entry.firstName,
              worksHrs: entry.entryHrs
            });
          }
        }
      });
    });
  }

  calculateOtherTotalUsedHrs():void{
    this.otherTotalUsedHrs = [];
    this.otherSubTasks.forEach(subTask => {
      subTask.workEntrys.forEach(entry => {
        let eHrsLastDTO = this.otherTotalUsedHrs.find( u => u.firstName === entry.firstName );
        if (eHrsLastDTO) {
          eHrsLastDTO.worksHrs += entry.entryHrs;
        }
        else {
          this.otherTotalUsedHrs.push({
            firstName: entry.firstName,
            worksHrs: entry.entryHrs
          });
        }
      });
    });
  }


  // copySubTask():void{
  //   this.displayInputService.showTaskCopyInputBox(this.projectname).then((result) => {
  //     if (result != null && result.length > 0) {   
  //       result.forEach(element => {
  //         const newSubTask = Object.assign(new SubTaskModelE(), {
  //         isEditing: true,
  //         subTaskSeq: this.subTasks.length + 1,
  //         status: 'Pending',
  //         subTaskName: element.subTaskName,
  //         subTaskETA: element.subTaskETA
  //       });
  //       this.subTasks.push(newSubTask);
  //       });
  //     this.isFilteredSubTask = false;
  //     }   
  //   });
  
  // }

  afterCopyPageClosed(result:ShowSubTaskToCopyDTOE[]):void{
    this.isNeedToCallCopyPage = false;

    result.forEach(element => {
      const newSubTask = Object.assign(new SubTaskModelE(), {
      isEditing: true,
      subTaskSeq: this.subTasks.length + 1,
      status: 'Pending',
      subTaskName: element.subTaskName,
      subTaskETA: element.subTaskETA
    });
    this.subTasks.push(newSubTask);
    });
    
    this.isFilteredSubTask = false;
  }

  openJiraInNewTab(jira: string) {
    window.open(URLConstants.Jira + jira, "_blank");
  }

  exportToExcel(): void {
    const workbook = new Workbook();
    const sheet = workbook.addWorksheet('Sub Tasks');

    // -----------------------------
    // HEADER SETUP
    // -----------------------------
    sheet.addRow([]);
    const headerRowNumber = sheet.rowCount;
    sheet.addRow([
      `Seq. No. ${this.taskSeq}`,
      this.taskName,
      '',
      `Task ETA: ${this.totalETA > 0 ? `${this.totalETA}h` : 'N.A.'}`,
      `Used ETA: ${this.usedETA > 0 ? `${this.usedETA}h` : 'N.A.'}`,
      `Left ETA: ${(this.totalETA - this.usedETA) > 0 ? `${this.totalETA - this.usedETA}h` : 'N.A.'}`
    ]);

    sheet.mergeCells(`B2:C2`);
    sheet.getRow(2).height = Math.ceil(this.taskName.length / 45) * 15; 
    
    let rowNumber = 3;
    const initialNumber = rowNumber;

    // HELPER FUNCTIONS
    const addRichTextSingleRow = (col:string, label: string, text: string) => {
      sheet.getCell(`${col}${rowNumber}`).value = {
        richText: [
          { text: label, font: { name: "Segoe UI", size: 11, color: { argb: "FF000000" }, bold: true } },
          { text: text, font: { name: "Segoe UI", size: 11, color: { argb: "FF000000" }} }
        ]
      };
    };
    
    const addRichTextRow = (label1: string, text1: string, label2: string, text2: string) => {
      addRichTextSingleRow('B', label1, text1);
      addRichTextSingleRow('C', label2, text2);
      rowNumber++;
    };

    const mergeAndSet = (label: string, text: string, ) => {
      sheet.mergeCells(`B${rowNumber}:C${rowNumber}`);
      addRichTextSingleRow('B', label, text);
      sheet.getRow(rowNumber).height = Math.ceil((label.length + text.length) / 45) * 15; 
      rowNumber++;
    };
    // -----------------------------

    addRichTextSingleRow('B', "Other person contribution: ", this.otherUsedETA > 0 ? `${this.otherUsedETA}h` : 'N.A.');
    
    this.otherTotalUsedHrs.forEach(entry => {    
      addRichTextSingleRow('C', `${entry.firstName}: `, `${entry.worksHrs}h`);
      rowNumber++;
    });

    if(initialNumber != rowNumber){
      sheet.mergeCells(`B${initialNumber}:B${rowNumber -1}`);
    }
    else{
      addRichTextSingleRow('C', '', '');
      rowNumber += 1;
    }

    addRichTextRow("Catagory: ", this.catagory, "Project: ", this.subProject);
    addRichTextRow("Network: ", this.network, "Status: ", this.status);
    addRichTextRow("Resource: ", this.firstName, "Jira: ", this.jira);

    mergeAndSet("Comments: ", this.myComments);
    mergeAndSet("Manager Comments: ", this.managerComments);
    mergeAndSet("Mail Titled: ", this.mailTitled);
    mergeAndSet("Below is for RN / Feature List Details", '');
    sheet.getCell(`B${rowNumber-1}`).fill = { 
      type: 'pattern', 
      pattern: 'solid', 
      fgColor: { argb: XlsxConstants.SEC_ROW_COLOR }
    };
    addRichTextRow("RN: ", this.rnName, "Fix Version: ", this.fixVersion);
    mergeAndSet("RN Feature Name: ", this.featureName);
    mergeAndSet("RN Comments: ", this.rnComments);

    // -----------------------------
    // SUBTASKS
    // -----------------------------
    sheet.addRow([]);
    const subTaskStartRowNumber = sheet.rowCount;
    sheet.addRow(['1', 'Sub Tasks']);

    const uniqueDates: string[] = Array.from(
      new Set(this.subTasks.flatMap(st => st.workEntrys.map(e => e.entryDate)))
    ).sort((a, b) => b.localeCompare(a));

    // Header for subTasks
    const subHeader = ['SN', 'Sub Task Name', 'Remark(For Lead)', 'SubTask ETA', 'Progress Status', ...uniqueDates];
    sheet.addRow(subHeader);

    // Data rows
    this.subTasks.forEach(subTask => {
      const rowData = [
        subTask.subTaskSeq.toString(),
        subTask.subTaskName,
        subTask.remark,
        subTask.subTaskETA > 0 ? `${subTask.subTaskETA}h` : '',
        subTask.status,
        ...uniqueDates.map(date => {
          const entry = subTask.workEntrys.find(e => e.entryDate === date);
          return entry ? `${entry.entryHrs}h` : '';
        })
      ];
      sheet.addRow(rowData);
    });

    if(this.subTasks.length === 0){
      sheet.addRow(['', 'No Sub Tasks Available']);
    }

    // empty row for spacing
    sheet.addRow([]);

    // -----------------------------
    // OTHER SUBTASKS
    // -----------------------------
    const otherSubTaskStartRowNumber = sheet.rowCount;
    sheet.addRow(['2', 'Other Sub Tasks']);

    const uniqueOtherDates: string[] = Array.from(
      new Set(this.otherSubTasks.flatMap(st => st.workEntrys.map(e => e.entryDate)))
    ).sort((a, b) => b.localeCompare(a));

    // Header for otherSubTasks
    const otherSubHeader = ['SN', 'Other Sub Task Name', 'Remark(For Lead)', 'Progress Status', 'Resource', ...uniqueOtherDates];
    sheet.addRow(otherSubHeader);

    // Data rows
    this.otherSubTasks.forEach(subTask => {
      const rowData = [
        subTask.subTaskSeq.toString(),
        subTask.subTaskName,
        subTask.remark,
        subTask.status,
        subTask.workEntrys[0]?.firstName || '',
        ...uniqueOtherDates.map(date => {
          const entry = subTask.workEntrys.find(e => e.entryDate === date);
          return entry ? `${entry.entryHrs}h` : '';
        })
      ];
      sheet.addRow(rowData);
    });

    if(this.otherSubTasks.length === 0){
      sheet.addRow(['', 'No Other Sub Tasks Available']);
    }

    // -----------------------------
    // STYLING
    // -----------------------------
    sheet.eachRow((row, rowNumber) => {
      row.eachCell((cell, colNumber) => {
        cell.border = {
          top: { style: 'thin' },
          bottom: { style: 'thin' },
          left: { style: 'thin' },
          right: { style: 'thin' }
        };

        // Column B -> left align + wrapText
        if (colNumber === 2 || colNumber === 3) {
          cell.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true };
        } else {
          cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: false };
        }
      });

      // HEADER STYLING
      if (rowNumber === subTaskStartRowNumber + 1 || rowNumber === otherSubTaskStartRowNumber + 1 || rowNumber === headerRowNumber + 1) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: XlsxConstants.FIRSTROW_COLOR } };
          cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
        });
      } 
      else if (rowNumber === subTaskStartRowNumber + 2 || rowNumber === otherSubTaskStartRowNumber + 2) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: XlsxConstants.SEC_ROW_COLOR } };
          cell.font = { bold: true, color: { argb: '000000' } };
        });
      }

    });

    // -----------------------------
    // AUTO WIDTH
    // -----------------------------
    sheet.columns.forEach(col => {
      let maxLen = 10;
      if(col.eachCell){
        col.eachCell(cell => {
          const val = cell.value ? cell.value.toString() : '';
          maxLen = Math.max(maxLen, val.length + 4);
        });
      }
      col.width = Math.min(maxLen, 60);
    });

    // -----------------------------
    // EXPORT
    // -----------------------------
    const fileName = `Task_Seq. No. ${this.taskSeq}.xlsx`;
    workbook.xlsx.writeBuffer().then(buffer => saveAs(new Blob([buffer]), fileName));
  }

}