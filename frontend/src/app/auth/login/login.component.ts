import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { UserService } from '../../users/user.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  email = '';
  password = '';
  errorMessage = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private userService: UserService, 
    private router: Router,
    private spinner: NgxSpinnerService
  ) {}

  login(): void {
    this.loading = true;
    this.spinner.show();

    this.authService.login({ email: this.email, password: this.password }).subscribe({
      next: (response) => {
        localStorage.setItem('token', response.token);
          
        this.userService.getProfile().subscribe({
          next: (profile) => {
            localStorage.setItem('user', JSON.stringify(profile)); // Salvar usuário no localStorage

            this.router.navigate(['/profile']).then(() => {
              this.spinner.hide();
              this.loading = false;
            });
          },
          error: () => {
            console.error('Erro ao carregar perfil');
            this.spinner.hide();
            this.loading = false;
          }
        });
      },
      error: () => {
        this.errorMessage = 'Usuário ou senha inválidos';
        this.spinner.hide();
        this.loading = false;
      }
    });
  }
}
