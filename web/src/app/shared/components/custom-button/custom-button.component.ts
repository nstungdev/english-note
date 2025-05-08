import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-custom-button',
  standalone: true,
  templateUrl: './custom-button.component.html',
  styleUrls: ['./custom-button.component.scss'],
  imports: [CommonModule, FontAwesomeModule],
})
export class CustomButtonComponent {
  @Input() type: 'button' | 'submit' = 'button';
  @Input() disabled: boolean = false;
  @Input() variant: 'primary' | 'secondary' | 'warning' | 'danger' | 'info' =
    'primary';
  @Input() outline: boolean = false;
  @Input() icon?: string;
  @Output() clicked = new EventEmitter<Event>();

  get iconTuple(): [string, string] | undefined {
    return this.icon ? ['fas', this.icon] : undefined;
  }

  handleClick(event: Event): void {
    if (!this.disabled) {
      this.clicked.emit(event);
    }
  }
}
