import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit {
  user: any;
  loading = true;

  constructor(
    private userService: UserService, 
    private router: Router,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit(): void {
    this.spinner.show();

    this.userService.getProfile().subscribe({
      next: (data) => {
        this.user = data;
        this.loading = false;
        this.spinner.hide();
      },
      error: (err) => {
        console.error('Erro ao carregar perfil:', err);
        this.loading = false;
        this.spinner.hide();
      }
    });
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }
}
