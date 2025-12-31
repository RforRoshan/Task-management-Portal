import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AddTaskRequestModel, AddTaskResponseModel, ApiResult, CurrentTaskModelE, DropDownValueModel, GetAllCurrentTasksRequestModel, GetAllCurrentTasksResponseModel, GetAllDropDownValuesRequestModel, GetAllDropDownValuesResponseModel, MiniUpdateTaskRequestModel, MiniUpdateTaskResponseModel, MoveTaskRequestModel, MoveTaskResponseModel, RequestObject, ResourceNameModel} from '../../../models/common-model';
import { ActionConstants, AppRouteConstants, ClassColorMap, ColorMap, ControllerConstants, JiraColorMap, NumberColorMap, SessionStorageKeyConstants, StatusColorMap, URLConstants } from '../../../constants/constants';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { DisplayHrsPipe } from "../../../pipes/display-hrs-pipe";
import { DisplayInputService } from '../../../services/display-input-service';
//import {WorkSheet, utils, WorkBook, writeFile} from 'xlsx-js-style';
import { Workbook } from 'exceljs';
import {saveAs} from 'file-saver';

@Component({
  selector: 'rks-current-task',
  imports: [CommonModule, FormsModule, DisplayHrsPipe],
  templateUrl: './current-task.html',
  styleUrl: './current-task.scss'
})
export class CurrentTask {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  isManager: boolean = ['101', '102'].includes(localStorage.getItem(SessionStorageKeyConstants.RoleId) || '');
  projectname: string = '';
  classColorMap: Record<string, string> = ClassColorMap;
  colorMap: Record<string, string> = ColorMap;
  jiraColorMap: Record<string, string> = JiraColorMap;
  statusColorMap: Record<string, string> = StatusColorMap;
  numberColorMap = NumberColorMap;
  projectId: number = 0;
  resourceNamesList: ResourceNameModel[] = [];
  statusList: DropDownValueModel[] = [];
  categoryList: DropDownValueModel[] = [];
  projectList: DropDownValueModel[] = [];
  networkList: DropDownValueModel[] = [];
  rNAndFeatureListMap: { [key: string]: string } = {};
  currentTasks:CurrentTaskModelE[] = [];
  resourceNameMap: { [key: string]: string } = {};
  distinctStatuses: string[] = [];
  distinctUserNames: string[] = [];
  distinctNetworkNames: string[] = [];
  distinctCategoryNames: string[] = [];

  filterStatusValue:string = '';
  filterUserNameValue:string = ''
  filterNetworkValue:string = ''
  filterCategoryValue:string = ''

  displayDate:string = '';

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService, private displayInputService:DisplayInputService) {
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  ngOnInit():void {
    this.getDropDownData();
    let projectId:string = localStorage.getItem(SessionStorageKeyConstants.ProjectId) || '0';
    this.projectId = parseInt(projectId);
  }

  getDropDownData():void {
    //this.isLoading = true;

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
          this.resourceNameMap = Object.fromEntries((this.resourceNamesList ?? []).map(r => [r.userName, r.name]));
          this.resourceNamesList = this.resourceNamesList.filter(r => r.isShow);

          this.rNAndFeatureListMap = Object.fromEntries(( responseModel.responseData.rnAndFeatureList ?? []).map(r => [r.guidId, r.value]));

          //this.isLoading = false;


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

  getTaskData(displayDate:string):void {

    if(displayDate == '' || displayDate == null || displayDate == undefined || displayDate.length < 7){
      return;
    }
    //this.isLoading = true;

    let requestObject: RequestObject<GetAllCurrentTasksRequestModel> = new RequestObject<GetAllCurrentTasksRequestModel>();
    let requestModel: GetAllCurrentTasksRequestModel = new GetAllCurrentTasksRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.displayDate = displayDate;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllCurrentTasks;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllCurrentTasksResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          this.currentTasks = [];
          (responseModel.responseData.tasks ?? []).map(task => {
            let currentTask = new CurrentTaskModelE();
            Object.assign(currentTask, task);
            this.currentTasks.push(currentTask);
          });

          this.advanceOptions();

          //console.log(this.currentTasks);
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

  moveTaskButton(taskUniqueId:string):void {
    this.alertService.showConfirmation('Do you want to move this task to done!', 'Yes, move it!').then((confirmed) => {
      if (confirmed) {
        this.moveTask(taskUniqueId);
      }
    });
  }

  moveTask(taskUniqueId: string):void {
    this.isLoading = true;

    let requestObject: RequestObject<MoveTaskRequestModel> = new RequestObject<MoveTaskRequestModel>();
    let requestModel: MoveTaskRequestModel = new MoveTaskRequestModel();
    requestModel.taskUniqueId = taskUniqueId;
    requestModel.IsAlive = false;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.MoveTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<MoveTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {          
          this.currentTasks = this.currentTasks.filter(t => t.taskUniqueId !== taskUniqueId);
          this.alertService.showSuccess("Task moved to done successfully.");
          this.advanceOptions();
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

  updateTask(currentTasks:CurrentTaskModelE):void {
    this.isLoading = true;

    let requestObject: RequestObject<MiniUpdateTaskRequestModel> = new RequestObject<MiniUpdateTaskRequestModel>();
    let requestModel: MiniUpdateTaskRequestModel = new MiniUpdateTaskRequestModel();
    requestModel.taskUniqueId = currentTasks.taskUniqueId;
    requestModel.todayDayWork = currentTasks.todayDayWork;
    requestModel.eHrsToday = currentTasks.eHrsToday;
    requestModel.itemToDiscuss = currentTasks.itemToDiscuss;
    requestModel.managerComments = currentTasks.managerComments;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.MiniUpdateTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<MiniUpdateTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          currentTasks.isEditing = false;
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

  addTask():void {

    this.displayInputService.showAddNewCurrentTaskInputBox(this.resourceNamesList, this.statusList, this.categoryList, this.projectList, this.networkList).then((result)=>{
      if(result){
        
        this.isLoading = true;

        let requestObject: RequestObject<AddTaskRequestModel> = new RequestObject<AddTaskRequestModel>();
        let requestModel: AddTaskRequestModel = new AddTaskRequestModel();
        
        requestModel.catagory = result.catagory;
        requestModel.subProject = result.subProject;
        requestModel.network = result.network;
        requestModel.status = result.status;
        requestModel.userName = result.userName;
        requestModel.userId = this.resourceNamesList.find(r => r.userName === requestModel.userName)?.userId || 0;
        requestModel.jira = result.jira;
        requestModel.taskName = result.taskName;
        requestModel.itemToDiscuss = result.itemToDiscuss;
        requestModel.myComments = result.myComments;
        requestModel.projectId = this.projectId;
        requestModel.projectKey = this.projectname;

        requestObject.controller = ControllerConstants.Task;
        requestObject.action = ActionConstants.AddTask;
        requestObject.requestModel = requestModel;

        this.httpService.post(requestObject).subscribe({
          next: (response) => {
            let responseModel: ApiResult<AddTaskResponseModel> = response;
            if(responseModel.status  && responseModel.responseData != null) {

              let currentTask = new CurrentTaskModelE();
              Object.assign(currentTask, responseModel.responseData);
              this.currentTasks.push(currentTask);
              
              this.advanceOptions();
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

  advanceOptions():void{
    this.distinctStatuses = [ ...new Set((this.currentTasks ?? []).map(t => t.status))];
    this.distinctUserNames = [ ...new Set((this.currentTasks ?? []).map(t => t.userName))];
    this.distinctNetworkNames = [ ...new Set((this.currentTasks ?? []).map(t => t.network))];
    this.distinctCategoryNames = [ ...new Set((this.currentTasks ?? []).map(t => t.catagory))];
  }

  clearAllFilter():void{
    this.filterStatusValue = '';
    this.filterUserNameValue = '';
    this.filterNetworkValue = '';
    this.filterCategoryValue = '';
    this.searchTerm = '';
    this.lastDayWorkFilter=false;
    this.itemToDiscussFilter=false;
    this.todayDayWorkFilter=false;
  }

  openJiraInNewTab(jira: string) {
    window.open(URLConstants.Jira + jira, "_blank");
  }

  managerFilter():void{
    this.lastDayWorkFilter=true;
    this.itemToDiscussFilter=true;
    //this.todayDayWorkFilter=true;
  }

  statusFilter(status:string):void{
    this.filterStatusValue = status;
  }

  usernameFilter(userName:string):void{
    this.filterUserNameValue = userName;
  }

  networkNameFilter(networkName:string):void{
    this.filterNetworkValue = networkName;
  }

  categoryFilter(categoryName:string):void{
    this.filterCategoryValue = categoryName;
  }

  goToTask(taskUniqueId: string): void {
    this.router.navigateByUrl(AppRouteConstants.Task.CurrentTaskById(this.projectname, taskUniqueId));
  }

  goToRNDetails(guidId: string): void {
    localStorage.setItem(SessionStorageKeyConstants.RNName, this.rNAndFeatureListMap[guidId] || '' );
    this.router.navigateByUrl(AppRouteConstants.RNFeature.Detalis(this.projectname, guidId));
  }

  //Table start
  searchTerm = '';

  lastDayWorkFilter:boolean=false;
  itemToDiscussFilter:boolean=false;
  todayDayWorkFilter:boolean=false;

  get filteredTasks() {
    return this.currentTasks.filter(task =>
      (task.taskName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.jira.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.myComments.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.managerComments.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.network.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.userName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.status.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.subProject.toLowerCase().includes(this.searchTerm.toLowerCase())) &&
      ((this.lastDayWorkFilter && task.lastDayWork) || !this.lastDayWorkFilter) &&
      ((this.itemToDiscussFilter && task.itemToDiscuss) || !this.itemToDiscussFilter) &&
      ((this.todayDayWorkFilter && task.todayDayWork) || !this.todayDayWorkFilter) && 
      ((this.filterStatusValue === task.status) || this.filterStatusValue === '') && 
      ((this.filterUserNameValue === task.userName) || this.filterUserNameValue === '')&& 
      ((this.filterNetworkValue === task.network) || this.filterNetworkValue === '') &&
      ((this.filterCategoryValue === task.catagory) || this.filterCategoryValue === '')
    );
  }
 //Table end

  exportToExcel(): void {

    const exportData = this.currentTasks.map(item => ({
      'Seq No': item.taskSeq,
      'Category': item.catagory,
      'Task Name': item.taskName,
      'Resource': this.resourceNameMap[item.userName] || 'UnAssigned',
      'Project': item.subProject,
      'Network': item.network,
      'Status': item.status,
      'ETA': item.totalETA > 0 ? `${item.totalETA}h` : '',
      'Used ETA': item.usedETA > 0 ? `${item.usedETA}h` : '',
      'Other Used ETA': item.otherUsedETA > 0 ? `${item.otherUsedETA}h` : '',
      'Jira': item.jira,
      'Last Day Work': item.lastDayWork ? 'Yes' : 'No',
      'Item To Discuss': item.itemToDiscuss ? 'Yes' : 'No',
      'Today Day Work': item.todayDayWork ? 'Yes' : 'No',
      'RN': this.rNAndFeatureListMap[item.rnGuidId] || '',
      'Fix Version': item.fixVersion,
      'Comments': item.myComments,
      'Manager Comments': item.managerComments,
      'EMail Titled': item.mailTitled,
      'EMailId': item.userName,
    }));

    // 🎨 COLOR CONSTANTS
    const HEADER_COLOR = '61adbf';  // Aqua Accent 4 (header)
    const ALT_ROW_COLOR = 'DFEEF2'; // 80% lighter aqua

    const workbook = new Workbook();
    const sheet = workbook.addWorksheet('Tasks');

    // 👉 Add header manually (Object keys)
    const headerKeys = Object.keys(exportData[0]);
    sheet.addRow(headerKeys);

    // 👉 Add all data rows
    exportData.forEach(row => sheet.addRow(Object.values(row)));

    // 👉 Apply styling
    sheet.eachRow((row, rowNumber) => {
      row.eachCell((cell, colNumber) => {
        cell.border = {
          top: { style: 'thin' },
          bottom: { style: 'thin' },
          left: { style: 'thin' },
          right: { style: 'thin' }
        };

        if ([3, 17, 18, 19].includes(colNumber)) {
          cell.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true };
        } 
        else {
          cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: false };
        }
      });

      // HEADER STYLING
      if (rowNumber === 1) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: HEADER_COLOR } };
          cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
        });
      } 
      else if (rowNumber % 2 === 0) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: ALT_ROW_COLOR } };
            cell.font = { bold: false, color: { argb: '000000' } };
        });
      }
    });

    // 👉 Auto width
    sheet.columns.forEach((col) => {
      let maxLen = 10;
      if (col.eachCell) {
        col.eachCell((cell) => {
          const val = cell.value ? cell.value.toString() : '';
          maxLen = Math.max(maxLen, val.length + 4);
        });
      }
      col.width = Math.min(maxLen, 60);
    });

    // 👉 Add autofilter
    sheet.autoFilter = {
      from: { row: 1, column: 1 },
      to: { row: 1, column: headerKeys.length }
    };

    // 👉 File name
    const today = new Date();
    const day: string = String(today.getDate()).padStart(2, "0");
    const month: string = String(today.getMonth() + 1).padStart(2, "0");
    const year: number = today.getFullYear();

    let hours: number = today.getHours();
    const minutes: string = String(today.getMinutes()).padStart(2, "0");

    const ampm: string = hours >= 12 ? "PM" : "AM";
    hours = hours % 12 || 12;
    const formattedHours: string = String(hours).padStart(2, "0");

    const fileName = `CurrentTask_${day}-${month}-${year}-${formattedHours}-${minutes}-${ampm}.xlsx`;

    // 👉 Export
    workbook.xlsx.writeBuffer().then(buffer => {
      saveAs(new Blob([buffer]), fileName);
    });
  }
//   exportToExcel(): void {

//   const exportData = this.currentTasks.map(item => ({
//     'Seq No': item.taskSeq,
//     'Category': item.catagory,
//     'Task Name': item.taskName,
//     'Resource': this.resourceNameMap[item.userName] || 'UnAssigned',
//     'Project': item.subProject,
//     'Network': item.network,
//     'Status': item.status,
//     'ETA': item.totalETA > 0 ? `${item.totalETA}h`: '',
//     'Used ETA': item.usedETA > 0 ? `${item.usedETA}h`: '',
//     'Other Used ETA': item.otherUsedETA > 0 ? `${item.otherUsedETA}h`: '',
//     'Comments': item.myComments,
//     'Manager Comments': item.managerComments,
//     'Jira': item.jira,
//     'Last Day Work': item.lastDayWork ? 'Yes' : 'No',
//     'Item To Discuss': item.itemToDiscuss ? 'Yes' : 'No',
//     'Today Day Work': item.todayDayWork ? 'Yes' : 'No',
//     'RN': this.rNAndFeatureListMap[item.rnGuidId] || '',
//     'Fix Version': item.fixVersion,
//     'EMail Titled': item.mailTitled,
//     'EMailId': item.userName,
//   }));

//   const worksheet: WorkSheet = utils.json_to_sheet(exportData);
//   const range = utils.decode_range(worksheet['!ref']!);

//   // 🎨 COLOR CONSTANTS
//   const headerFill = { fgColor: { rgb: "61adbf" } };     // header
//   const headerFont = { bold: true, color: { rgb: "FFFFFF" } }; // White text
//   const altRowFill = { fgColor: { rgb: "DFEEF2" } };     //  alternate row

//   for (let R = range.s.r; R <= range.e.r; ++R) {
//     for (let C = range.s.c; C <= range.e.c; ++C) {

//       const cellRef = utils.encode_cell({ r: R, c: C });
//       if (!worksheet[cellRef]) continue;

//       let fillStyle = undefined;

//       // 🟦 HEADER ROW (R === 0)
//       if (R === 0) {
//         fillStyle = headerFill;
//         worksheet[cellRef].s = {
//           alignment: { wrapText: false, vertical: 'center', horizontal: 'left' },
//           border: {
//             top: { style: 'thin' },
//             bottom: { style: 'thin' },
//             left: { style: 'thin' },
//             right: { style: 'thin' }
//           },
//           font: headerFont,
//           fill: fillStyle
//         };
//         continue;
//       }

//       // 🎨 ALTERNATE ROW COLOR (even rows only)
//       if (R % 2 === 0) {
//         fillStyle = altRowFill;
//       }

//       worksheet[cellRef].s = {
//         alignment: {
//           wrapText: false,
//           vertical: 'center',
//           horizontal: 'left'
//         },
//         border: {
//           top: { style: 'thin' },
//           bottom: { style: 'thin' },
//           left: { style: 'thin' },
//           right: { style: 'thin' }
//         },
//         ...(fillStyle ? { fill: fillStyle } : {})
//       };
//     }
//   }

//   // AUTO COLUMN WIDTH
//   const colWidths: number[] = [];

//   exportData.forEach(row => {
//     Object.values(row).forEach((value, colIndex) => {
//       const cellLength = value ? value.toString().length : 10;
//       colWidths[colIndex] = Math.max(colWidths[colIndex] || 10, cellLength + 4);
//     });
//   });

//   worksheet['!cols'] = colWidths.map(width => ({ wch: Math.min(width, 60) }));

//   // FILTER
//   worksheet['!autofilter'] = { ref: worksheet['!ref']! };

//   const today = new Date();
//   const dd = String(today.getDate()).padStart(2, '0');
//   const mm = String(today.getMonth() + 1).padStart(2, '0');
//   const yy = today.getFullYear();
//   const tt = String(today.getHours()).padStart(2, '0') + '_' + String(today.getMinutes()).padStart(2, '0');

//   const sheetName = `CurrentTask_${dd}_${mm}_${yy}_${tt}`;

//   const workbook: WorkBook = {
//     Sheets: { [sheetName]: worksheet },
//     SheetNames: [sheetName]
//   };

//   writeFile(workbook, `${sheetName}.xlsx`);
// }


}
