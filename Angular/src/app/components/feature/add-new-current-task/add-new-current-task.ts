import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DropDownValueModel, ResourceNameModel } from '../../../models/common-model';

@Component({
  selector: 'rks-add-new-current-task',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-new-current-task.html',
  styleUrl: './add-new-current-task.scss'
})
export class AddNewCurrentTask {
  @Input() resourceNamesList: ResourceNameModel[] = [];
  @Input() statusList: DropDownValueModel[] = [];
  @Input() categoryList: DropDownValueModel[] = [];
  @Input() projectList: DropDownValueModel[] = [];
  @Input() networkList: DropDownValueModel[] = [];
  
  userName: string = '';
  catagory: string = '';
  taskName: string = '' ;
  subProject: string = '';
  network: string = '';
  status: string = ''; 
  myComments: string = '';
  jira: string = 'N.A.';
  itemToDiscuss:boolean = false;

  ngOnInit(){
    this.catagory = this.categoryList[0]?.value || '';
    this.subProject = this.projectList[0]?.value || '';
    this.network = this.networkList[0]?.value || '';
    this.status = this.statusList[0]?.value || '';
    this.userName = 'UnAssigned';
  }


  @Output() closed = new EventEmitter<{userName : string; catagory : string; taskName : string; subProject : string; network : string; status : string; myComments: string; jira: string; itemToDiscuss:boolean;} | null>();

  onCancel() {
    this.closed.emit(null);
  }

  onSubmit() {
    this.closed.emit({userName:this.userName, catagory:this.catagory, taskName:this.taskName, subProject:this.subProject, network:this.network, status:this.status, myComments:this.myComments,jira:this.jira, itemToDiscuss:this.itemToDiscuss});
  }
}


