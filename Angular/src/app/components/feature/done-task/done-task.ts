import { Component } from '@angular/core';
import { ApiResult, DoneTaskModel, GetAllDoneTasksRequestModel, GetAllDoneTasksResponseModel, GetAllResourceNamesRequestModel, GetAllResourceNamesResponseModel, MoveTaskRequestModel, MoveTaskResponseModel, RequestObject } from '../../../models/common-model';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { ActionConstants, AppRouteConstants, ClassColorMap, ColorMap, ControllerConstants, JiraColorMap, SessionStorageKeyConstants, StatusColorMap, URLConstants } from '../../../constants/constants';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DisplayHrsPipe } from '../../../pipes/display-hrs-pipe';
import { Workbook } from 'exceljs';
import {saveAs} from 'file-saver';

@Component({
  selector: 'rks-done-task',
  imports: [CommonModule, FormsModule, DisplayHrsPipe],
  templateUrl: './done-task.html',
  styleUrl: './done-task.scss'
})
export class DoneTask {
  isLoading:boolean = false;
  isFullyLoaded:boolean = false;
  projectname: string = '';
  doneTasks:DoneTaskModel[] = [];
  resourceNameMap: { [key: string]: string } = {};
  classColorMap: Record<string, string> = ClassColorMap;
  colorMap: Record<string, string> = ColorMap;
  jiraColorMap: Record<string, string> = JiraColorMap;
  statusColorMap: Record<string, string> = StatusColorMap;

  constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }
  
  ngOnInit():void {
    this.getTaskData();
  }

  getTaskData():void {

    let requestObject: RequestObject<GetAllDoneTasksRequestModel> = new RequestObject<GetAllDoneTasksRequestModel>();
    let requestModel: GetAllDoneTasksRequestModel = new GetAllDoneTasksRequestModel();
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllDoneTasks;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllDoneTasksResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          this.doneTasks = responseModel.responseData.tasks;
          //console.log(this.doneTasks);

          this.getResourceNamesData();
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
          
          
          this.resourceNameMap = Object.fromEntries((responseModel.responseData.resourceNames ?? []).map(r => [r.userName, r.name]));
          this.isFullyLoaded = true;

          //console.log(this.resourceNameMap);
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
  this.alertService.showConfirmation('Do you want to move this task to current!', 'Yes, unmove it!').then((confirmed) => {
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
    requestModel.IsAlive = true;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.MoveTask;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<MoveTaskResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {          
          this.doneTasks = this.doneTasks.filter(t => t.taskUniqueId !== taskUniqueId);
          this.alertService.showSuccess("Task moved to current successfully.");
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

  goToTask(taskUniqueId: string): void {
    this.router.navigateByUrl(AppRouteConstants.Task.DoneTaskById(this.projectname, taskUniqueId));
  }
  
  
  //Table start
  searchTerm = '';

  get filteredTasks() {
    return this.doneTasks.filter(task =>
      (task.taskName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.jira.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.myComments.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.managerComments.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.network.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.userName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.status.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      task.subProject.toLowerCase().includes(this.searchTerm.toLowerCase()))
    );
  }
 //Table end

  openJiraInNewTab(jira: string) {
    window.open(URLConstants.Jira + jira, "_blank");
  }

  exportToExcel(): void {

    const exportData = this.doneTasks.map(item => ({
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
      'RN': item.rnName,
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
    const sheet = workbook.addWorksheet('Done Tasks');

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

        if ([3, 14, 15, 16].includes(colNumber)) {
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

    const fileName = `DoneTask_${day}-${month}-${year}-${formattedHours}-${minutes}-${ampm}.xlsx`;

    // 👉 Export
    workbook.xlsx.writeBuffer().then(buffer => {
      saveAs(new Blob([buffer]), fileName);
    });
  } 

}
