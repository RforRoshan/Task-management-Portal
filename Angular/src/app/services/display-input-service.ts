import { ApplicationRef, ComponentRef, createComponent, EnvironmentInjector, Injectable, Type } from '@angular/core';
import { WorkEntryDisplay } from '../components/shared/work-entry-display/work-entry-display';
import { AddUsefullLink } from '../components/feature/use-full-links/add-usefull-link/add-usefull-link';
import { AddNewCurrentTask } from '../components/feature/add-new-current-task/add-new-current-task';
import { DropDownValueModel, ResourceNameModel } from '../models/common-model';
// import { CopyTask } from '../components/feature/copy-task/copy-task';

@Injectable({
  providedIn: 'root'
})
export class DisplayInputService {
  private compRef?: ComponentRef<any>;

  constructor(private appRef: ApplicationRef, private injector: EnvironmentInjector ) {}

  /** Mount any component dynamically and pass optional inputs */
  private mountComponent<T extends object>(component: Type<T>, inputs?: Partial<T>): T {
    this.closeComponent();

    const comp = createComponent(component, { environmentInjector: this.injector });
    this.compRef = comp;

    if (inputs) Object.assign(comp.instance, inputs);

    this.appRef.attachView(comp.hostView);
    const domElem = (comp.hostView as any).rootNodes[0] as HTMLElement;
    document.body.appendChild(domElem);

    return comp.instance;
  }

  /** Close any active component */
  private closeComponent(): void {
    if (this.compRef) {
      this.appRef.detachView(this.compRef.hostView);
      this.compRef.destroy();
      this.compRef = undefined;
    }
  }

  /** Generic helper to show a component and return its result */
  private showComponent<T extends object, R = any>(
    component: Type<T>,
    inputs?: Partial<T>
  ): Promise<R | null> {
    return new Promise((resolve) => {
      const comp = this.mountComponent(component, inputs) as any;

      if (!comp.closed) {
        console.warn(`${component.name} does not have a 'closed' EventEmitter`);
        this.closeComponent();
        resolve(null);
        return;
      }

      comp.closed.subscribe((result: R | null) => {
        this.closeComponent();
        resolve(result);
      });
    });
  }

  // ================================
  // ✅ Specific Component Methods
  // ================================

  showWorkEntryInputBox(): Promise<{ enterDate: string; enterHrs: number } | null> {
    return this.showComponent(WorkEntryDisplay);
  }

  showUseFullLinkInputBox( isUpdate: boolean, linkName: string, linkURL: string ): Promise<{ linkName: string; linkURL: string } | null> {
    // 👇 passing inputs to component
    return this.showComponent(AddUsefullLink, { isUpdate, linkName, linkURL });
  }

  showAddNewCurrentTaskInputBox( resourceNamesList: ResourceNameModel[], statusList: DropDownValueModel[], categoryList: DropDownValueModel[],
     projectList: DropDownValueModel[], networkList: DropDownValueModel[] ): Promise<{userName : string; catagory : string; taskName : string; subProject : string; network : string; status : string; myComments: string; jira: string; itemToDiscuss:boolean;} | null> {
    // 👇 passing inputs to component
    return this.showComponent(AddNewCurrentTask, {resourceNamesList, statusList, categoryList, projectList, networkList});
  }

  // showTaskCopyInputBox( projectname: string): Promise<ShowSubTaskToCopyDTOE[] | null> {
  //   // 👇 passing inputs to component
  //   return this.showComponent(CopyTask, {projectname});
  // }
}