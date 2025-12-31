import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'rks-add-usefull-link',
  imports: [CommonModule, FormsModule],
  templateUrl: './add-usefull-link.html',
  styleUrl: './add-usefull-link.scss'
})
export class AddUsefullLink {
  @Input() isUpdate: boolean = false;
  @Input() linkName: string = ''; 
  @Input() linkURL: string = '';

  @Output() closed = new EventEmitter<{ linkName: string; linkURL: string } | null>();

  onCancel() {
    this.closed.emit(null);
  }

  onSubmit() {
    this.closed.emit({ linkName: this.linkName, linkURL: this.linkURL });
  }
}
