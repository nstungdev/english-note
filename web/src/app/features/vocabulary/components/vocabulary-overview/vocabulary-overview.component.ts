import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VocabularyActionPanelComponent } from '../vocabulary-action-panel/vocabulary-action-panel.component';
import { VocabularyTableComponent } from '../vocabulary-table/vocabulary-table.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-vocabulary-overview',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    VocabularyActionPanelComponent,
    VocabularyTableComponent,
  ],
  templateUrl: './vocabulary-overview.component.html',
  styleUrls: ['./vocabulary-overview.component.scss'],
})
export class VocabularyOverviewComponent {}
