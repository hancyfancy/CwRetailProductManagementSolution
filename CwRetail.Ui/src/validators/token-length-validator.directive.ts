import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';

@Directive({
  selector: '[tokenLengthValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: TokenLengthValidatorDirective,
    multi: true
  }]
})
export class TokenLengthValidatorDirective implements Validator {

  constructor() { }

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && control.value.length != 200) {
      return { 'tokenLengthInvalid': true };
    }
    return null;
  }
}
