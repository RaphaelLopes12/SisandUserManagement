import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../user.service';
import { AuthService } from '../../auth/auth.service';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { ToastService } from '../../shared/toast.service';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  userId: string | null = null;
  isEditing = false;
  loading = false;
  hideNavbar = false;
  showPasswordFields = true;

  roles = [
    { value: 'Admin', label: 'Administrator' },
    { value: 'User', label: 'Usuário' },
  ];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) {
    this.userForm = this.fb.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        username: [
          '',
          [
            Validators.required,
            Validators.maxLength(50),
            this.noSpacesValidator,
          ],
        ],
        address: ['', Validators.required],
        birthDate: [null, Validators.required],
        phoneNumber: ['', Validators.required],
        password: ['', [Validators.minLength(6)]],
        passwordConfirmation: [''],
        role: ['User', Validators.required],
      },
      { validators: this.passwordsMatchValidator }
    );

    this.userForm.get('username')?.valueChanges.subscribe((value) => {
      if (value) {
        this.userForm.patchValue(
          {
            username: value.trim().replace(/\s/g, ''),
          },
          { emitEvent: false }
        );
      }
    });
  }

  noSpacesValidator(control: AbstractControl): ValidationErrors | null {
    return /\s/.test(control.value) ? { noSpacesAllowed: true } : null;
  }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id');
    this.isEditing = !!this.userId;

    this.route.queryParams.subscribe((params) => {
      this.hideNavbar = params['fromLogin'] === 'true';
    });

    if (this.isEditing) {
      this.showPasswordFields = false;
      this.loading = true;

      this.userService.getUserById(this.userId!).subscribe({
        next: (user) => {
          this.userForm.patchValue({
            name: user.name,
            email: user.email,
            username: user.username,
            address: user.address,
            phoneNumber: user.phoneNumber,
            role: user.role,
            birthDate: this.convertDateToNgbFormat(user.birthDate),
          });
          this.loading = false;
        },
        error: () => {
          this.toastService.error('Erro ao carregar usuário');
          this.loading = false;
        },
      });
    }
  }

  saveUser(): void {
    if (this.userForm.invalid) return;

    const userData = this.userForm.value;
    userData.birthDate = this.convertDateToISO(userData.birthDate);

    if (this.isEditing && !userData.password) {
      delete userData.password;
      delete userData.passwordConfirmation;
    }

    this.loading = true;

    if (this.isEditing) {
      this.userService.updateUser(this.userId!, userData).subscribe({
        next: () => {
          this.toastService.success('Usuário atualizado com sucesso!');
          this.router.navigate(['/users']);
        },
        error: (error) => {
          this.handleError(error);
        },
        complete: () => {
          this.loading = false;
        },
      });
    } else {
      this.authService.register(userData).subscribe({
        next: () => {
          this.toastService.success('Usuário registrado com sucesso!');
          this.router.navigate(['/login'], {
            queryParams: { fromRegister: 'true' },
          });
        },
        error: (error) => {
          this.handleError(error);
        },
        complete: () => {
          this.loading = false;
        },
      });
    }
  }

  private passwordsMatchValidator(
    group: AbstractControl
  ): ValidationErrors | null {
    const password = group.get('password')?.value;
    const passwordConfirmation = group.get('passwordConfirmation')?.value;
    return password === passwordConfirmation
      ? null
      : { passwordsMismatch: true };
  }

  private handleError(error: any): void {
    this.loading = false;

    if (error.status === 400) {
      const validationErrors = error.error.errors;

      if (validationErrors) {
        Object.values(validationErrors).forEach((messages: any) => {
          messages.forEach((msg: string) => this.toastService.error(msg));
        });
        return;
      }
    }

    if (error.error?.message) {
      this.toastService.error(error.error.message);
      return;
    }

    this.toastService.error('Ocorreu um erro ao processar sua solicitação.');
  }

  convertDateToNgbFormat(dateString: string): NgbDateStruct {
    if (!dateString) return { year: 2000, month: 1, day: 1 };
    const date = new Date(dateString);
    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate(),
    };
  }

  convertDateToISO(date: NgbDateStruct): string {
    if (!date) return '';
    return `${date.year}-${this.padNumber(date.month)}-${this.padNumber(
      date.day
    )}`;
  }

  padNumber(value: number): string {
    return value < 10 ? `0${value}` : `${value}`;
  }

  cancel(): void {
    this.router.navigate(['/users']);
  }
}
