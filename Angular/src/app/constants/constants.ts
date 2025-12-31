import { ProjectAccess } from "../models/common-model";

export const AppRouteConstants = {
  
    Login: '/login',
    FirstTimeUser: (token: string) =>  `login/firsttimeuser/${token}`,
    ForgotPassword: '/forgotpassword',
    AccessDenied: '/accessdenied',
    SelectDashboard: '/projectdashboard',
    Dashboard: (projectName: string) => `/${projectName}/dashboard`,
    Task:{
        CurrentTask: (projectName: string) => `/${projectName}/dashboard/currenttask`,
        CurrentTaskById: (projectName: string, taskUniqueId: string) => `/${projectName}/dashboard/currenttask/${taskUniqueId}`,
        DoneTask: (projectName: string) => `/${projectName}/dashboard/donetask`,
        DoneTaskById: (projectName: string, taskUniqueId: string) => `/${projectName}/dashboard/donetask/${taskUniqueId}`,
        TaskSetup: (projectName: string) => `/${projectName}/dashboard/tasksetup`,
        TaskSetupByType: (projectName: string, type: string) => `/${projectName}/dashboard/tasksetup/${type}`
    },
    UseFullLinks: (projectName: string) => `/${projectName}/dashboard/usefulllinks`,
    
    RNFeature: {
        Root: (projectName: string) => `/${projectName}/dashboard/rnfeature`,
        Detalis: (projectName: string,  guidId: string) => `/${projectName}/dashboard/rnfeature/${guidId}`
    },
    UserManagement: {
        Root: (projectName: string) => `/${projectName}/dashboard/usermanagement`, 
        RegisterUser: (projectName: string) => `/${projectName}/dashboard/usermanagement/registeruser`,
        UserAccess: (projectName: string) => `/${projectName}/dashboard/usermanagement/useraccess`
    },
    MyProfile: {
        Root: (projectName: string) => `/${projectName}/dashboard/myprofile`,
        ChangePassword: (projectName: string) => `/${projectName}/dashboard/myprofile/changepassword`,
        PersonalDetails: (projectName: string) => `/${projectName}/dashboard/myprofile/personaldetails`
    }
};


export const ControllerConstants = {
    User: 'User',
    Task: 'Task'

};

export const ActionConstants = {
    Login: 'login',
    Register:'Register',
    GetProjectAccess: 'GetProjectAccess',
    GetDashBoardData: 'GetDashBoardData',
    ForgotPasswardLinkGenerate: 'ForgotPasswardLinkGenerate',
    ForgotPasswardReset: 'ForgotPasswardReset',
    GetAllDropDownValues: 'GetAllDropDownValues',
    GetAllCurrentTasks: 'GetAllCurrentTasks',
    GetAllDoneTasks: 'GetAllDoneTasks',
    GetTaskById: 'GetTaskById',
    AddTask: 'AddTask',
    MiniUpdateTask: 'MiniUpdateTask',
    UpdateTask: 'UpdateTask',
    MoveTask: 'MoveTask',
    GetAllResourceNames: 'GetAllResourceNames',
    ShowAndHideResourceName: 'ShowAndHideResourceName',
    GetSubTasksById: 'GetSubTasksById',
    AddSubTask: 'AddSubTask',
    UpdateSubTask: 'UpdateSubTask',
    AddWorkTime: 'AddWorkTime',
    UpdateWorkTime:'UpdateWorkTime',
    GetTaskLogById:'GetTaskLogById',
    GetAllDropDownValuesByProjectKey:'GetAllDropDownValuesByProjectKey',
    AddDropDownValues:'AddDropDownValues',
    DeleteDropDownValues:'DeleteDropDownValues',
    UpdateDropDownValues: 'UpdateDropDownValues',
    GetAllRoleByAccess:'GetAllRoleByAccess',
    ChangePassward:'ChangePassward',
    FirstTimeUser: 'FirstTimeUser',
    GetAllUser: 'GetAllUser',
    GetProjectAccessById: 'GetProjectAccessById',
    AlterUserAccess: 'AlterUserAccess',
    GetAllUseFullLink: 'GetAllUseFullLink',
    AddUseFullLink: 'AddUseFullLink',
    UpdateUseFullLink: 'UpdateUseFullLink',
    DeleteUseFullLink: 'DeleteUseFullLink',
    GetAllTaskComment: 'GetAllTaskComment',
    AddTaskComment: 'AddTaskComment',
    UpdateTaskComment: 'UpdateTaskComment',
    DeleteTaskComment: 'DeleteTaskComment',
    ApproveETA: 'ApproveETA',
    GetShowTasksToCopy: 'GetShowTasksToCopy',
    GetShowSubTasksToCopy: 'GetShowSubTasksToCopy',
    GetAllRNs: 'GetAllRNs',
    AddRNAndFeature: 'AddRNAndFeature',
    UpdateRNAndFeature: 'UpdateRNAndFeature',
    GetAllTasksByRNGuidId: 'GetAllTasksByRNGuidId'
};

export const URLConstants = {
    Jira: 'https://roshan.atlassian.net/browse/'

};

export const SessionStorageKeyConstants = {
    Auth: '26456235',
    Username: '65487532',
    UserId: '65147532',
    RoleId: '65247532',
    ProjectId: '65347532',
    UserRoleId: '65447532',
    FirstName: '65857532',
    MiddleName: '65647532',
    LastName: '65877532',
    FullName: '65947530',
    MultiProjectAccessTo: '65047532',
    PageSize: '546588666',
    RNName: '9876543210',
    PreviousUrl: '1234567890',
    ThemeName: 'ThemeName'

};

export const ClassColorMap: Record<string, string> = {
    A: 'bg-primary',
    B: 'bg-success',
    C: 'bg-danger',
    D: 'bg-warning',
    E: 'bg-info',
    F: 'bg-secondary',
    G: 'bg-dark',
    H: 'bg-primary',
    I: 'bg-success',
    J: 'bg-danger',
    K: 'bg-warning',
    L: 'bg-info',
    M: 'bg-secondary',
    N: 'bg-dark',
    O: 'bg-primary',
    P: 'bg-danger',
    Q: 'bg-danger',
    R: 'bg-primary',
    S: 'bg-info',
    T: 'bg-secondary',
    U: 'bg-dark',
    V: 'bg-primary',
    W: 'bg-success',
    X: 'bg-danger',
    Y: 'bg-warning',
    Z: 'bg-info'
};

export const ColorMap: Record<string, string> = {
    A: '#4CAF50',
    B: '#E91E63',
    C: '#9C27B0',
    D: '#2196F3',
    E: '#673AB7',
    F: '#4CAF50',
    G: '#03A9F4',
    H: '#00BCD4',
    I: '#009688',
    J: '#3F51B5',
    K: '#8BC34A',
    L: '#CDDC39',
    M: '#9C27B0',
    N: '#FFC107',
    O: '#FF9800',
    P: '#FF5722',
    Q: '#795548',
    R: '#00BCD4',
    S: '#E91E63',
    T: '#607D8B',
    U: '#9E9E9E',
    V: '#8BC34A',
    W: '#FF9800',
    X: '#3F51B5',
    Y: '#FFEB3B',
    Z: '#F44336'
};

export const StatusColorMap: Record<string, string> = {
    A: '#64C7CC',  // Soft Teal
    B: '#A7C7E7',  // Pastel Blue
    C: '#A8E6CF',  // Pastel Green
    D: '#C5B4E3',  // Pastel Purple
    E: '#FFBC80',  // Soft Orange
    F: '#ADEFD1',  // Mint
    G: '#E6E6FA',  // Lavender
    H: '#C6FF00',  // Bright Lime
    I: '#4FC3F7',  // Sky Blue
    J: '#FF6F61',  // Coral
    K: '#B388FF',  // Electric Purple
    L: '#26A69A',  // Sea Green
    M: '#00E5FF',  // Aqua Mint
    N: '#FF8A80',  // Sunset Pink
    O: '#546E7A',  // Steel Grey
    P: '#B0BEC5',  // Warm Grey
    Q: '#6A7FDB',  // Slate Blue
    R: '#C48B9F',  // Dusty Rose
    S: '#9FAE7C',  // Olive Green
    T: '#B8AFA6',  // Taupe
    U: '#64B5F6',  // Light Blue
    V: '#81C784',  // Light Green
    W: '#FFCCBC',  // Soft Peach
    X: '#F8BBD0',  // Light Pink
    Y: '#D1C4E9',  // Soft Violet
    Z: '#FFE082'   // Soft Yellow
};

export const JiraColorMap: Record<string, string> = {
  CPP:  '#1E88E5', // Blue
  PPG:  '#E53935', // Green
  PLP:  '#922d50', // Orange
  CCP:  '#8E24AA', // Purple
  PLA:  '#E53935', // Red
};



export const NumberColorMap: Record<number, string> = {
  1:  '#FF6B6B',  // Coral Red
  2:  '#F06595',  // Pink Rose
  3:  '#CC5DE8',  // Purple
  4:  '#845EF7',  // Violet
  5:  '#5C7CFA',  // Blue
  6:  '#339AF0',  // Sky Blue
  7:  '#22B8CF',  // Cyan
  8:  '#20C997',  // Mint Green
  9:  '#51CF66',  // Green
  10: '#94D82D',  // Lime
  11: '#FCC419',  // Yellow
  12: '#FF922B',  // Orange
  13: '#FF6B6B',  // Coral Red (looped)
  14: '#F06595',
  15: '#CC5DE8',
  16: '#845EF7',
  17: '#5C7CFA',
  18: '#339AF0',
  19: '#22B8CF',
  20: '#20C997',
  21: '#51CF66',
  22: '#94D82D',
  23: '#FCC419',
  24: '#FF922B',
  25: '#FF6B6B',
  26: '#F06595',
  27: '#CC5DE8',
  28: '#845EF7',
  29: '#5C7CFA',
  30: '#339AF0',
  31: '#22B8CF',
  32: '#FF6B6B',  // Coral Red
  33: '#F06595',  // Pink Rose
  34: '#CC5DE8',  // Purple
  35: '#845EF7',  // Violet
  36: '#5C7CFA',  // Blue
  37: '#339AF0',  // Sky Blue
  38: '#22B8CF',  // Cyan
  39: '#20C997',  // Mint Green
  40: '#51CF66',  // Green
  41: '#FCC419',  // Yellow
  42: '#FF922B',  // Orange
  43: '#FF6B6B',  // Coral Red (looped)
  44: '#F06595',
  45: '#CC5DE8',
  46: '#845EF7',
  47: '#5C7CFA',
  48: '#339AF0',
  49: '#22B8CF',
  50: '#20C997',
  51: '#51CF66',
  52: '#94D82D',
  53: '#FCC419',
  54: '#FF922B',
  55: '#FF6B6B',
  56: '#F06595',
  57: '#CC5DE8',
  58: '#845EF7',
  59: '#5C7CFA',
  60: '#339AF0',
  61: '#22B8CF'
};

export const XlsxConstants = {
    FIRSTROW_COLOR : '61adbf',
    SEC_ROW_COLOR : 'DFEEF2'
}