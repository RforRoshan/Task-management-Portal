//============================================================
export class RequestObject<T> {
  controller: string = '';
  action: string = '';
  requestModel!: T;
}

export class ApiResult<T> {
	errorCode: string = '';
	message: string = '';
	responseData!: T;
	status: boolean = false;
}
//============================================================

export class LoginRequestModel {
  emailId: string = '';
  password: string = '';
  source: string = 'Web';
}

export class LoginResponseModel {
  token: string = '';
  isFirstTimeLogin: boolean= false;
}
//============================================================

export class RegisterRequestModel {
  username: string = '';
  firstName: string = '';
  middleName: string = '';
  lastName: string = '';
  employeeNumber: number = 0;
  email: string = '';
  iP: string = '';
  accessDetails:AccessRegisterModel[] = [];
}

export class RegisterResponseModel {
  status: boolean = false;
  message: string = '';
}

export class AccessRegisterModel{
  roleId: number = 0;
  projectId: number = 0;
}

export class ProjectAccessView{
  projectId:number = 0;
  projectKey : string = '';
  projectName: string = '';
  isSelected: boolean = false;
  roleId:number = 0;
}
//============================================================

export class GetAllRoleByAccessRequestModel {
  projectKey: string = '';
}

export class GetAllRoleByAccessResponseModel {
  roles: RoleModel[] = [];
}

export class RoleModel {
  roleId: number = 0;
  roleName: string = '';
}
//============================================================
export class GetProjectAccessRequestModel {
}

export class GetProjectAccessResponseModel{
    projectAccess:ProjectAccess[] = [];
}
export class ProjectAccess{
  projectId:number = 0;
  projectKey : string = '';
  projectName: string = '';
  description: string = '';
}
//============================================================

export class GetDashBoardDataRequestModel {
  projectKey: string = '';
}

export class UserDetails{
  username: string = '';
  firstName: string = '';
  middleName: string = '';
  lastName: string = ''
  email: string = '';
  userRoleId: number = 0;
  userId: number = 0
  roleId: number = 0;
  projectId: number = 0;
}
//============================================================
export class ForgotPasswardLinkGenerateRequestModel {
  emailId: string = '';
  linkPrefix: string = '';
}
export class ForgotPasswardLinkGenerateResponseModel {
  isEmailExist: boolean = false;
}
//============================================================
export class ForgotPasswardRequestModel {
  token: string = '';
  password: string = '';
  source: string = '';

}
export class ForgotPasswardResponseModel {
  status: boolean = false;
}
//============================================================

export class FirstTimeUserRequestModel {
  token: string = '';
  password: string = '';
  source: string = 'Web';
}

export class FirstTimeUserResponseModel {
  token: string = '';
}
//============================================================
export class GetAllDropDownValuesRequestModel {
  projectKey: string = '';
}

export class GetAllDropDownValuesResponseModel {
  resourceNames: ResourceNameModel[] = [];
  status: DropDownValueModel[] = [];
  category: DropDownValueModel[] = [];
  project: DropDownValueModel[] = [];
  network: DropDownValueModel[] = [];
  rnAndFeatureList: DropDownValueModel[] = [];
}

export class ResourceNameModel{
  guidId: string = '';
  userId: number = 0;
  userName:string = '';
  name:string = '';
  lastName:string = '';
  isShow:boolean = true;
}

export class DropDownValueModel{
  guidId: string = '';
  value: string = '';
}
//============================================================
export class GetAllCurrentTasksRequestModel{
  projectKey: string = '';
  displayDate: string  = '';
}

export class GetAllCurrentTasksResponseModel {
    tasks: CurrentTaskModel[] = [];
}

export class CurrentTaskModel{
  userId: number = 0;
  userName: string = 'UnAssigned';
  taskUniqueId: string = '';
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
  isApprove: boolean = false;
  rnGuidId: string = '00000000-0000-0000-0000-000000000000';
  fixVersion: string = '';
  mailTitled: string = '';
}

export class EHrsLastDTO
{
    firstName: string = '';
    worksHrs: number = 0;
}

export class CurrentTaskModelE extends CurrentTaskModel{
  isEditing: boolean = false;
}
//============================================================
export class GetAllDoneTasksRequestModel{
  projectKey: string = '';
}


export class GetAllDoneTasksResponseModel {
    tasks: DoneTaskModel[] = [];
}

export class DoneTaskModel{
  userId: number = 0;
  userName: string = '';
  taskUniqueId: string = '';
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
  rnName: string = '';
  fixVersion: string = '';
  mailTitled: string = '';
}

//============================================================
export class GetTaskByIdRequestModel{
  taskUniqueId: string = '';
  displayDate: string  = '';
}

export class GetTaskByIdResponseModel{
  userId: number = 0;
  userName: string = 'UnAssigned';
  taskUniqueId: string = '';
  taskSeq: number = 0;
  catagory: string = '';
  taskName: string = '' ;
  subProject: string = '';
  network: string = '';
  status: string = ''; 
  totalETA: number = 0;
  usedETA: number = 0;
  otherUsedETA: number = 0;
  isApprove: boolean = false;
  lastDayWork: boolean = false;
  todayDayWork: boolean = false;
  itemToDiscuss: boolean = false;
  eHrsToday: string = '';
  eHrsLast: EHrsLastDTO[] = [];
  myComments: string = '';
  managerComments: string = '';
  jira: string = '';
  rnGuidId: string = '';
  featureName: string = '';
  fixVersion: string = '';
  rnComments: string = '';
  mailTitled: string = '';
}

export class GetTaskByIdModelE extends GetTaskByIdResponseModel{
  firstName: string = '';
  lastName: string = ''
}
//============================================================
export class AddTaskRequestModel{
  userId: number = 0;
  userName: string = '';
  catagory: string = '';
  taskName: string = '' ;
  subProject: string = '';
  projectKey: string = '';
  projectId: number = 0;
  network: string = '';
  status: string = ''; 
  itemToDiscuss: boolean = false;
  myComments: string = '';
  jira: string = '';
}

export class AddTaskResponseModel{
  userId: number = 0;
  userName: string = 'UnAssigned';
  taskUniqueId: string = '';
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
}
//============================================================

export class MiniUpdateTaskRequestModel{
  taskUniqueId: string = '';
  todayDayWork: boolean = false;
  eHrsToday: string = '';
  itemToDiscuss: boolean = false;
  managerComments: string = '';
}

export class MiniUpdateTaskResponseModel{
  status: boolean = false;
}

//============================================================

export class UpdateTaskRequestModel{
  taskUniqueId: string = '';
  userId: number = 0;
  userName: string = '';
  catagory: string = '';
  taskName: string = '' ;
  subProject: string = '';
  network: string = '';
  status: string = '';
  itemToDiscuss: boolean = false;
  myComments: string = '';
  jira: string = '';
  rnGuidId: string = '';
  featureName: string = '';
  fixVersion: string = '';
  rnComments: string = '';
  mailTitled: string = '';
}

export class UpdateTaskResponseModel{
  userName: string = '';
}
//============================================================

export class MoveTaskRequestModel{
  taskUniqueId: string = '';
  IsAlive: boolean = true;
}

export class MoveTaskResponseModel{
  status: boolean = false;
}
//============================================================

export class GetAllResourceNamesRequestModel {
    projectKey: string = '';
}

export class GetAllResourceNamesResponseModel {
    resourceNames: ResourceNameModel[] = [];
}
//============================================================

export class GetSubTasksByIdRequestModel{
  taskUniqueId: string = '';
}

export class GetSubTasksByIdResponseModel{
  taskUniqueId: string = '';
  subTasks: SubTaskModel[] = [];
  otherSubTasks: SubTaskModel[] = [];
}

export class SubTaskModel
{
  subTaskUniqueId: string = '';
  subTaskSeq: number = 0;
  subTaskName: string = '';
  remark: string = ''
  subTaskETA: number = 0;
  status: string = ''
  isColour: boolean = false;
  isApprove: boolean = false;
  workEntrys: WorkEntryDTO[] = [];
}
export class WorkEntryDTO
{
  workEntryUniqueId: string = '';
  entryDate: string = '';
  entryHrs: number = 0;
  userId: number = 0;
}

export class WorkEntryDTOE extends WorkEntryDTO{
  firstName:string = '';
  isEditing: boolean = false;
}

export class SubTaskModelE extends SubTaskModel{
  override workEntrys: WorkEntryDTOE[] = [];
  isEditing: boolean = false;
}
//============================================================

export class AddSubTaskRequestModel{
  taskUniqueId: string = '';
  subTaskSeq: number = 0;
  subTaskName: string = '';
  remark: string = ''
  subTaskETA: number = 0;
  status: string = ''
  isColour: boolean = false;
  isSelf: boolean = true;
}

export class AddSubTaskResponseModel{
  subTaskUniqueId: string = '';
  totalETA: number = 0;
}
//============================================================

export class UpdateSubTaskRequestModel{
  subTaskUniqueId: string = '';
  subTaskSeq: number = 0;
  subTaskName: string = '';
  remark: string = ''
  subTaskETA: number = 0;
  status: string = ''
  isColour: boolean = false;
}

export class UpdateSubTaskResponseModel{
  totalETA: number = 0;
}
//============================================================

export class AddWorkTimeRequestModel{
  subTaskUniqueId: string = '';
  taskUniqueId: string = '';
  entryDate: string = '';
  entryHrs: number = 0;
}

export class AddWorkTimeResponseModel{
  workEntryUniqueId: string = '';
  entryHrs: number = 0;
  usedETA: number = 0;
  otherUsedETA: number = 0;
  userId: number = 0;
}
//============================================================

export class UpdateWorkTimeRequestModel{
  workEntryUniqueId: string = '';
  entryHrs: number = 0;
}

export class UpdateWorkTimeResponseModel{
  entryHrs: number = 0;
  usedETA: number = 0;
  otherUsedETA: number = 0;
}
//============================================================

export class GetTaskLogByIdRequestModel{
  taskUniqueId: string = '';
}

export class GetTaskLogByIdResponseModel{
  tasksLogs:TasksLogModel[]=[];
}

export class TasksLogModel{
  log:string='';
  userId:number=0;
  date:string = '';
}

export class TasksLog{
  log:string='';
  firstName:string='';
  lastName:string='';
  date:string = '';
}
//============================================================

export class GetAllDropDownValuesByProjectKeyRequestModel{
  projectKey: string = '';
  type: string = '';
}

export class GetAllDropDownValuesByProjectKeyResponseModel{
  dropDownValueModels:DropDownValueModel[]=[];
}

export class DropDownValueModelE extends DropDownValueModel{
  isEditing: boolean = false;
}
//============================================================

export class AddDropDownValuesRequestModel{
  projectKey: string = '';
  type: string = '';
  value: string = '';
}

export class AddDropDownValuesResponseModel{
  guidId: string = '';
}
//============================================================


export class UpdateDropDownValuesRequestModel{
  guidId: string = '';
  value: string = '';
}

export class UpdateDropDownValuesResponseModel{
  status: Boolean = false;
}
//============================================================

export class DeleteDropDownValuesRequestModel{
  guidId: string = '';
}

export class DeleteDropDownValuesResponseModel{
  status: Boolean = false;
}
//============================================================

export class ShowAndHideResourceNameRequestModel{
  guidId: string = '';
  isShow:boolean = true;
}

export class ShowAndHideResourceNameResponseModel{
  status: Boolean = false;
}
//============================================================

export class ChangePasswardRequestModel{
  oldPassword: string = '';
  newPassword: string = '';
  source: string = 'Web';
}

export class ChangePasswardResponseModel{
  status: Boolean = false;
}
//============================================================

export class GetAllUserRequestModel {
  projectKey: string = '';
}

export class GetAllUserResponseModel {
  userIdNames: GetAllUserModel[] = [];
}

export class GetAllUserModel {
  userId: number = 0;
  userName: string = '';
}
//============================================================

export class GetProjectAccessByIdRequestModel {
  userId: number = 0;
}

export class GetProjectAccessByIdResponseModel {
  projectAndRoles: ProjectAndRoleModel[] = [];
}

export class ProjectAndRoleModel {
  roleId: number = 0;
  projectId: number = 0;
}
//============================================================

export class AlterUserAccessRequestModel {
  userAccess: UserAccessModel[] = [];
  userId:number = 0;
}

export class AlterUserAccessResponseModel {
  status:boolean = false;
}

export class UserAccessModel{
  projectId:number = 0;
  projectKey : string = '';
  roleId:number = 0;
  acessHave:boolean = false;
}
//============================================================
export class GetAllUseFullLinkRequestModel {
  projectKey: string = '';
}

export class GetAllUseFullLinkResponseModel {
  useFullLinks: UseFullLinkModel[] = [];
}

export class UseFullLinkModel{
  guidId:string = ''; 
  linkName: string = ''; 
  linkURL: string = '';
}
//============================================================

export class AddUseFullLinkRequestModel {
  projectKey: string = ''; 
  linkName: string = ''; 
  linkURL: string = '';
}

export class AddUseFullLinkResponseModel {
  guidId:string = '';
}
//============================================================

export class UpdateUseFullLinkRequestModel {
  guidId:string = ''; 
  linkName: string = ''; 
  linkURL: string = '';
}

export class UpdateUseFullLinkResponseModel {
  status:boolean = false;
}
//============================================================

export class DeleteUseFullLinkRequestModel {
  guidId:string = '';
}

export class DeleteUseFullLinkResponseModel {
  status:boolean = false;
}
//============================================================

export class GetAllTaskCommentRequestModel{
  taskUniqueId: string = '';
}

export class GetAllTaskCommentResponseModel {
    taskComments: TaskCommentModel[] = [];
}

export class TaskCommentModel{
  guidId: string = '';
  firstName:string = '';
  lastName:string = '';
  comment: string = '';
  commentedOn: Date | null = null;
  commentEditedOn: Date | null = null;
  isPreviouslyEdited: boolean = false;
  isSameUserComment: boolean = false;
}

export class TaskCommentModelE extends TaskCommentModel{
  isEdited: boolean = false;
  oldComment: string = '';
}
//============================================================

export class AddTaskCommentRequestModel {
  taskUniqueId: string = ''; 
  comment: string = '';
  projectKey: string = ''; 
}

export class AddTaskCommentResponseModel {
  guidId: string = '';
  firstName:string = '';
  lastName:string = '';
  commentedOn: Date | null = null;
}
//============================================================

export class UpdateTaskCommentRequestModel {
  guidId:string = ''; 
  comment: string = '';
}

export class UpdateTaskCommentResponseModel {
  commentEditedOn: Date | null = null;
  isPreviouslyEdited: boolean = false;
}
//============================================================

export class DeleteTaskCommentRequestModel {
  guidId:string = '';
}

export class DeleteTaskCommentResponseModel {
  status:boolean = false;
}
//============================================================

export class ApproveETARequestModel{
  uniqueId: string = '';
  isAll: boolean = false;
}

export class ApproveETAResponseModel{
  totalETA: number = 0;
}
//============================================================

export class GetShowTasksToCopyRequestModel{
  projectKey: string = '';
  isAlive: boolean = true;
}

export class GetShowTasksToCopyResponseModel{
  tasks: ShowTaskToCopyDTO[] = [];
}

export class ShowTaskToCopyDTO{
  firstName: string = '';
  taskUniqueId: string = '';
  taskSeq: number = 0;
  taskName: string = '' ;
  subProject: string = '';
  network: string = '';
  status: string = ''; 
}
//============================================================
export class GetShowSubTasksToCopyRequestModel{
  taskUniqueId: string = '';
}

export class GetShowSubTasksToCopyResponseModel{
  subTasks: ShowSubTaskToCopyDTO[] = [];
}


export class ShowSubTaskToCopyDTO{
  subTaskSeq: number = 0;
  subTaskName: string = '';
  subTaskETA: number = 0;
}

export class ShowSubTaskToCopyDTOE extends ShowSubTaskToCopyDTO{
  isSelected: boolean = true;
}
//============================================================

export class GetAllRNsRequestModel {
  projectKey: string = '';
}

export class GetAllRNsResponseModel {
  rnAndFeatureList: DropDownValueModel[] = [];
}
//============================================================

export class AddRNAndFeatureRequestModel {
  rn: string = '';
  projectKey:string = '';
}

export class AddRNAndFeatureResponseModel {
  guidId:string = ''; 
}
//============================================================

export class UpdateRNAndFeatureRequestModel {
  guidId:string = ''; 
  rn: string = '';
}

export class UpdateRNAndFeatureResponseModel {
  guidId:string = ''; 
}
//============================================================

export class GetAllTasksByRNGuidIdRequestModel {
  projectKey: string = '';
  guidId:string = '';
}

export class GetAllTasksByRNGuidIdResponseModel {
  tasks: TaskForRNDTO[] = [];
}

export class TaskForRNDTO {
  taskUniqueId: string = '';
  jira: string = '';
  featureName: string = '' ;
  fixVersion: string = '';
  rnComments: string = '';
  firstName: string = '';
  isAlive: boolean = false;
}
//============================================================