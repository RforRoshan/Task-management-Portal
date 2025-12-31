import { Routes } from '@angular/router';
import { Login } from './components/feature/login/login/login';
import { Dashboard } from './components/shared/dashboard/dashboard';
import { ForgotPassword } from './components/feature/login/forgot-password/forgot-password';
import { ProjectChoice } from './components/feature/project-choice/project-choice';
import { AccessDenied } from './components/shared/access-denied/access-denied';
import { ProjectHome } from './components/feature/project-home/project-home';
import { CurrentTask } from './components/feature/current-task/current-task';
import { DoneTask } from './components/feature/done-task/done-task';
import { TaskSetupHome } from './components/feature/task-setup/task-setup-home/task-setup-home';
import { RegisterUser } from './components/feature/user-management/register-user/register-user';
import { UserManagementHome } from './components/feature/user-management/user-management-home/user-management-home';
import { ProfileHome } from './components/feature/profile/profile-home/profile-home';
import { ProfileChangePassword } from './components/feature/profile/profile-change-password/profile-change-password';
import { SubTask } from './components/feature/sub-task/sub-task';
import { ResetPassword } from './components/feature/login/forgot-password/reset-password/reset-password';
import { ResourceSetup } from './components/feature/task-setup/resource-setup/resource-setup';
import { CNSSetup } from './components/feature/task-setup/cnssetup/cnssetup';
import { UserAccess } from './components/feature/user-management/user-access/user-access';
import { PersonalDetails } from './components/feature/profile/personal-details/personal-details';
import { FirstTimeUser } from './components/feature/login/first-time-user/first-time-user';
import { NoValidURL } from './components/shared/no-valid-url/no-valid-url';
import { UseFullLinks } from './components/feature/use-full-links/use-full-links';
import { DoneSubTask } from './components/feature/done-sub-task/done-sub-task';
import { RNFeature } from './components/feature/rnfeature/rnfeature';
import { RNFeatureDetails } from './components/feature/rnfeature-details/rnfeature-details';


const projectChildren: Routes = [
    { path: '', component: ProjectHome},
    { path: 'currenttask', component: CurrentTask},
    { path: 'currenttask/:taskUniqueId', component: SubTask},
    { path: 'donetask', component: DoneTask},
    { path: 'donetask/:taskUniqueId', component: DoneSubTask},
    {
        path: 'tasksetup',
        children: [
            { path: '', component: TaskSetupHome},
            { path: 'network', component: CNSSetup, data: { type: 'network' }},
            { path: 'category', component: CNSSetup, data: { type: 'category' }},
            { path: 'status', component: CNSSetup, data: { type: 'status' }},
            { path: 'resource', component: ResourceSetup},
        ]
    },
    {
        path: 'usermanagement',
        children: [
            { path: '', component: UserManagementHome},
            { path: 'registeruser', component: RegisterUser},
            { path: 'useraccess', component: UserAccess}
        ]
    },
    {
        path: 'myprofile',
        children: [
            { path: '', component: ProfileHome},
            { path: 'changepassword', component: ProfileChangePassword},
            { path: 'personaldetails', component: PersonalDetails}
        ]
    },
    { path: 'usefulllinks', component: UseFullLinks},
    { path: 'rnfeature', component: RNFeature},
    { path: 'rnfeature/:guidId', component: RNFeatureDetails}
];

export const routes: Routes = [
    
    { path: '', component: Login},
    { path: 'login', component: Login},
    { path: 'login/firsttimeuser/:token', component: FirstTimeUser},
    { path: 'forgotpassword', component: ForgotPassword},
    { path: 'forgotpassword/:token', component: ResetPassword},
    { path: 'projectdashboard', component: ProjectChoice},
    { path: 'accessdenied', component: AccessDenied},
    { 
        path: 'cpp/dashboard', 
        component: Dashboard,
        data: { projectname: 'cpp' },
        children: projectChildren
    },
    { 
        path: 'ccp/dashboard', 
        component: Dashboard,
        data: { projectname: 'ccp' },
        children: projectChildren
    },
    { 
        path: 'plat/dashboard', 
        component: Dashboard,
        data: { projectname: 'plat' },
        children: projectChildren
    },
    //{ path: '**', redirectTo: 'accessdenied' }
    {path: '**', component: NoValidURL}
];
