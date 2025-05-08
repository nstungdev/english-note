import { Component, Input, forwardRef, OnInit, Injector } from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  NgControl,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { getErrorMessage } from '../../utils/form-utils';

@Component({
  selector: 'app-custom-input',
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomInputComponent),
      multi: true,
    },
  ],
  templateUrl: './custom-input.component.html',
  styleUrls: ['./custom-input.component.scss'],
})
export class CustomInputComponent implements ControlValueAccessor, OnInit {
  private static idCounter = 0;
  public _uid: number;
  @Input() label: string = '';
  @Input() type: string = 'text';

  value: string = '';
  disabled: boolean = false;
  onChange = (_: any) => {};
  onTouched = () => {};
  private ngControl: NgControl | null = null;

  constructor(private injector: Injector) {
    this._uid = CustomInputComponent.idCounter++;
  }

  ngOnInit(): void {
    const control = this.injector.get(NgControl, null);
    this.ngControl = control;
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }

  /** Returns the appropriate error message based on validation errors */
  get errorMessage(): string | null {
    return getErrorMessage(this.ngControl?.control || null, this.label);
  }

  writeValue(obj: any): void {
    this.value = obj;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.value = input.value;
    this.onChange(this.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
