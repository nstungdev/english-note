import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-vocabulary-table',
  standalone: true,
  templateUrl: './vocabulary-table.component.html',
  styleUrls: ['./vocabulary-table.component.scss'],
  imports: [MatTableModule, MatButtonModule, CommonModule],
})
export class VocabularyTableComponent implements OnInit {
  displayedColumns: string[] = ['word', 'meaning', 'actions'];
  dataSource = new MatTableDataSource<any>([
    {
      word: 'example',
      meanings: [
        {
          partOfSpeech: 'noun',
          meaning: 'A representative form or pattern',
        },
      ],
    },
    {
      word: 'test',
      meanings: [
        {
          partOfSpeech: 'verb',
          meaning:
            'To take measures to check the quality, performance, or reliability of something',
        },
      ],
    },
  ]);

  constructor() {}

  ngOnInit(): void {}
}
