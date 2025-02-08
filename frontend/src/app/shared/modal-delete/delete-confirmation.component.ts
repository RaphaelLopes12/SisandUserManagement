import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-delete-confirmation',
  template: `
    <div class="modal-header">
      <h4 class="modal-title">Confirmação de Exclusão</h4>
      <button type="button" class="btn-close" aria-label="Close" (click)="modal.dismiss('cancel')"></button>
    </div>
    <div class="modal-body">
      <p>Tem certeza que deseja excluir <strong>{{ userName }}</strong>?</p>
      <p class="text-danger">Essa ação não pode ser desfeita.</p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-secondary" (click)="modal.dismiss('cancel')">Cancelar</button>
      <button type="button" class="btn btn-danger" (click)="confirmDelete()">Excluir</button>
    </div>
  `
})
export class DeleteConfirmationComponent {
  userName: string = '';

  constructor(public modal: NgbActiveModal) {}

  confirmDelete(): void {
    this.modal.close(true);
  }
}
