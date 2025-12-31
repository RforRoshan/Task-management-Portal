import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService } from '../../../services/http-service';
import { AlertService } from '../../../services/alert-service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ApiResult, GetAllTasksByRNGuidIdRequestModel, GetAllTasksByRNGuidIdResponseModel, RequestObject, TaskForRNDTO } from '../../../models/common-model';
import { ActionConstants, AppRouteConstants, ColorMap, ControllerConstants, JiraColorMap, SessionStorageKeyConstants, URLConstants, XlsxConstants } from '../../../constants/constants';
import { Workbook } from 'exceljs';
import {saveAs} from 'file-saver';

@Component({
  selector: 'rks-rnfeature-details',
  imports: [CommonModule, FormsModule],
  templateUrl: './rnfeature-details.html',
  styleUrl: './rnfeature-details.scss'
})
export class RNFeatureDetails {
  isFullyLoaded:boolean = false;
  guidId:string = '';
  rnName: string = localStorage.getItem(SessionStorageKeyConstants.RNName) ?? '';
  projectname:string = '';
  tasks: TaskForRNDTO[] = [];
  jiraColorMap: Record<string, string> = JiraColorMap;
  colorMap: Record<string, string> = ColorMap;
  
 constructor(private router: Router, private route: ActivatedRoute, private httpService: HttpService, private alertService: AlertService) {
    let guidId = this.route.snapshot.paramMap.get('guidId');
    this.guidId = guidId!;
    const projectname = this.route.parent?.snapshot.data['projectname'];
    this.projectname = projectname!;
  }

  ngOnInit():void {
    this.getData();
  }

  getData():void {

    let requestObject: RequestObject<GetAllTasksByRNGuidIdRequestModel> = new RequestObject<GetAllTasksByRNGuidIdRequestModel>();
    let requestModel: GetAllTasksByRNGuidIdRequestModel = new GetAllTasksByRNGuidIdRequestModel();
    requestModel.projectKey = this.projectname;
    requestModel.guidId = this.guidId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.GetAllTasksByRNGuidId;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<GetAllTasksByRNGuidIdResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.tasks = responseModel.responseData.tasks ?? [];
          this.isFullyLoaded = true;

          //console.log(this.tasks);
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

  openJiraInNewTab(jira: string) {
    window.open(URLConstants.Jira + jira, "_blank");
  }

  goToTask(tasks: TaskForRNDTO): void {
    if(tasks.isAlive){
      this.router.navigateByUrl(AppRouteConstants.Task.CurrentTaskById(this.projectname, tasks.taskUniqueId));
    }
    else{
      this.router.navigateByUrl(AppRouteConstants.Task.DoneTaskById(this.projectname, tasks.taskUniqueId));
    }
    
  }


  exportToExcel(): void {

    const workbook = new Workbook();
    const sheet = workbook.addWorksheet(`RN_${this.rnName}`);

    sheet.addRow([]);
    sheet.addRow(['', '', `Tasks for Release Note: ${this.rnName}`, '', '', '']);
    sheet.addRow(['Sn. No.', 'Jira ID', 'Feature Name', 'Fix Version', 'RN Comments', 'Resource']);

    // 👉 Add all data rows
    let sn = 1;
    this.tasks.forEach(
      task => 
        sheet.addRow([sn++, task.jira, task.featureName, task.fixVersion, task.rnComments, task.firstName])
    );

    // 👉 Apply styling
    sheet.eachRow((row, rowNumber) => {
      row.eachCell((cell, colNumber) => {
        cell.border = {
          top: { style: 'thin' },
          bottom: { style: 'thin' },
          left: { style: 'thin' },
          right: { style: 'thin' }
        };

        if ([3, 5].includes(colNumber)) {
          cell.alignment = { vertical: 'middle', horizontal: 'left', wrapText: true };
        } 
        else {
          cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: false };
        }
      });

      // HEADER STYLING
      if (rowNumber === 2) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: XlsxConstants.FIRSTROW_COLOR } };
          cell.font = { bold: true, color: { argb: 'FFFFFFFF' } };
        });
      } 
      else if (rowNumber === 3) {
        row.eachCell(cell => {
          cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: XlsxConstants.SEC_ROW_COLOR } };
          cell.font = { bold: true, color: { argb: '000000' } };
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

    // 👉 Export
    workbook.xlsx.writeBuffer().then(buffer => {
      saveAs(new Blob([buffer]), `RN_${this.rnName}.xlsx`);
    });
  }


  
}
