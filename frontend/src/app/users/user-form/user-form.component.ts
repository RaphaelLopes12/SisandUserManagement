import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../user.service';
import { AuthService } from '../../auth/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html'
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  userId: string | null = null;
  isEditing = false;
  loading = false;
  hideNavbar = false;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {  
    this.userId = this.route.snapshot.paramMap.get('id');
    this.isEditing = !!this.userId;
  
    this.route.queryParams.subscribe((params) => {
      this.hideNavbar = params['fromLogin'] === 'true';
    });

    if (this.isEditing) {
      this.loading = true;
      this.userService.getUserById(this.userId!).subscribe({
        next: (user) => {
          this.userForm.patchValue({
            name: user.name,
            email: user.email
          });
          this.loading = false;
        },
        error: () => {
          this.toastService.error('Erro ao carregar usuário');
          this.loading = false;
        }
      });
    }
  }

  saveUser(): void {
    if (this.userForm.invalid) return;
  
    const userData = this.userForm.value;
  
    if (this.isEditing) {
      this.userService.updateUser(this.userId!, userData).subscribe({
        next: () => {
          this.toastService.success('Usuário atualizado com sucesso!');
          this.router.navigate(['/users']);
        },
        error: () => this.toastService.error('Erro ao atualizar usuário')
      });
    } else {
      this.authService.register(userData).subscribe({
        next: () => {
          this.toastService.success('Usuário registrado com sucesso!');
          this.router.navigate(['/login'], { queryParams: { fromRegister: 'true' } });
        },
        error: () => this.toastService.error('Erro ao criar usuário')
      });
    }
  }  

  cancel(): void {
    this.router.navigate(['/users']);
  }
}
