import { ApplicationRef, ComponentRef, createComponent, Injectable, Injector } from '@angular/core';
import { AlertDisplay } from '../components/shared/alert-display/alert-display';


@Injectable({
  providedIn: 'root'
})
export class AlertService {
   private alertRef?: ComponentRef<AlertDisplay>;

  constructor(private appRef: ApplicationRef) {}

  private mountAlert(): AlertDisplay {
    this.closeAlert();
    const alert = createComponent(AlertDisplay, {
      environmentInjector: this.appRef.injector
    });
    this.alertRef = alert;
    this.appRef.attachView(alert.hostView);
    const domElem = (alert.hostView as any).rootNodes[0] as HTMLElement;
    document.body.appendChild(domElem);
    return alert.instance;
  }

  showSuccess(message:string) {
    const alert = this.mountAlert();
    alert.type = 'success';
    alert.title = 'Success!';
    alert.message = message;
    alert.icon = 'bi-check-circle-fill';
    alert.close.subscribe(() => this.closeAlert());
  }

  showError(message:string = 'Something went wrong.') {
    const alert = this.mountAlert();
    alert.type = 'danger';
    alert.title = 'Error!';
    alert.message = message;
    alert.icon = 'bi-x-circle-fill';
    alert.close.subscribe(() => this.closeAlert());
  }

  showTokenError() {
    const alert = this.mountAlert();
    alert.type = 'warning';
    alert.title = 'Error!';
    alert.message = 'Token expire, please login again!';
    alert.icon = 'bi-exclamation-triangle-fill';
    alert.close.subscribe(() => this.closeAlert());
  }

  showConfirmation(message:string, confirmText:string): Promise<boolean> {
    return new Promise<boolean>((resolve) => {
      const alert = this.mountAlert();
      alert.type = 'warning';
      alert.title = 'Are you sure?';
      alert.message = message;
      alert.icon = 'bi-exclamation-triangle-fill';
      alert.isConfirm = true;
      alert.confirmText = confirmText;
      alert.cancelText = 'Cancel';

      alert.confirm.subscribe(() => {
        this.closeAlert();
        resolve(true);
      });

      alert.cancel.subscribe(() => {
        this.closeAlert();
        resolve(false);
      });

      alert.close.subscribe(() => {
        this.closeAlert();
        resolve(false);
      });
    });
  }

  closeAlert() {
    if (this.alertRef) {
      this.appRef.detachView(this.alertRef.hostView);
      this.alertRef.destroy();
      this.alertRef = undefined;
    }
  }
}
