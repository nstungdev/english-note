import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomButtonComponent } from '@/shared/components/custom-button/custom-button.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ImportVocabularyDialogComponent } from '../import-vocabulary-dialog/import-vocabulary-dialog.component';

@Component({
  selector: 'app-vocabulary-action-panel',
  standalone: true,
  imports: [CommonModule, CustomButtonComponent, MatCardModule, MatIconModule],
  templateUrl: './vocabulary-action-panel.component.html',
  styleUrls: ['./vocabulary-action-panel.component.scss'],
})
export class VocabularyActionPanelComponent {
  constructor(
    private readonly router: Router,
    private readonly dialog: MatDialog
  ) {}
  onAdd(): void {
    this.router.navigate(['/add-vocabulary']);
  }
  onBulkUpload(): void {
    this.dialog
      .open(ImportVocabularyDialogComponent, {
        width: '400px',
        disableClose: true,
        autoFocus: false,
      })
      .afterClosed()
      .subscribe((result) => {
        if (result) {
          // TODO: reload vocabulary list if needed
        }
      });
  }
  onExport(): void {
    // TODO: Implement export action
  }
}
