import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormArray,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { VocabularyService } from '../../services/vocabulary.service';
import { CreateVocabularyRequest } from '../../models/vocabulary.model';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatToolbar } from '@angular/material/toolbar';

@Component({
  selector: 'app-add-vocabulary',
  templateUrl: './add-vocabulary.component.html',
  styleUrls: ['./add-vocabulary.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatCardModule,
    MatToolbar,
  ],
})
export class AddVocabularyComponent {
  vocabularyForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private vocabularyService: VocabularyService
  ) {
    this.vocabularyForm = this.fb.group({
      word: ['', Validators.required],
      meanings: this.fb.array([this.createMeaningGroup()]),
    });
  }

  get meanings(): FormArray {
    return this.vocabularyForm.get('meanings') as FormArray;
  }

  createMeaningGroup(): FormGroup {
    return this.fb.group({
      partOfSpeech: ['', Validators.required],
      meaning: ['', Validators.required],
      ipa: [''],
      pronunciation: [''],
      example: [''],
      note: [''],
      usage: [''],
    });
  }

  addMeaning(): void {
    this.meanings.push(this.createMeaningGroup());
  }

  removeMeaning(index: number): void {
    this.meanings.removeAt(index);
  }

  onSubmit(): void {
    if (this.vocabularyForm.valid) {
      const request: CreateVocabularyRequest = this.vocabularyForm.value;
      this.vocabularyService.create(request).subscribe({
        next: (response) => {
          alert(response.message);
          this.vocabularyForm.reset();
        },
        error: (err) => {
          alert(err.message);
        },
      });
    }
  }
}
