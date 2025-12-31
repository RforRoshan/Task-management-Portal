import { Component } from '@angular/core';
import { ApiResult, DropDownValueModel, EHrsLastDTO, GetAllDropDownValuesRequestModel, GetAllDropDownValuesResponseModel, GetAllTaskCommentRequestModel, GetAllTaskCommentResponseModel, GetSubTasksByIdRequestModel, GetSubTasksByIdResponseModel, GetTaskByIdRequestModel, GetTaskByIdResponseModel, GetTaskLogByIdRequestModel, GetTaskLogByIdResponseModel, RequestObject, ResourceNameModel, SubTaskModelE, TaskCommentModelE, TasksLog, WorkEntryDTOE } from '../../../models/common-model';
import { ActionConstants, AppRouteConstants, ClassColorMap, ColorMap, ControllerConstants, JiraColorMap, SessionStorageKeyConstants, StatusColorMap, URLConstants, XlsxConstants } from '../../../constants/constants';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DisplayHrsPipe } from '../../../pipes/display-hrs-pipe';
import { LeftETAPipe } from '../../../pipes/left-eta-pipe';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { Workbook } from 'exceljs';
import {saveAs} from 'file-saver';

@Component({
  selector: 'rks-done-sub-task',
  imports: [CommonModule, FormsModule, DisplayHrsPipe, LeftETAPipe],
  templateUrl: './done-sub-task.html',
  styleUrl: './done-sub-task.scss'
})
export class DoneSubTask {
  isFullyLoaded:boolean = false;

  classColorMap: Record<string, string> = ClassColorMap;
  colorMap: Record<string, string> = ColorMap;
  jiraColorMap: Record<string, string> = JiraColorMap;
  statusColorMap: Record<string, string> = StatusColorMap;

  isOpen = true;
  taskUniqueId: string = '';
  projectname: string = '';
  
  resourceNamesList: ResourceNameModel[] = [];
  resourceNamesListForLog: ResourceNameModel[] = [];
  rnAndFeatureList: DropDownValueModel[] = [];
  
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


  isOpenSelfSubTasks = true;
  isOpenOtherSubTasks = true;
  subTasks:SubTaskModelE[] = [];
  otherSubTasks:SubTaskModelE[] = [];
  otherTotalUsedHrs: EHrsLastDTO[] = [];

  isOpenComments = true;
  public Editor: any = ClassicEditor;
  public taskCommentsList: TaskCommentModelE[] = [];

  isOpenLogBox = false;
  tasksLogs:TasksLog[]=[];
  isAPINeedToCallForLogHistory:boolean= true;

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
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
          this.myComments = responseModel.responseData.myComments;
          this.managerComments = responseModel.responseData.managerComments;
          this.jira = responseModel.responseData.jira;


          this.totalETA = responseModel.responseData.totalETA;
          this.usedETA = responseModel.responseData.usedETA;
          this.otherUsedETA = responseModel.responseData.otherUsedETA;
          this.isApprove = responseModel.responseData.isApprove;

          this.rnGuidId = responseModel.responseData.rnGuidId;
          this.featureName = responseModel.responseData.featureName;
          this.fixVersion = responseModel.responseData.fixVersion;
          this.rnComments = responseModel.responseData.rnComments;
          this.mailTitled = responseModel.responseData.mailTitled;

          this.rnName = this.rnAndFeatureList.find(r => r.guidId === this.rnGuidId)?.value || '';
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
          this.rnAndFeatureList = responseModel.responseData.rnAndFeatureList;

          this.resourceNamesListForLog = [...this.resourceNamesList];
          this.resourceNamesList = this.resourceNamesList.filter(r => r.isShow);

          const today = new Date();
          // if(this.projectname == 'ccp' || this.projectname == 'plat'){
          //   today.setDate(today.getDate() - 1);
          // }
          const yyyy = today.getFullYear();
          const mm = String(today.getMonth() + 1).padStart(2, '0');
          const dd = String(today.getDate()).padStart(2, '0');
          const displayDate = `${yyyy}-${mm}-${dd}`;

          this.getTaskData(displayDate);
          this.getSubTaskData();
          this.getTaskCommentData();

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

  // ================Sub Task=========================

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

          this.subTasks = (responseModel.responseData.subTasks ?? []).map(st => {
            const mapped = Object.assign(new SubTaskModelE(), st);

            mapped.workEntrys = (st.workEntrys ?? []).map(w => {
              const entry = Object.assign(new WorkEntryDTOE(), w);
              entry.firstName = this.resourceNamesListForLog.find(r => r.userId === w.userId)?.name || '';
              return entry;
            });

            return mapped;
          });

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
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
      }
    });
  }


  getTaskCommentData():void {
    let requestObject: RequestObject<GetAllTaskCommentRequestModel> = new RequestObject<GetAllTaskCommentRequestModel>();
    let requestModel: GetAllTaskCommentRequestModel = new GetAllTaskCommentRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllTaskComment;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllTaskCommentResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
            (responseModel.responseData.taskComments ?? []).map(e => {
            let taskComment = new TaskCommentModelE();
            taskComment.oldComment = e.comment;
            Object.assign(taskComment, e);
            this.taskCommentsList.push(taskComment);
          });      
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
      }
    });
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

  callLogData():void{
    this.isOpenLogBox = !this.isOpenLogBox

    if(this.isAPINeedToCallForLogHistory){
      this.isAPINeedToCallForLogHistory = false;
      this.getLogData()
    }
  }

  openJiraInNewTab(jira: string) {
    window.open(URLConstants.Jira + jira, "_blank");
  }
  
  goToRNDetails(guidId: string): void {
    localStorage.setItem(SessionStorageKeyConstants.RNName, this.rnName || '' );
    this.router.navigateByUrl(AppRouteConstants.RNFeature.Detalis(this.projectname, guidId));
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
      sheet.getRow(rowNumber).height = Math.ceil((label.length + text.length)  / 45) * 12; 
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
