import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { CustomButtonComponent } from '@/shared/components/custom-button/custom-button.component';
import { VocabularyService } from '../../services/vocabulary.service';
import { LoadingComponent } from '@/shared/components/loading/loading.component';

@Component({
  selector: 'app-import-vocabulary-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    CustomButtonComponent,
    LoadingComponent,
  ],
  templateUrl: './import-vocabulary-dialog.component.html',
  styleUrls: ['./import-vocabulary-dialog.component.scss'],
})
export class ImportVocabularyDialogComponent {
  selectedFile: File | null = null;
  fileName: string = '';
  error: string = '';
  loading = false;

  constructor(
    private readonly dialogRef: MatDialogRef<ImportVocabularyDialogComponent>,
    private readonly vocabularyService: VocabularyService
  ) {}

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length === 1) {
      const file = input.files[0];
      if (file.type === 'application/json' || file.name.endsWith('.json')) {
        this.selectedFile = file;
        this.fileName = file.name;
        this.error = '';
      } else {
        this.error = 'Only .json files are accepted.';
        this.selectedFile = null;
        this.fileName = '';
      }
    } else {
      this.selectedFile = null;
      this.fileName = '';
      this.error = '';
    }
  }

  onImport() {
    if (!this.selectedFile) {
      this.error = 'Please select a .json file to import.';
      return;
    }
    this.loading = true;
    this.vocabularyService.importVocabularyBulk(this.selectedFile).subscribe({
      next: () => {
        this.loading = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message ?? 'Import failed.';
      },
    });
  }

  onClose() {
    this.dialogRef.close(false);
  }
}
