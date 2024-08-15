import { Component, EventEmitter, Input, Output } from '@angular/core';
import Modal from '../../models/Modal';

@Component({
  selector: 'app-modal-confirm',
  standalone: true,
  imports: [],
  templateUrl: './modal-confirm.component.html',
  styleUrl: './modal-confirm.component.scss'
})
export class ModalConfirmComponent {
  @Input() modal: Modal = new Modal();
  @Output() confirm = new EventEmitter<void>();

  onConfirm() {
    this.confirm.emit();
  }
}
