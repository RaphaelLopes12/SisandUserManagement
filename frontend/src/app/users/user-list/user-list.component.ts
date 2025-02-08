import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { ToastService } from '../../shared/toast.service';
import { DeleteConfirmationComponent } from 'src/app/shared/modal-delete/delete-confirmation.component';
import { User } from '../models/user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  searchQuery: string = '';
  loading = true;
  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalItems: number = 0;
  typingTimeout: any;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastService: ToastService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.userService.getUsers(this.currentPage, this.itemsPerPage, this.searchQuery).subscribe({
      next: (data) => {
        console.log('Dados recebidos do backend:', data);
        this.users = data.users.map((user: any): User => ({
          id: user.Id,
          name: user.Name,
          email: user.Email,
          username: user.Username,
          address: user.Address,
          birthDate: user.BirthDate,
          phoneNumber: user.PhoneNumber,
          role: user.Role,
          createdAt: user.CreatedAt,
          updatedAt: user.UpdatedAt
        }));
        console.log('Usuários processados:', this.users);
        this.totalItems = data.totalUsers;
        this.loading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar usuários:', err);
        this.loading = false;
      },
    });
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.loadUsers();
  }

  searchUsers(): void {
    clearTimeout(this.typingTimeout);
    this.typingTimeout = setTimeout(() => {
      this.currentPage = 1;
      this.loadUsers();
    }, 500);
  }

  navigateToCreate(): void {
    this.router.navigate(['/users/new']);
  }

  navigateToEdit(userId: string): void {
    this.router.navigate([`/users/edit/${userId}`]);
  }

  openDeleteModal(user: User): void {
    const modalRef = this.modalService.open(DeleteConfirmationComponent);
    modalRef.componentInstance.userName = user.name;

    modalRef.result
      .then((result) => {
        if (result) {
          this.deleteUser(user.id);
        }
      })
      .catch(() => {});
  }

  deleteUser(id: string): void {
    this.userService.deleteUser(id).subscribe({
      next: () => {
        this.loadUsers();
        this.toastService.success('Usuário excluído com sucesso!');
      },
      error: (err) => {
        console.error('Erro ao excluir usuário:', err);
        this.toastService.error('Erro ao excluir usuário.');
      },
    });
  }
}
