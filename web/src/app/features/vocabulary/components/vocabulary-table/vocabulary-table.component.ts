import { AfterViewInit, Component, OnInit } from '@angular/core';
import { VocabularyService } from '../../services/vocabulary.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatTableDataSource } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { VocabularyDTO } from '../../models/vocabulary.model';
import { firstValueFrom } from 'rxjs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-vocabulary-table',
  standalone: true,
  templateUrl: './vocabulary-table.component.html',
  styleUrls: ['./vocabulary-table.component.scss'],
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatExpansionModule,
    MatIconModule,
  ],
})
export class VocabularyTableComponent implements AfterViewInit {
  displayedColumns: string[] = ['index', 'word', 'meanings', 'actions'];
  dataSource: VocabularyDTO[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 10;
  expandedRow: VocabularyDTO | null = null;

  constructor(private readonly vocabularyService: VocabularyService) {}

  ngAfterViewInit(): void {
    this.loadVocabularies();
  }

  ngOnInit(): void {}

  loadVocabularies() {
    firstValueFrom(
      this.vocabularyService.getList(this.page, this.pageSize)
    ).then((response) => {
      const data = response.data;
      this.dataSource = data.items;
      this.totalCount = data.totalCount;
      console.log(this.dataSource);
    });
  }

  onPageChange(event: any): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadVocabularies();
  }

  getIndex(idx: number): number {
    return (this.page - 1) * this.pageSize + idx + 1;
  }

  onEdit(vocab: VocabularyDTO): void {
    // TODO: Implement edit action
  }

  onDelete(vocab: VocabularyDTO): void {
    // TODO: Implement delete action
  }

  toggleExpand(row: VocabularyDTO): void {
    this.expandedRow = this.expandedRow === row ? null : row;
  }
}
