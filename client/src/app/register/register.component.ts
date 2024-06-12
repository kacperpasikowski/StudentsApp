import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { passwordValidator } from '../validators/passwordValidator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  registerForm: FormGroup = new FormGroup({});
  isSubmitting = false;

  constructor(private accountService: AccountService, private router: Router){}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      email: new FormControl('', Validators.email),
      password: new FormControl('', [Validators.required,
        Validators.minLength(8), Validators.maxLength(16),
        passwordValidator()
      ]),
      confirmPassword: new FormControl('', [
        Validators.required, this.matchValue('password')
      ]),
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValue(matchTo: string): ValidatorFn{
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
  }
  get password(){
    return this.registerForm.get('password');
  }

  register() {
    if (this.registerForm.valid) {
        // Ensure the model is populated with form values
        this.model = this.registerForm.value;

        console.log(this.model);
        this.accountService.register(this.model).subscribe({
            next: response => {
                console.log(response);
                this.router.navigateByUrl('/');
            },
            error: error => {
              console.log(error);
              this.isSubmitting = false;
            }
        });
    } else {
        console.log('Form is invalid');
    }
}





}
