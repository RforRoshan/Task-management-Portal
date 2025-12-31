import { Component, Input } from '@angular/core';
import { HttpService } from '../../../../services/http-service';
import { AlertService } from '../../../../services/alert-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { AddTaskCommentRequestModel, AddTaskCommentResponseModel, ApiResult, DeleteTaskCommentRequestModel, DeleteTaskCommentResponseModel, GetAllTaskCommentRequestModel, GetAllTaskCommentResponseModel, RequestObject, TaskCommentModel, TaskCommentModelE, UpdateTaskCommentRequestModel, UpdateTaskCommentResponseModel } from '../../../../models/common-model';
import { ActionConstants, ColorMap, ControllerConstants } from '../../../../constants/constants';

@Component({
  selector: 'rks-task-comments',
  imports: [CommonModule, FormsModule, CKEditorModule],
  templateUrl: './task-comments.html',
  styleUrl: './task-comments.scss'
})
export class TaskComments {

  @Input({ required: true }) taskUniqueId!: string;
  @Input({ required: true }) projectname!: string;
  colorMap: Record<string, string> = ColorMap;
  isOpen = true;

  commentText: string = '';
  public Editor: any = ClassicEditor;
  public taskComments: TaskCommentModelE[] = [];

  constructor(private httpService: HttpService, private alertService: AlertService) {}

  ngOnInit():void {
    this.getData();
  }

  updateButton(taskComment:TaskCommentModelE):void{ 
    if (taskComment.comment != taskComment.oldComment) {
      this.updateComment(taskComment);
    }
  }

  cancelUpdateComment(taskCommentModel:TaskCommentModelE):void{ 
    taskCommentModel.comment = taskCommentModel.oldComment;
    taskCommentModel.isEdited = false;
  }
  
  addButton() {
    if (this.commentText.trim()) {
      this.addNewComment();
    }
  }

  CancelComment() {
    this.commentText = '';
  }

  deleteButton(taskCommentModel:TaskCommentModelE):void{
    this.alertService.showConfirmation(`Do you want to delete comment!`, 'Yes, delete it!').then((confirmed) => {
        if (confirmed) {
          this.deleteComment(taskCommentModel);
        }
      });
  }


  getData():void {
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
            this.taskComments.push(taskComment);
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



  
  addNewComment():void{
    let requestObject: RequestObject<AddTaskCommentRequestModel> = new RequestObject<AddTaskCommentRequestModel>();
    let requestModel: AddTaskCommentRequestModel = new AddTaskCommentRequestModel();
    requestModel.taskUniqueId = this.taskUniqueId;
    requestModel.comment = this.commentText;
    requestModel.projectKey = this.projectname;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.AddTaskComment;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<AddTaskCommentResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          let taskComment:TaskCommentModelE = new TaskCommentModelE();
          taskComment.guidId = responseModel.responseData.guidId;
          taskComment.firstName = responseModel.responseData.firstName;
          taskComment.lastName = responseModel.responseData.lastName;
          taskComment.commentedOn = responseModel.responseData.commentedOn;
          taskComment.comment = this.commentText;
          taskComment.oldComment = this.commentText;
          taskComment.isSameUserComment = true;

          this.taskComments.unshift(taskComment);
          this.commentText = '';
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
      }
    });
  }

  updateComment(taskComment:TaskCommentModelE):void{
    let requestObject: RequestObject<UpdateTaskCommentRequestModel> = new RequestObject<UpdateTaskCommentRequestModel>();
    let requestModel: UpdateTaskCommentRequestModel = new UpdateTaskCommentRequestModel();
    requestModel.guidId = taskComment.guidId;
    requestModel.comment = taskComment.comment;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.UpdateTaskComment;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<UpdateTaskCommentResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {
          
          
          taskComment.isPreviouslyEdited = responseModel.responseData.isPreviouslyEdited;
          taskComment.commentEditedOn = responseModel.responseData.commentEditedOn;
          taskComment.oldComment = taskComment.comment;
          taskComment.isEdited = false;
        }
        else {
          this.alertService.showError(response.message);
        }
      },
      error: (error) => {
      }
    });
  }

  deleteComment(taskComment:TaskCommentModelE):void{
    
    let requestObject: RequestObject<DeleteTaskCommentRequestModel> = new RequestObject<DeleteTaskCommentRequestModel>();
    let requestModel: DeleteTaskCommentRequestModel = new DeleteTaskCommentRequestModel();
    requestModel.guidId = taskComment.guidId;

    requestObject.controller = ControllerConstants.Task;
    requestObject.action = ActionConstants.DeleteTaskComment;
    requestObject.requestModel = requestModel;

    this.httpService.post(requestObject).subscribe({
      next: (response) => {
        let responseModel: ApiResult<DeleteTaskCommentResponseModel> = response;
        if(responseModel.status  && responseModel.responseData != null) {

          this.taskComments = this.taskComments.filter(l => l !== taskComment);
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