import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomButtonComponent } from '@/shared/components/custom-button/custom-button.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-vocabulary-action-panel',
  standalone: true,
  imports: [CommonModule, CustomButtonComponent, MatCardModule, MatIconModule],
  templateUrl: './vocabulary-action-panel.component.html',
  styleUrls: ['./vocabulary-action-panel.component.scss'],
})
export class VocabularyActionPanelComponent {
  onAdd(): void {
    // TODO: Implement add vocabulary action (e.g., open dialog or navigate)
  }
  onBulkUpload(): void {
    // TODO: Implement bulk upload action
  }
  onExport(): void {
    // TODO: Implement export action
  }
}
