import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'rks-work-entry-display',
  imports: [CommonModule, FormsModule],
  templateUrl: './work-entry-display.html',
  styleUrl: './work-entry-display.scss'
})
export class WorkEntryDisplay {
  enterDate: string = this.getTodayDate();
  enterHrs: number = 0;

  @Output() closed = new EventEmitter<{ enterDate: string, enterHrs: number } | null>();

  onCancel() {
    this.closed.emit(null);
  }

  onSubmit() {
    if(this.enterHrs == null || this.enterHrs < 0){
      this.enterHrs = 0; 
    }
    this.closed.emit({ enterDate: this.enterDate, enterHrs: this.enterHrs });
  }

  private getTodayDate(): string {
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    return `${yyyy}-${mm}-${dd}`;
  }
}
