import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'rks-alert-display',
  imports: [CommonModule, FormsModule],
  templateUrl: './alert-display.html',
  styleUrl: './alert-display.scss'
})
export class AlertDisplay {
  @Input() title = '';
  @Input() message = '';
  @Input() icon = '';
  @Input() type: 'success' | 'danger' | 'warning' | 'info' = 'info';

  // Confirmation mode
  @Input() isConfirm = false;
  @Input() confirmText = 'Yes';
  @Input() cancelText = 'Cancel';

  @Output() close = new EventEmitter<void>();
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  onClose() {
    this.close.emit();
  }

  onConfirm() {
    this.confirm.emit();
  }

  onCancel() {
    this.cancel.emit();
  }
}
