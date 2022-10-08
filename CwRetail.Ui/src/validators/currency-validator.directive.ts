import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[currencyValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: CurrencyValidatorDirective,
    multi: true
  }]
})
export class CurrencyValidatorDirective implements Validator {

  constructor() { }

  validate(control: AbstractControl): { [key: string]: any } | null {
    if (control.value && !(/^[0-9]*(\.[0-9]{0,2})?$/.test(control.value))) {
      return { 'currencyInvalid': true };
    }
    return null;
  }
}
