import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomButtonComponent } from '@/shared/components/custom-button/custom-button.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vocabulary-action-panel',
  standalone: true,
  imports: [CommonModule, CustomButtonComponent, MatCardModule, MatIconModule],
  templateUrl: './vocabulary-action-panel.component.html',
  styleUrls: ['./vocabulary-action-panel.component.scss'],
})
export class VocabularyActionPanelComponent {
  constructor(private readonly router: Router) {}
  onAdd(): void {
    this.router.navigate(['/add-vocabulary']);
  }
  onBulkUpload(): void {
    // TODO: Implement bulk upload action
  }
  onExport(): void {
    // TODO: Implement export action
  }
}
