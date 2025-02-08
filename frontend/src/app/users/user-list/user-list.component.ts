import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { ToastService } from '../../shared/toast.service';
import { DeleteConfirmationComponent } from 'src/app/shared/modal-delete/delete-confirmation.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
})
export class UserListComponent implements OnInit {
  users: any[] = [];
  filteredUsers: any[] = [];
  paginatedUsers: any[] = [];
  searchQuery: string = '';
  loading = true;
  itemsPerPage: number = 5;
  currentPage: number = 1;
  totalItems: number = 0;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastService: ToastService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.userService.getUsers().subscribe({
      next: (data) => {
        this.users = data;
        this.filteredUsers = data;
        this.totalItems = data.length;
        this.updatePagination();
        this.loading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar usuários:', err);
        this.loading = false;
      },
    });
  }

  filterUsers(): void {
    this.filteredUsers = this.users.filter(
      (user) =>
        user.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        user.email.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
    this.totalItems = this.filteredUsers.length;
    this.changePage(1);
  }

  updatePagination(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.paginatedUsers = this.filteredUsers.slice(start, end);
  }

  changePage(page: number): void {
    this.currentPage = page;
    this.updatePagination();
  }
  
  navigateToCreate(): void {
    this.router.navigate(['/users/new']);
  }

  navigateToEdit(userId: string): void {
    this.router.navigate([`/users/edit/${userId}`]);
  }


  openDeleteModal(user: any): void {
    const modalRef = this.modalService.open(DeleteConfirmationComponent);
    modalRef.componentInstance.userName = user.name;

    modalRef.result.then((result) => {
      if (result) {
        this.deleteUser(user.id);
      }
    }).catch(() => {});
  }

  deleteUser(id: string): void {
    this.userService.deleteUser(id).subscribe({
      next: () => {
        this.users = this.users.filter((user) => user.id !== id);
        this.filterUsers();
        this.toastService.success('Usuário excluído com sucesso!');
      },
      error: (err) => {
        console.error('Erro ao excluir usuário:', err);
        this.toastService.error('Erro ao excluir usuário.');
      },
    });
  }
}
