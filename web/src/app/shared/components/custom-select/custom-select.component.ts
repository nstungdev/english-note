import { Component, Input, forwardRef, OnInit, Injector } from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  NgControl,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { getErrorMessage } from '../../utils/form-utils';

interface CustomSelectOption {
  label: string;
  value: string | number;
}

@Component({
  selector: 'app-custom-select',
  standalone: true,
  imports: [CommonModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CustomSelectComponent),
      multi: true,
    },
  ],
  templateUrl: './custom-select.component.html',
  styleUrls: ['./custom-select.component.scss'],
})
export class CustomSelectComponent implements ControlValueAccessor, OnInit {
  private static idCounter = 0;
  public _uid: number;
  @Input() label: string = '';
  @Input() options: CustomSelectOption[] = [];
  @Input() disabled = false;
  @Input() placeholder: string = '';

  value: string | number | null = null;
  onChange = (_: any) => {};
  onTouched = () => {};
  private ngControl: NgControl | null = null;

  constructor(private readonly injector: Injector) {
    this._uid = CustomSelectComponent.idCounter++;
  }

  ngOnInit(): void {
    const control = this.injector.get(NgControl, null);
    this.ngControl = control;
    if (this.ngControl) {
      this.ngControl.valueAccessor = this;
    }
  }

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

  onSelectionChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.value = select.value;
    this.onChange(this.value);
  }

  onBlur(): void {
    this.onTouched();
  }
}
