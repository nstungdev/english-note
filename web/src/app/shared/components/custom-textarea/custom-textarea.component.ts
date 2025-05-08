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
  selector: 'app-custom-textarea',
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatInputModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomTextareaComponent),
      multi: true,
    },
  ],
  templateUrl: './custom-textarea.component.html',
  styleUrls: ['./custom-textarea.component.scss'],
})
export class CustomTextareaComponent implements ControlValueAccessor, OnInit {
  private static idCounter = 0;
  public _uid: number;
  @Input() label: string = '';
  @Input() rows: number = 3;

  value: string = '';
  disabled: boolean = false;
  onChange = (_: any) => {};
  onTouched = () => {};
  private ngControl: NgControl | null = null;

  constructor(private injector: Injector) {
    this._uid = CustomTextareaComponent.idCounter++;
  }

  ngOnInit(): void {
    const control = this.injector.get(NgControl, null);
    this.ngControl = control;
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }

  writeValue(obj: any): void {
    this.value = obj ?? '';
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

  /** Returns the appropriate error message based on validation errors */
  get errorMessage(): string | null {
    return getErrorMessage(this.ngControl?.control || null, this.label);
  }

  onInput(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    this.value = textarea.value;
    this.onChange(this.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
